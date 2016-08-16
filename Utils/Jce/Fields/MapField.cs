using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace moe.berd.Utils.Jce.Fields
{
	public class MapField : DataField<IDictionary<JceField,JceField>>, IDictionary<JceField,JceField>
	{
		public JceField this[JceField key]
		{
			get
			{
				return this.Data[key];
			}

			set
			{
				this.Data[key] = value;
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

		public ICollection<JceField> Keys
		{
			get
			{
				return this.Data.Keys;
			}
		}

		public ICollection<JceField> Values
		{
			get
			{
				return this.Data.Values;
			}
		}

		public MapField(int tag,IDictionary<JceField,JceField> data = null) : base(tag,data)
		{
			if(data == null)
			{
				Data = new Dictionary<JceField,JceField>();
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
			sb.Append("[Map]\n");
			sb.Append(prefix);
			sb.Append("{\n");
			prefix += "	";
			foreach(KeyValuePair<JceField,JceField> field in this)
			{
				string tmp = field.Key.Print(prefix);
				sb.Append(tmp.Substring(0,tmp.Length - 1));
				sb.Append(':');
				if(field.Value is MapField)
				{
					sb.Append(((MapField)field.Value).Print(prefix,true));
				}
				else if(field.Value is ListField)
				{
					sb.Append(((ListField)field.Value).Print(prefix,true));
				}
				else if(field.Value is StructField)
				{
					sb.Append(((StructField)field.Value).Print(prefix,true));
				}
				else
				{
					sb.Append(field.Value.Print(""));
				}
			}
			sb.Append(prefix.Substring(1));
			sb.Append("}\n");
			return sb.ToString();
		}

		public void Add(KeyValuePair<JceField,JceField> item)
		{
			this.Data.Add(item);
		}

		public void Add(JceField key,JceField value)
		{
			this.Data.Add(key,value);
		}

		public void Clear()
		{
			this.Data.Clear();
		}

		public bool Contains(KeyValuePair<JceField,JceField> item)
		{
			return this.Data.Contains(item);
		}

		public bool ContainsKey(JceField key)
		{
			return this.Data.ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<JceField,JceField>[] array,int arrayIndex)
		{
			this.Data.CopyTo(array,arrayIndex);
		}

		public IEnumerator<KeyValuePair<JceField,JceField>> GetEnumerator()
		{
			return this.Data.GetEnumerator();
		}

		public bool Remove(KeyValuePair<JceField,JceField> item)
		{
			return this.Data.Remove(item);
		}

		public bool Remove(JceField key)
		{
			return this.Data.Remove(key);
		}

		public bool TryGetValue(JceField key,out JceField value)
		{
			return this.Data.TryGetValue(key,out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.Data.GetEnumerator();
		}
	}
}
