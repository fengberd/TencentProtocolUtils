using System.IO;
using System.Text;

namespace moe.berd.Utils.Jce.Fields
{
	public abstract class JceField
	{
		public int Tag;

		public abstract string Print(string prefix = "");

		public JceField(int tag)
		{
			Tag = tag;
		}

		public void Print(Stream stream,string prefix = "")
		{
			byte[] buffer = Encoding.UTF8.GetBytes(Print(prefix));
			stream.Write(buffer,0,buffer.Length);
		}
	}
}
