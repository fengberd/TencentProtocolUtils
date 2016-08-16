using System;

namespace moe.berd.Utils.Jce.Fields
{
	public class StringField : DataField<string>
	{
		public StringField(int tag,string data = null) : base(tag,data)
		{

		}

		public override string Print(string prefix = "")
		{
			return prefix + "[String]=>\"" + Data.Replace("\\","\\\\").Replace("\n","\\n").Replace("\r","\\r").Replace("\"","\\\"") + "\"\n";
		}
	}
}
