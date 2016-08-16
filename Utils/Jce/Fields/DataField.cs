using System;

namespace moe.berd.Utils.Jce.Fields
{
	public abstract class DataField<T> : JceField
	{
		public T Data;

		public DataField(int tag,T data = default(T)) : base(tag)
		{
			Data = data;
		}

		public T GetData()
		{
			return Data;
		}

		public void SetData(T data)
		{
			this.Data = data;
		}
	}
}
