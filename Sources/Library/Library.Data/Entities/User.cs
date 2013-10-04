namespace Library.Data.Entities
{
	public class User : EntityWithId
	{
		public string Login { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string PasswordSalt { get; set; }
	}
}