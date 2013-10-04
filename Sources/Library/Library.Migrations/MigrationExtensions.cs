using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using FluentMigrator;
using FluentMigrator.Builders.Create.Table;

namespace Library.Migrations
{
	public static class MigrationExtensions
	{
		private const string SetIdentityInsert = "SET IDENTITY_INSERT [{0}] ";
		private const string InsertCommand = "INSERT INTO [{0}] ({1}) VALUES ({2})";

		public static ICreateTableColumnOptionOrWithColumnSyntax WithIdColumn(this ICreateTableWithColumnSyntax tableWithColumnSyntax)
		{
			return tableWithColumnSyntax
			.WithColumn("Id")
			.AsInt32()
			.NotNullable()
			.PrimaryKey()
			.Identity();
		}

		public static void InsertWithIdentity(this Migration migration, string tableName, params object[] dataAsAnonymousTypes)
		{
			InsertWithIdentity(migration, tableName, String.Empty, dataAsAnonymousTypes);
		}

		public static void InsertWithIdentity(this Migration migration, string tableName, string schemaName, params object[] dataAsAnonymousTypes)
		{
			migration.Execute.WithConnection((connection, transaction) =>
			{
				SetIdentityInsertTo(transaction, tableName, "ON");
				foreach (object dataAsAnonymousType in dataAsAnonymousTypes)
				{
					InsertObject(transaction, tableName, dataAsAnonymousType);
				}
				SetIdentityInsertTo(transaction, tableName, "OFF");
			});
		}

		private static void SetIdentityInsertTo(IDbTransaction transaction, string tableName, string to)
		{
			using (IDbCommand cmd = transaction.Connection.CreateCommand())
			{
				cmd.Transaction = transaction;
				cmd.CommandText = String.Format(SetIdentityInsert + to, tableName);
				cmd.ExecuteNonQuery();
			}
		}

		private static void InsertObject(IDbTransaction transaction, string tableName, object dataAsAnonymousType)
		{
			try
			{
				using (IDbCommand cmd = transaction.Connection.CreateCommand())
				{
					StringBuilder fieldNames = new StringBuilder();
					StringBuilder fieldValues = new StringBuilder();
					int length = dataAsAnonymousType.GetType().GetProperties().Length;

					for (int i = 0; i < length; i++)
					{
						PropertyInfo property = dataAsAnonymousType.GetType().GetProperties()[i];

						fieldNames.AppendFormat("[{0}]", property.Name);
						fieldValues.AppendFormat("@{0}", property.Name);

						IDbDataParameter parameter = cmd.CreateParameter();
						parameter.ParameterName = "@" + property.Name;
						parameter.Value = property.GetValue(dataAsAnonymousType);
						cmd.Parameters.Add(parameter);

						if (i != length - 1)
						{
							fieldNames.Append(", ");
							fieldValues.Append(", ");
						}
					}

					cmd.Transaction = transaction;
					cmd.CommandText = String.Format(InsertCommand, tableName, fieldNames, fieldValues);
					cmd.ExecuteNonQuery();
				}
			}
			catch (SqlException e)
			{
				// Do not catch PK violation exceptions
				if (e.Number != 2627)
				{
					throw;
				}
			}
		}
	}
}