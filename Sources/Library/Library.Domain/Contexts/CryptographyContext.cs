using System;
using System.Security.Cryptography;
using System.Text;

namespace Library.Domain.Contexts
{
	public class CryptographyContext : ICryptographyContext
	{
		private const string GeneratedPasswordSymbols = "abcdefghjkmnpqrstwxyzABCDEFGHJKMNPQRSTWXYZ23456789$-+?&=!%{}";
		private const int GeneratedPasswordLength = 10;
		private const string ConstantSalt = "57535B1324E229E68A734F99F942E49A8C6ADB08AA1EA15602BD954636A1EA745B41AF41F1CA3D78FE3B0B98F7FF5913AFA292FE2E98AD020506E0BF439A6EB97504DD0FC40EC6E5B017C3FB28AB3FDABD3394A57EDB8089BA5D6D1D72522DA030C73944BCB94F2A07D12571A27E5E75B22E58925FCCE68476E39C0DE83CD0E00BABE052BAFC871081DD2EEAA48AB8EE5090EDE6D005D6C9BB4814E5B2C9B259638CA67F0A66EDFADEFA4946DB74527EF8F08FD375B91A7047FD4CF876BCBFC20C5BCE58DE17135EB260016485572A93609072C75E2140DBF706C5080199535D421C92AFC511F4CEB6E19F1EF24C8BBC2DC6E8C8409E35B10A1EBD36A9FB6DBAF060363C0C0C343A4CC36D36BF08BA0676B9188B1B1234884D6D564632773D9A8281A3CA1C8B711C75D1860EDF04891B36CB9617264B4BA2103106C38E1D38FE21476150889CD46BD2A7322AF8AC9A6DAB40F2D645D196E5CACE33BFA0C3CED8AD21ACA13F25F6594EE0B01DC70D0CD940B3FD6D3DA3249E3814288C616C6A090B8B65085DFDA85ECB6496D0590C08E4CA189CB9E7167BE66EB17305AF18BC82271BEEE6E45E94264855D8DB4A527F144DB22A09AFA9CCF333E2ED8BF6244338CF22B2D1EC469044DEDE56E35954E7A69F093F357D94625DF832108D04ADBB0FB9EC379CFC77E2D4B9EF863E1295E919BD5D772D9AD33177E17F20622A27575E";
		// it generates 2 symbols for every byte so length has to be divided by 2.
		private const int SaltLength = 1024 / 2;

		public string Hash(string input)
		{
			using (SHA512 sha512 = new SHA512Managed())
			{
				return BytesToHex(sha512.ComputeHash(GetASCIIBytes(input)));
			}
		}

		public string Hmac(string input, string key)
		{
			using (HMACSHA512 hmacsha512 = new HMACSHA512(HexToBytes(key)))
			{
				return BytesToHex(hmacsha512.ComputeHash(GetASCIIBytes(input)));
			}
		}

		/// <summary>
		/// Generates string of random hex characters.
		/// </summary>
		/// <returns>Random hex characters</returns>
		public string GenerateRandomSalt()
		{
			byte[] data = new byte[SaltLength];
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

			rng.GetNonZeroBytes(data);
			return BytesToHex(data);
		}

		public string EncodePassword(string password, string randomSalt)
		{
			string hashedPassword = Hash(password);
			string randomSaltedPassword = Hmac(hashedPassword, randomSalt);
			return Hmac(randomSaltedPassword, ConstantSalt);
		}

		public string GetRandomPassword()
		{
			char[] password = new char[GeneratedPasswordLength];
			for (int i = 0; i < GeneratedPasswordLength; i++)
			{
				password[i] = GeneratedPasswordSymbols[GetRandom().Next(GeneratedPasswordSymbols.Length)];
			}
			return password.ToString();
		}

		private static byte[] GetASCIIBytes(string input)
		{
			return Encoding.ASCII.GetBytes(input);
		}

		private static byte[] HexToBytes(string input)
		{
			int charsNum = input.Length;
			byte[] bytes = new byte[charsNum / 2];

			for (int i = 0; i < charsNum; i += 2)
			{
				bytes[i / 2] = Convert.ToByte(input.Substring(i, 2), 16);
			}
			return bytes;
		}

		private static string BytesToHex(byte[] input)
		{
			return BitConverter.ToString(input).Replace("-", String.Empty);
		}

		/// <summary>
		/// Gets a random object with a real random seed
		/// </summary>
		private static Random GetRandom()
		{
			byte[] randomBytes = new byte[4];

			new RNGCryptoServiceProvider().GetBytes(randomBytes);

			// Convert 4 bytes into a 32-bit integer value.
			int seed = (randomBytes[0] & 0x7f) << 24 |
						randomBytes[1] << 16 |
						randomBytes[2] << 8 |
						randomBytes[3];
			return new Random(seed);
		}
	}
}