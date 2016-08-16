using System.Reflection;

namespace moe.berd.Utils.Jce.Fields
{
	public abstract class NumberField<Number> : DataField<Number>
	{
		public NumberField(int tag,Number data = default(Number)) : base(tag)
		{
			this.Data = data;
		}
	}
}
