using System.Text;
using System.Collections.Generic;

namespace moe.berd.Utils.Jce.Fields
{
	public class StructField : DataField<IList<JceField>>
	{
		public StructField(int tag,IList<JceField> data = null) : base(tag,data)
		{
			if(data == null)
			{
				Data = new List<JceField>();
			}
		}

		public override string Print(string prefix = "")
		{
			return Print(prefix,false);
		}

		public string Print(string prefix = "",bool ignoreFirst = false)
		{
			StringBuilder sb = new StringBuilder();
			if(!ignoreFirst)
			{
				sb.Append(prefix);
			}
			sb.Append("[Struct]\n");
			sb.Append(prefix);
			sb.Append("{\n");
			prefix += "	";
			foreach(JceField field in Data)
			{
				sb.Append(field.Print(prefix));
			}
			sb.Append(prefix.Substring(1));
			sb.Append("}\n");
			return sb.ToString();
		}

		public JceField GetField(int tag)
		{
			foreach(JceField field in Data)
			{
				if(field.Tag == tag)
				{
					return field;
				}
			}
			return null;
		}

		public void Set(int tag,JceField val)
		{
#warning TODO:Sort by tag
			for(int i = 0;i < Data.Count;i++)
			{
				if(Data[i].Tag == tag)
				{
					Data[i] = val;
					return;
				}
			}
			Data.Add(val);
		}
	}
}
