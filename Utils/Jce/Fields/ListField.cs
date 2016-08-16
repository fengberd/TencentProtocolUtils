using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace moe.berd.Utils.Jce.Fields
{
	public class ListField : DataField<IList<JceField>>, IList<JceField>
	{
		public JceField this[int index]
		{
			get
			{
				return this.Data[index];
			}

			set
			{
				this.Data[index] = value;
			}
		}

		public int Count
		{
			get
			{
				return this.Data.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return this.Data.IsReadOnly;
			}
		}

		public ListField(int tag,IList<JceField> data = null) : base(tag,data)
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
			sb.Append("[List]\n");
			sb.Append(prefix);
			sb.Append("{\n");
			prefix += "	";
			foreach(JceField field in this)
			{
				sb.Append(field.Print(prefix));
			}
			sb.Append(prefix.Substring(1));
			sb.Append("}\n");
			return sb.ToString();
		}

		public void Add(JceField item)
		{
			this.Data.Add(item);
		}

		public void Clear()
		{
			this.Data.Clear();
		}

		public bool Contains(JceField item)
		{
			return this.Data.Contains(item);
		}

		public void CopyTo(JceField[] array,int arrayIndex)
		{
			this.Data.CopyTo(array,arrayIndex);
		}

		public IEnumerator<JceField> GetEnumerator()
		{
			return this.Data.GetEnumerator();
		}

		public int IndexOf(JceField item)
		{
			return this.Data.IndexOf(item);
		}

		public void Insert(int index,JceField item)
		{
			this.Data.Insert(index,item);
		}

		public bool Remove(JceField item)
		{
			return this.Data.Remove(item);
		}

		public void RemoveAt(int index)
		{
			this.Data.RemoveAt(index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.Data.GetEnumerator();
		}
	}
}
