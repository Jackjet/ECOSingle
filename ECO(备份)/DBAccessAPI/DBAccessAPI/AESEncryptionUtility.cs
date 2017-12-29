using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace DBAccessAPI
{
	public class AESEncryptionUtility
	{
		private static int Main(string[] args)
		{
			if (args.Length < 2)
			{
				Console.WriteLine("Usage: AESEncryptionUtility infile outFile");
				return 1;
			}
			string text = args[0];
			string text2 = args[1];
			AESEncryptionUtility.Encrypt(text, text2, "test");
			AESEncryptionUtility.Decrypt(text2, "_decrypted" + text, "test");
			return 0;
		}
		public static byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
		{
			MemoryStream memoryStream = new MemoryStream();
			Rijndael rijndael = Rijndael.Create();
			rijndael.Key = Key;
			rijndael.IV = IV;
			CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write);
			cryptoStream.Write(clearData, 0, clearData.Length);
			cryptoStream.Close();
			return memoryStream.ToArray();
		}
		public static string Encrypt(string clearText, string Password)
		{
			byte[] bytes = Encoding.Unicode.GetBytes(clearText);
			PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(Password, new byte[]
			{
				73,
				118,
				97,
				110,
				32,
				77,
				101,
				100,
				118,
				101,
				100,
				101,
				118
			});
			byte[] inArray = AESEncryptionUtility.Encrypt(bytes, passwordDeriveBytes.GetBytes(32), passwordDeriveBytes.GetBytes(16));
			return Convert.ToBase64String(inArray);
		}
		public static byte[] Encrypt(byte[] clearData, string Password)
		{
			PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(Password, new byte[]
			{
				73,
				118,
				97,
				110,
				32,
				77,
				101,
				100,
				118,
				101,
				100,
				101,
				118
			});
			return AESEncryptionUtility.Encrypt(clearData, passwordDeriveBytes.GetBytes(32), passwordDeriveBytes.GetBytes(16));
		}
		public static void Encrypt(string fileIn, string fileOut, string Password)
		{
			FileStream fileStream = new FileStream(fileIn, FileMode.Open, FileAccess.Read);
			FileStream stream = new FileStream(fileOut, FileMode.OpenOrCreate, FileAccess.Write);
			PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(Password, new byte[]
			{
				73,
				118,
				97,
				110,
				32,
				77,
				101,
				100,
				118,
				101,
				100,
				101,
				118
			});
			Rijndael rijndael = Rijndael.Create();
			rijndael.Key = passwordDeriveBytes.GetBytes(32);
			rijndael.IV = passwordDeriveBytes.GetBytes(16);
			CryptoStream cryptoStream = new CryptoStream(stream, rijndael.CreateEncryptor(), CryptoStreamMode.Write);
			int num = 4096;
			byte[] buffer = new byte[num];
			int num2;
			do
			{
				num2 = fileStream.Read(buffer, 0, num);
				cryptoStream.Write(buffer, 0, num2);
			}
			while (num2 != 0);
			cryptoStream.Close();
			fileStream.Close();
		}
		public static void Encrypt(byte[] b_head, string fileIn, string fileOut, string Password)
		{
			FileStream fileStream = new FileStream(fileIn, FileMode.Open, FileAccess.Read);
			FileStream fileStream2 = new FileStream(fileOut, FileMode.OpenOrCreate, FileAccess.Write);
			fileStream2.Write(b_head, 0, b_head.Length);
			PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(Password, new byte[]
			{
				73,
				118,
				97,
				110,
				32,
				77,
				101,
				100,
				118,
				101,
				100,
				101,
				118
			});
			Rijndael rijndael = Rijndael.Create();
			rijndael.Key = passwordDeriveBytes.GetBytes(32);
			rijndael.IV = passwordDeriveBytes.GetBytes(16);
			CryptoStream cryptoStream = new CryptoStream(fileStream2, rijndael.CreateEncryptor(), CryptoStreamMode.Write);
			int num = 4096;
			byte[] buffer = new byte[num];
			int num2;
			do
			{
				num2 = fileStream.Read(buffer, 0, num);
				cryptoStream.Write(buffer, 0, num2);
			}
			while (num2 != 0);
			cryptoStream.Close();
			fileStream.Close();
		}
		public static byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
		{
			MemoryStream memoryStream = new MemoryStream();
			Rijndael rijndael = Rijndael.Create();
			rijndael.Key = Key;
			rijndael.IV = IV;
			CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndael.CreateDecryptor(), CryptoStreamMode.Write);
			cryptoStream.Write(cipherData, 0, cipherData.Length);
			cryptoStream.Close();
			return memoryStream.ToArray();
		}
		public static string Decrypt(string cipherText, string Password)
		{
			byte[] cipherData = Convert.FromBase64String(cipherText);
			PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(Password, new byte[]
			{
				73,
				118,
				97,
				110,
				32,
				77,
				101,
				100,
				118,
				101,
				100,
				101,
				118
			});
			byte[] bytes = AESEncryptionUtility.Decrypt(cipherData, passwordDeriveBytes.GetBytes(32), passwordDeriveBytes.GetBytes(16));
			return Encoding.Unicode.GetString(bytes);
		}
		public static byte[] Decrypt(byte[] cipherData, string Password)
		{
			PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(Password, new byte[]
			{
				73,
				118,
				97,
				110,
				32,
				77,
				101,
				100,
				118,
				101,
				100,
				101,
				118
			});
			return AESEncryptionUtility.Decrypt(cipherData, passwordDeriveBytes.GetBytes(32), passwordDeriveBytes.GetBytes(16));
		}
		public static void Decrypt(string fileIn, string fileOut, string Password)
		{
			FileStream fileStream = null;
			FileStream fileStream2 = null;
			CryptoStream cryptoStream = null;
			try
			{
				fileStream = new FileStream(fileIn, FileMode.Open, FileAccess.Read);
				fileStream2 = new FileStream(fileOut, FileMode.OpenOrCreate, FileAccess.Write);
				PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(Password, new byte[]
				{
					73,
					118,
					97,
					110,
					32,
					77,
					101,
					100,
					118,
					101,
					100,
					101,
					118
				});
				Rijndael rijndael = Rijndael.Create();
				rijndael.Key = passwordDeriveBytes.GetBytes(32);
				rijndael.IV = passwordDeriveBytes.GetBytes(16);
				cryptoStream = new CryptoStream(fileStream2, rijndael.CreateDecryptor(), CryptoStreamMode.Write);
				int num = 4096;
				byte[] buffer = new byte[num];
				int num3;
				do
				{
					int num2 = AccessDBUpdate.GetPercent(fileStream.Position, fileStream.Length);
					double value = 0.01 * (double)num2 * 40.0;
					num2 = Convert.ToInt32(value);
					if (num2 > 40)
					{
						num2 = 40;
					}
					if (num2 < 1)
					{
						num2 = 1;
					}
					DBTools.ProgramBar_Percent = num2;
					num3 = fileStream.Read(buffer, 0, num);
					cryptoStream.Write(buffer, 0, num3);
				}
				while (num3 != 0);
				cryptoStream.Close();
				fileStream.Close();
				DBTools.ProgramBar_Percent = 40;
			}
			catch (Exception ex)
			{
				try
				{
					if (cryptoStream != null)
					{
						cryptoStream.Close();
					}
				}
				catch
				{
				}
				try
				{
					if (fileStream != null)
					{
						fileStream.Close();
					}
				}
				catch
				{
				}
				try
				{
					if (fileStream2 != null)
					{
						fileStream2.Close();
					}
				}
				catch
				{
				}
				throw ex;
			}
		}
		public static void Decrypt(int i_length, string fileIn, string fileOut, string Password)
		{
			FileStream fileStream = null;
			FileStream fileStream2 = null;
			CryptoStream cryptoStream = null;
			try
			{
				fileStream = new FileStream(fileIn, FileMode.Open, FileAccess.Read);
				fileStream2 = new FileStream(fileOut, FileMode.OpenOrCreate, FileAccess.Write);
				PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(Password, new byte[]
				{
					73,
					118,
					97,
					110,
					32,
					77,
					101,
					100,
					118,
					101,
					100,
					101,
					118
				});
				Rijndael rijndael = Rijndael.Create();
				rijndael.Key = passwordDeriveBytes.GetBytes(32);
				rijndael.IV = passwordDeriveBytes.GetBytes(16);
				cryptoStream = new CryptoStream(fileStream2, rijndael.CreateDecryptor(), CryptoStreamMode.Write);
				byte[] buffer = new byte[i_length * 1024];
				int num = 4096;
				byte[] buffer2 = new byte[num];
				int num2 = fileStream.Read(buffer, 0, i_length * 1024);
				do
				{
					int num3 = AccessDBUpdate.GetPercent(fileStream.Position, fileStream.Length);
					double value = 0.01 * (double)num3 * 40.0;
					num3 = Convert.ToInt32(value);
					if (num3 > 40)
					{
						num3 = 40;
					}
					if (num3 < 1)
					{
						num3 = 1;
					}
					DBTools.ProgramBar_Percent = num3;
					num2 = fileStream.Read(buffer2, 0, num);
					cryptoStream.Write(buffer2, 0, num2);
				}
				while (num2 != 0);
				cryptoStream.Close();
				fileStream.Close();
				DBTools.ProgramBar_Percent = 40;
			}
			catch (Exception ex)
			{
				try
				{
					if (cryptoStream != null)
					{
						cryptoStream.Close();
					}
				}
				catch
				{
				}
				try
				{
					if (fileStream != null)
					{
						fileStream.Close();
					}
				}
				catch
				{
				}
				try
				{
					if (fileStream2 != null)
					{
						fileStream2.Close();
					}
				}
				catch
				{
				}
				throw ex;
			}
		}
	}
}
