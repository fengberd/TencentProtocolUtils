using System;

namespace moe.berd.Utils.Jce.Fields
{
	public class ByteField : NumberField<byte>
	{
		public ByteField(int tag,byte data = default(byte)) : base(tag,data)
		{

		}

		public override string Print(string prefix = "")
		{
			return prefix + "[Byte]=>\"" + Data + "\"\n";
		}
	}
}
