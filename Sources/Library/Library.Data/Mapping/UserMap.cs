using System.Data.Entity.ModelConfiguration;
using Library.Data.Entities;

namespace Library.Data.Mapping
{
	public class UserMap : EntityTypeConfiguration<User>
	{
		public UserMap()
		{
			// Primary Key
			HasKey(t => t.Id);

			// Properties
			Property(t => t.Login).IsRequired().HasMaxLength(255);
			Property(t => t.Email).IsRequired().HasMaxLength(1023);
			Property(t => t.Password).IsRequired().IsFixedLength().HasMaxLength(128);
			Property(t => t.PasswordSalt).IsRequired().IsFixedLength().HasMaxLength(1024);

			// Table & Column Mappings
			ToTable("User");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.Login).HasColumnName("Login");
			Property(t => t.Email).HasColumnName("Email");
			Property(t => t.Password).HasColumnName("Password");
			Property(t => t.PasswordSalt).HasColumnName("PasswordSalt");
		}
	}
}