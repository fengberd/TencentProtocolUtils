using System;

namespace moe.berd.Utils.Jce.Fields
{
	public class DoubleField : NumberField<double>
	{
		public DoubleField(int tag,double data = default(double)) : base(tag,data)
		{

		}

		public override string Print(string prefix = "")
		{
			return prefix + "[Double]=>\"" + Data + "\"\n";
		}
	}
}
