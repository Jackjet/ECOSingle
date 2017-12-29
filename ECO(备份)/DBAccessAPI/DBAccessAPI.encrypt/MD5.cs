using System;
using System.Security.Cryptography;
using System.Text;
namespace DBAccessAPI.encrypt
{
	public class MD5
	{
		public static string GenerateMD5(string str)
		{
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			byte[] bytes = Encoding.Default.GetBytes(str);
			return BitConverter.ToString(mD5CryptoServiceProvider.ComputeHash(bytes));
		}
	}
}
