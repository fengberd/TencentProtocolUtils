using System;

namespace moe.berd.Utils.Jce.Fields
{
	public class ZeroField : NumberField<byte>
	{
		public new byte Data
		{
			get
			{
				return 0;
			}
			
			set
			{
				if(value != 0)
				{
					throw new Exception("Can't set value of zero field.");
				}
			}
		}

		public ZeroField(int tag,byte data = default(byte)) : base(tag,data)
		{

		}

		public override string Print(string prefix = "")
		{
			return prefix + "[Zero]\n";
		}
	}
}
