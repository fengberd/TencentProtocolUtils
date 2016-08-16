using System;

namespace moe.berd.Utils.Jce
{
	public class Header
	{
		public byte Type = 0;
		public int Tag = 0, Count = 0;

		public Header(int tag = 0,byte type = 0)
		{
			Tag = tag;
			Type = type;
		}
	}
}
