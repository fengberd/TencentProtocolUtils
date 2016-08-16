using System;

namespace moe.berd.Utils.Jce.Fields
{
	public class FloatField : NumberField<float>
	{
		public FloatField(int tag,float data = default(float)) : base(tag,data)
		{

		}

		public override string Print(string prefix = "")
		{
			return prefix + "[Float]=>\"" + Data + "\"\n";
		}
	}
}
