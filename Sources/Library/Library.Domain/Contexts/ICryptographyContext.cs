namespace Library.Domain.Contexts
{
	public interface ICryptographyContext
	{
		string Hash(string input);
		string Hmac(string input, string key);
		string GenerateRandomSalt();
		string EncodePassword(string input, string randomSalt);
		string GetRandomPassword();
	}
}