using System.Data.Entity;
using Library.Data.Entities;
using Library.Data.Mapping;

namespace Library.Data
{
	public class LibraryContext : DbContext, ILibraryContext
	{
		static LibraryContext()
		{
			Database.SetInitializer<LibraryContext>(null);
		}

		public LibraryContext()
			: base("Name=LibraryConnectionString")
		{
		}

		public DbSet<User> Users { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Configurations.Add(new UserMap());
		}
	}
}