using CommonAPI;
using System;
using System.IO;
using System.Net;
namespace DBAccessAPI
{
	public class FTP
	{
		private string host;
		private string user;
		private string pass;
		private int port;
		private FtpWebRequest ftpRequest;
		private FtpWebResponse ftpResponse;
		private Stream ftpStream;
		private int bufferSize = 2048;
		public FTP(string hostIP, int iport, string userName, string password)
		{
			this.host = hostIP;
			this.user = userName;
			this.pass = password;
			this.port = iport;
		}
		public void download(string remoteFile, string localFile)
		{
			try
			{
				this.ftpRequest = (FtpWebRequest)WebRequest.Create(string.Concat(new object[]
				{
					"ftp://",
					this.host,
					":",
					this.port,
					"/",
					remoteFile
				}));
				this.ftpRequest.Credentials = new NetworkCredential(this.user, this.pass);
				this.ftpRequest.UseBinary = true;
				this.ftpRequest.UsePassive = true;
				this.ftpRequest.KeepAlive = true;
				this.ftpRequest.Method = "RETR";
				this.ftpResponse = (FtpWebResponse)this.ftpRequest.GetResponse();
				this.ftpStream = this.ftpResponse.GetResponseStream();
				FileStream fileStream = new FileStream(localFile, FileMode.Create);
				byte[] buffer = new byte[this.bufferSize];
				int i = this.ftpStream.Read(buffer, 0, this.bufferSize);
				try
				{
					while (i > 0)
					{
						fileStream.Write(buffer, 0, i);
						i = this.ftpStream.Read(buffer, 0, this.bufferSize);
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}
				fileStream.Close();
				this.ftpStream.Close();
				this.ftpResponse.Close();
				this.ftpRequest = null;
			}
			catch (Exception ex2)
			{
				Console.WriteLine(ex2.ToString());
			}
		}
		public void upload(string remoteFile, string localFile)
		{
			try
			{
				this.ftpRequest = (FtpWebRequest)WebRequest.Create(string.Concat(new object[]
				{
					"ftp://",
					this.host,
					":",
					this.port,
					"/",
					remoteFile
				}));
				this.ftpRequest.Credentials = new NetworkCredential(this.user, this.pass);
				this.ftpRequest.UseBinary = true;
				this.ftpRequest.UsePassive = true;
				this.ftpRequest.KeepAlive = true;
				this.ftpRequest.Method = "STOR";
				this.ftpStream = this.ftpRequest.GetRequestStream();
				FileStream fileStream = new FileStream(localFile, FileMode.Open);
				byte[] buffer = new byte[this.bufferSize];
				int count = fileStream.Read(buffer, 0, this.bufferSize);
				try
				{
					while (count != 0)
					{
						this.ftpStream.Write(buffer, 0, count);
						count = fileStream.Read(buffer, 0, this.bufferSize);
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}
				fileStream.Close();
				this.ftpStream.Close();
				this.ftpRequest = null;
			}
			catch (Exception ex2)
			{
				Console.WriteLine(ex2.ToString());
			}
		}
		public void delete(string deleteFile)
		{
			try
			{
				this.ftpRequest = (FtpWebRequest)WebRequest.Create(string.Concat(new object[]
				{
					"ftp://",
					this.host,
					":",
					this.port,
					"/",
					deleteFile
				}));
				this.ftpRequest.Credentials = new NetworkCredential(this.user, this.pass);
				this.ftpRequest.UseBinary = true;
				this.ftpRequest.UsePassive = true;
				this.ftpRequest.KeepAlive = true;
				this.ftpRequest.Method = "DELE";
				this.ftpResponse = (FtpWebResponse)this.ftpRequest.GetResponse();
				this.ftpResponse.Close();
				this.ftpRequest = null;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
		public void rename(string currentFileNameAndPath, string newFileName)
		{
			try
			{
				this.ftpRequest = (FtpWebRequest)WebRequest.Create(string.Concat(new object[]
				{
					"ftp://",
					this.host,
					":",
					this.port,
					"/",
					currentFileNameAndPath
				}));
				this.ftpRequest.Credentials = new NetworkCredential(this.user, this.pass);
				this.ftpRequest.UseBinary = true;
				this.ftpRequest.UsePassive = true;
				this.ftpRequest.KeepAlive = true;
				this.ftpRequest.Method = "RENAME";
				this.ftpRequest.RenameTo = newFileName;
				this.ftpResponse = (FtpWebResponse)this.ftpRequest.GetResponse();
				this.ftpResponse.Close();
				this.ftpRequest = null;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
		public void createDirectory(string newDirectory)
		{
			try
			{
				this.ftpRequest = (FtpWebRequest)WebRequest.Create(string.Concat(new object[]
				{
					"ftp://",
					this.host,
					":",
					this.port,
					"/",
					newDirectory
				}));
				this.ftpRequest.Credentials = new NetworkCredential(this.user, this.pass);
				this.ftpRequest.UseBinary = true;
				this.ftpRequest.UsePassive = true;
				this.ftpRequest.KeepAlive = true;
				this.ftpRequest.Method = "MKD";
				this.ftpResponse = (FtpWebResponse)this.ftpRequest.GetResponse();
				this.ftpResponse.Close();
				this.ftpRequest = null;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
		public string getFileCreatedDateTime(string fileName)
		{
			try
			{
				this.ftpRequest = (FtpWebRequest)WebRequest.Create(string.Concat(new object[]
				{
					"ftp://",
					this.host,
					":",
					this.port,
					"/",
					fileName
				}));
				this.ftpRequest.Credentials = new NetworkCredential(this.user, this.pass);
				this.ftpRequest.UseBinary = true;
				this.ftpRequest.UsePassive = true;
				this.ftpRequest.KeepAlive = true;
				this.ftpRequest.Method = "MDTM";
				this.ftpResponse = (FtpWebResponse)this.ftpRequest.GetResponse();
				this.ftpStream = this.ftpResponse.GetResponseStream();
				StreamReader streamReader = new StreamReader(this.ftpStream);
				string result = null;
				try
				{
					result = streamReader.ReadToEnd();
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}
				streamReader.Close();
				this.ftpStream.Close();
				this.ftpResponse.Close();
				this.ftpRequest = null;
				return result;
			}
			catch (Exception ex2)
			{
				Console.WriteLine(ex2.ToString());
			}
			return "";
		}
		public string getFileSize(string fileName)
		{
			try
			{
				this.ftpRequest = (FtpWebRequest)WebRequest.Create(string.Concat(new object[]
				{
					"ftp://",
					this.host,
					":",
					this.port,
					"/",
					fileName
				}));
				this.ftpRequest.Credentials = new NetworkCredential(this.user, this.pass);
				this.ftpRequest.UseBinary = true;
				this.ftpRequest.UsePassive = true;
				this.ftpRequest.KeepAlive = true;
				this.ftpRequest.Method = "SIZE";
				this.ftpResponse = (FtpWebResponse)this.ftpRequest.GetResponse();
				this.ftpStream = this.ftpResponse.GetResponseStream();
				StreamReader streamReader = new StreamReader(this.ftpStream);
				string result = null;
				try
				{
					while (streamReader.Peek() != -1)
					{
						result = streamReader.ReadToEnd();
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}
				streamReader.Close();
				this.ftpStream.Close();
				this.ftpResponse.Close();
				this.ftpRequest = null;
				return result;
			}
			catch (Exception ex2)
			{
				Console.WriteLine(ex2.ToString());
			}
			return "";
		}
		public string[] directoryListSimple(string directory)
		{
			try
			{
				this.ftpRequest = (FtpWebRequest)WebRequest.Create(string.Concat(new object[]
				{
					"ftp://",
					this.host,
					":",
					this.port,
					"/",
					directory
				}));
				this.ftpRequest.Credentials = new NetworkCredential(this.user, this.pass);
				this.ftpRequest.UseBinary = true;
				this.ftpRequest.UsePassive = true;
				this.ftpRequest.KeepAlive = true;
				this.ftpRequest.Method = "NLST";
				this.ftpResponse = (FtpWebResponse)this.ftpRequest.GetResponse();
				this.ftpStream = this.ftpResponse.GetResponseStream();
				StreamReader streamReader = new StreamReader(this.ftpStream);
				string text = null;
				try
				{
					while (streamReader.Peek() != -1)
					{
						text = text + streamReader.ReadLine() + "|";
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}
				streamReader.Close();
				this.ftpStream.Close();
				this.ftpResponse.Close();
				this.ftpRequest = null;
				try
				{
					return text.Split("|".ToCharArray());
				}
				catch (Exception ex2)
				{
					Console.WriteLine(ex2.ToString());
				}
			}
			catch (Exception ex3)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex3.Message + "\n" + ex3.StackTrace);
				Console.WriteLine(ex3.ToString());
			}
			return new string[]
			{
				""
			};
		}
		public string[] directoryListDetailed(string directory)
		{
			try
			{
				this.ftpRequest = (FtpWebRequest)WebRequest.Create(string.Concat(new object[]
				{
					"ftp://",
					this.host,
					":",
					this.port,
					"/",
					directory
				}));
				this.ftpRequest.Credentials = new NetworkCredential(this.user, this.pass);
				this.ftpRequest.UseBinary = true;
				this.ftpRequest.UsePassive = true;
				this.ftpRequest.KeepAlive = true;
				this.ftpRequest.Method = "LIST";
				this.ftpResponse = (FtpWebResponse)this.ftpRequest.GetResponse();
				this.ftpStream = this.ftpResponse.GetResponseStream();
				StreamReader streamReader = new StreamReader(this.ftpStream);
				string text = null;
				try
				{
					while (streamReader.Peek() != -1)
					{
						text = text + streamReader.ReadLine() + "|";
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}
				streamReader.Close();
				this.ftpStream.Close();
				this.ftpResponse.Close();
				this.ftpRequest = null;
				try
				{
					return text.Split("|".ToCharArray());
				}
				catch (Exception ex2)
				{
					Console.WriteLine(ex2.ToString());
				}
			}
			catch (Exception ex3)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex3.Message + "\n" + ex3.StackTrace);
				Console.WriteLine(ex3.ToString());
			}
			return new string[]
			{
				""
			};
		}
	}
}
