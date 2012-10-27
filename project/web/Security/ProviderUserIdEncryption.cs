using System;
using System.IO;
using System.Linq;
using System.Web.Security;
using Bobasoft.Security;

namespace Bobasoft.Web.Security
{
	public class ProviderUserIdEncryption : IEncryption<string, string,string>
	{
		//======================================================
		#region _Public methods_

		public string Encrypt(string param1, string param2)
		{
			using (var memoryStream = new MemoryStream())
			using (var binaryWriter = new BinaryWriter(memoryStream))
			{
				binaryWriter.Write(param1);
				binaryWriter.Write(param2);
				binaryWriter.Flush();
				var data = new byte[memoryStream.Length + Padding.Length];
				Buffer.BlockCopy(Padding, 0, data, 0, Padding.Length);
				Buffer.BlockCopy(memoryStream.GetBuffer(), 0, data, Padding.Length, (int) memoryStream.Length);
				return MachineKey.Encode(data, MachineKeyProtection.All);
			}
		}

		public bool TryDecrypt(string input, out string param1, out string param2)
		{
			param1 = null;
			param2 = null;
			if (string.IsNullOrEmpty(input))
				return false;

			var buffer = MachineKey.Decode(input, MachineKeyProtection.All);
			if (buffer.Length < Padding.Length)
				return false;
			if (Padding.Where((t, index) => t != buffer[index]).Any())
				return false;

			using (var memoryStream = new MemoryStream(buffer, Padding.Length, buffer.Length - Padding.Length))
			using (var binaryReader = new BinaryReader(memoryStream))
			{
				try
				{
					var str1 = binaryReader.ReadString();
					var str2 = binaryReader.ReadString();
					if (memoryStream.ReadByte() == -1)
					{
						param1 = str1;
						param2 = str2;
						return true;
					}
				}
				catch (Exception)
				{
				}
			}

			return false;
		}

		#endregion

		//======================================================
		#region _Private, protected, internal fields_

		private static readonly byte[] Padding = new byte[]
			                                 {
				                                 133,
				                                 197,
				                                 101,
				                                 114
			                                 };

		#endregion
	}
}