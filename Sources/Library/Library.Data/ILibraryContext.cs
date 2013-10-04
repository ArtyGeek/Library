using System.Data.Entity;
using Library.Data.Entities;

namespace Library.Data
{
	public interface ILibraryContext
	{
		DbSet<User> Users { get; set; }
		int SaveChanges();
	}
}