using System;
using System.Globalization;


namespace moe.berd.Utils
{
	public static class Binary
	{
		public static byte[] HexToBytes(string hex)
		{
			hex = hex.Replace(" ",string.Empty).Replace("\r",string.Empty).Replace("\n",string.Empty).Replace("　",string.Empty);
			if(hex.Length % 2 == 1)
			{
				throw new Exception("Wrong data length");
			}
			byte[] result = new byte[hex.Length / 2];
			for(int i = 0;i < result.Length;i++)
			{
				result[i] = byte.Parse(hex.Substring(i * 2,2),NumberStyles.HexNumber,CultureInfo.InvariantCulture);
			}
			return result;
		}

		public static string BytesToHex(byte[] data)
		{
			return BitConverter.ToString(data).Replace('-',' ');
		}
	}
}
