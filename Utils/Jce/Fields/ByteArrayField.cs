using System;

namespace moe.berd.Utils.Jce.Fields
{
	public class ByteArrayField : DataField<byte[]>
	{
		public ByteArrayField(int tag,byte[] data = null) : base(tag,data)
		{

		}
		
		public override string Print(string prefix = "")
		{
			return prefix + "[ByteArray]=>\"" + Binary.BytesToHex(Data) + "\"\n";
		}
	}
}
