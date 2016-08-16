using System;

namespace moe.berd.Utils.Jce.Fields
{
	public class ShortField : NumberField<short>
	{
		public ShortField(int tag,short data = default(short)) : base(tag,data)
		{

		}

		public override string Print(string prefix = "")
		{
			return prefix + "[Short]=>\"" + Data + "\"\n";
		}
	}
}
