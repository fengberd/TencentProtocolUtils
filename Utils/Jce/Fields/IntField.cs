using System;

namespace moe.berd.Utils.Jce.Fields
{
	public class IntField : NumberField<int>
	{
		public IntField(int tag,int data = default(int)) : base(tag,data)
		{

		}

		public override string Print(string prefix = "")
		{
			return prefix + "[Int]=>\"" + Data + "\"\n";
		}
	}
}
