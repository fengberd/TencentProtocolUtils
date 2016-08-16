using System;
using System.IO;
using System.Threading;

using moe.berd.Utils;
using moe.berd.Utils.Jce;
using moe.berd.Utils.Jce.Fields;

namespace moe.berd.Utils.Test
{
	class Program
	{
		static void Main(string[] args)
		{
			File.WriteAllBytes("F:/wtf.bin",Binary.HexToBytes("10 02 2C 3C 4C 56 09 4B 51 51 43 6F 6E 66 69 67 66 0C 53 69 67 6E 61 74 75 72 65 52 65 71 7D 00 00 5A 08 00 01 06 03 72 65 71 18 00 01 06 16 4B 51 51 43 6F 6E 66 69 67 2E 53 69 67 6E 61 74 75 72 65 52 65 71 1D 00 00 33 0A 19 00 01 06 20 62 65 39 31 30 61 66 33 39 61 32 36 61 34 61 39 39 32 63 36 66 64 30 31 61 31 34 33 65 64 31 39 22 20 02 AE F1 39 00 01 06 01 30 4C 0B 8C 98 0C A8 0C"));
			JceStream stream = new JceStream(Binary.HexToBytes("10 02 2C 3C 42 01 B2 CD 43 56 03 53 53 4F 66 13 53 76 63 52 65 71 43 68 65 63 6B 41 70 70 49 44 4E 65 77 7D 00 00 32 08 00 01 06 03 72 65 73 18 00 01 06 1A 4B 51 51 43 6F 6E 66 69 67 2E 47 72 61 79 55 69 6E 43 68 65 63 6B 52 65 73 70 1D 00 00 07 0A 1C 2C 3D 00 0C 0B 8C 98 0C A8 0C"));
			StructField data = stream.ReadAll();
			stream.Dispose();
			stream = new JceStream();
			stream.WriteAll(data);
			Console.WriteLine(Binary.BytesToHex(stream.ToArray()));
			while(true)
			{
				Thread.Sleep(50);
			}
		}
	}
}
