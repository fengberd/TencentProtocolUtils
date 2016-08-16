using System;

namespace moe.berd.Utils.Jce.Fields
{
	public class LongField : NumberField<long>
	{
		public LongField(int tag,long data = default(long)) : base(tag,data)
		{

		}

		public override string Print(string prefix = "")
		{
			return prefix + "[Long]=>\"" + Data + "\"\n";
		}
	}
}
