using FluentMigrator;

namespace Library.Migrations
{
	[Migration(1)]
	public class InitialStructure : Migration
	{
		private const string UserTableName = "User";

		public override void Up()
		{
			if (!Schema.Table(UserTableName).Exists())
			{
				Create.Table(UserTableName)
					.WithIdColumn()
					.WithColumn("Login").AsString(255).NotNullable()
					.WithColumn("Email").AsString(1023).NotNullable()
					.WithColumn("Password").AsFixedLengthString(128).NotNullable()
					.WithColumn("PasswordSalt").AsFixedLengthString(1024).NotNullable();
			}
		}

		public override void Down()
		{
			if (Schema.Table(UserTableName).Exists())
			{
				Delete.Table(UserTableName);
			}
		}
	}
}