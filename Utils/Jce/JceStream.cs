using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

using moe.berd.Utils.Jce.Fields;

namespace moe.berd.Utils.Jce
{
	public class JceStream : MemoryStream
	{
		public const int MaxStringLength = 104857600;
		public const byte Type_Byte = 0,
			Type_Short = 1,
			Type_Int = 2,
			Type_Long = 3,
			Type_Float = 4,
			Type_Double = 5,
			Type_String1 = 6,
			Type_String4 = 7,
			Type_Map = 8,
			Type_List = 9,
			Type_StructBegin = 10,
			Type_StructEnd = 11,
			Type_Zero = 12,
			Type_ByteArray = 13;

		public Encoding ServerEncoding = Encoding.GetEncoding("GBK");

		public JceStream() : base()
		{

		}

		public JceStream(byte[] data) : base(data)
		{

		}

		#region Seek Functions

		public long Seek(long offset)
		{
			return Seek(offset,SeekOrigin.Current);
		}

		public void SkipField()
		{
			Header header = ReadHeader();
			SkipField(header.Type);
		}

		public void SkipStruct()
		{
			while(true)
			{
				Header test = ReadHeader();
				if(test.Type == Type_StructEnd)
				{
					break;
				}
				SkipField(test.Type);
			}
		}

		public void SkipField(byte type)
		{
			switch(type)
			{
			case Type_Byte:
				Seek(1);
				break;
			case Type_Short:
				Seek(2);
				break;
			case Type_Int:
			case Type_Float:
				Seek(4);
				break;
			case Type_Long:
			case Type_Double:
				Seek(8);
				break;
			case Type_String1:
				int length = ReadByte();
				Seek(length < 0 ? length + 256 : length);
				break;
			case Type_String4:
				Seek(ReadInt());
				break;
			case Type_Map:
				{
					int count = Read<int>(0) * 2;
					for(int i = 0;i < count;i++)
					{
						SkipField();
					}
				}
				break;
			case Type_List:
				{
					int count = Read<int>(0);
					for(int i = 0;i < count;i++)
					{
						SkipField();
					}
				}
				break;
			case Type_StructBegin:
				SkipStruct();
				break;
			case Type_StructEnd:
			case Type_Zero:
				break;
			case Type_ByteArray:
				Header header = ReadHeader();
				if(header.Type != Type_Byte)
				{
					throw new Exception("Data incorrect in type " + Type_ByteArray + ",except " + Type_Byte + " but got " + header.Type);
				}
				Seek(Read<int>(0));
				break;
			default:
				throw new ArgumentException("Unknown type " + type + ".");
			}
		}

		public bool SkipToTag(int tag)
		{
			while(true)
			{
				Header header = PeakHeader();
				if(tag <= header.Tag || header.Type == 11)
				{
					if(tag != header.Tag)
					{
						break;
					}
					return true;
				}
				Seek(header.Count);
				SkipField(header.Type);
			}
			return false;
		}

		#endregion

		#region Read Functions

		public StructField ReadAll()
		{
			StructField result = new StructField(0);
			while(Position < Length)
			{
				try
				{
					JceField read = ReadField();
					result.Set(read.Tag,read);
				}
				catch { }
			}
			return result;
		}

		public byte[] Read(int count)
		{
			byte[] buffer = new byte[count];
			Read(buffer,0,count);
			return buffer;
		}

		public T Read<T>(int tag)
		{
			if(SkipToTag(tag))
			{
				Header header = ReadHeader();
				Type type = typeof(T);
				switch(header.Type)
				{
				case Type_Byte:
					if(type == typeof(byte))
					{
						return (T)(object)(byte)ReadByte();
					}
					else if(type == typeof(short))
					{
						return (T)(object)(short)ReadByte();
					}
					else if(type == typeof(int))
					{
						return (T)(object)ReadByte();
					}
					else if(type == typeof(long))
					{
						return (T)(object)(long)ReadByte();
					}
					break;
				case Type_Short:
					if(type == typeof(short))
					{
						return (T)(object)BitConverter.ToInt16(ReadReverse(2),0);
					}
					else if(type == typeof(int))
					{
						return (T)(object)(int)BitConverter.ToInt16(ReadReverse(2),0);
					}
					else if(type == typeof(long))
					{
						return (T)(object)(long)BitConverter.ToInt16(ReadReverse(2),0);
					}
					break;
				case Type_Int:
					if(type == typeof(int))
					{
						return (T)(object)BitConverter.ToInt32(ReadReverse(4),0);
					}
					else if(type == typeof(long))
					{
						return (T)(object)(long)BitConverter.ToInt32(ReadReverse(4),0);
					}
					break;
				case Type_Long:
					if(type == typeof(long))
					{
						return (T)(object)BitConverter.ToInt32(ReadReverse(8),0);
					}
					break;
				case Type_Float:
					if(type == typeof(float))
					{
						return (T)(object)BitConverter.ToSingle(ReadReverse(4),0);
					}
					else if(type == typeof(double))
					{
						return (T)(object)(double)BitConverter.ToSingle(ReadReverse(4),0);
					}
					break;
				case Type_Double:
					if(type == typeof(double))
					{
						return (T)(object)BitConverter.ToDouble(ReadReverse(8),0);
					}
					break;
				case Type_String1:
					if(type == typeof(string))
					{
						int length = ReadByte();
						return (T)(object)ServerEncoding.GetString(Read(length < 0 ? length + 256 : length));
					}
					break;
				case Type_String4:
					if(type == typeof(string))
					{
						int length = ReadInt();
						if(length > MaxStringLength || length < 0)
						{
							throw new IndexOutOfRangeException("String size " + length + " out of range.");
						}
						return (T)(object)ServerEncoding.GetString(Read(length));
					}
					break;
				case Type_Zero:
					if(type == typeof(byte))
					{
						return (T)(object)(byte)0;
					}
					else if(type == typeof(short))
					{
						return (T)(object)(short)0;
					}
					else if(type == typeof(int))
					{
						return (T)(object)0;
					}
					else if(type == typeof(long))
					{
						return (T)(object)(long)0;
					}
					else if(type == typeof(float))
					{
						return (T)(object)0f;
					}
					else if(type == typeof(double))
					{
						return (T)(object)0d;
					}
					break;
				}
				throw new ArgumentException("Type mismatch.");
			}
			throw new Exception("Tag not exists.");
		}

		public JceField ReadField()
		{
			Header header = PeakHeader();
			switch(header.Type)
			{
			case Type_Byte:
				return ReadField<ByteField>(header.Tag);
			case Type_Short:
				return ReadField<ShortField>(header.Tag);
			case Type_Int:
				return ReadField<IntField>(header.Tag);
			case Type_Long:
				return ReadField<LongField>(header.Tag);
			case Type_Float:
				return ReadField<FloatField>(header.Tag);
			case Type_Double:
				return ReadField<DoubleField>(header.Tag);
			case Type_String1:
			case Type_String4:
				return ReadField<StringField>(header.Tag);
			case Type_Map:
				return ReadField<MapField>(header.Tag);
			case Type_List:
				return ReadField<ListField>(header.Tag);
			case Type_StructBegin:
				return ReadField<StructField>(header.Tag);
			case Type_StructEnd:
				throw new Exception("Got struct end field without struct begin tag.");
			case Type_Zero:
				return ReadField<ZeroField>(header.Tag);
			case Type_ByteArray:
				ByteArrayField field = ReadField<ByteArrayField>(header.Tag);
				/* Maybe we should decode it by ourself.
				using(JceStream stream = new JceStream(field.Data))
				{
					try
					{
						return stream.ReadAll();
					}
					catch { }
				}
				*/
				return field;
			}
			throw new Exception("Unknown type.");
		}

		public T ReadField<T>(int tag)
		{
			Type type = typeof(T);
			if(type == typeof(ByteField))
			{
				return (T)(object)new ByteField(tag,Read<byte>(tag));
			}
			else if(type == typeof(ShortField))
			{
				return (T)(object)new ShortField(tag,Read<short>(tag));
			}
			else if(type == typeof(IntField))
			{
				return (T)(object)new IntField(tag,Read<int>(tag));
			}
			else if(type == typeof(LongField))
			{
				return (T)(object)new LongField(tag,Read<long>(tag));
			}
			else if(type == typeof(FloatField))
			{
				return (T)(object)new FloatField(tag,Read<float>(tag));
			}
			else if(type == typeof(DoubleField))
			{
				return (T)(object)new DoubleField(tag,Read<double>(tag));
			}
			else if(type == typeof(StringField))
			{
				return (T)(object)new StringField(tag,Read<string>(tag));
			}
			else
			{
				if(SkipToTag(tag))
				{
					Header header = ReadHeader();
					switch(header.Type)
					{
					case Type_Map:
						if(type == typeof(MapField))
						{
							int count = Read<int>(0);
							if(count < 0)
							{
								throw new Exception("Count invalid.");
							}
							MapField result = new MapField(header.Tag);
							for(int i = 0;i < count;i++)
							{
								result.Add(ReadField(),ReadField());
							}
							return (T)(object)result;
						}
						break;
					case Type_List:
						if(type == typeof(ListField))
						{
							int count = Read<int>(0);
							if(count < 0)
							{
								throw new Exception("Count invalid.");
							}
							ListField result = new ListField(header.Tag);
							for(int i = 0;i < count;i++)
							{
								result.Add(ReadField());
							}
							return (T)(object)result;
						}
						break;
					case Type_StructBegin:
						if(type == typeof(StructField))
						{
							StructField result = new StructField(header.Tag);
							while(true)
							{
								Header test = PeakHeader();
								if(test.Type == Type_StructEnd)
								{
									Seek(test.Count);
									break;
								}
								JceField field = ReadField();
								result.Set(field.Tag,field);
							}
							return (T)(object)result;
						}
						break;
					case Type_StructEnd:
						throw new Exception("Got struct end field without struct begin tag.");
					case Type_Zero:
						if(type == typeof(ZeroField))
						{
							return (T)(object)new ZeroField(header.Tag);
						}
						break;
					case Type_ByteArray:
						if(type == typeof(ByteArrayField))
						{
							Header check = ReadHeader();
							if(check.Type != Type_Byte)
							{
								throw new Exception("Data incorrect in type " + Type_ByteArray + ",except " + Type_Byte + " but got " + header.Type);
							}
							int length = Read<int>(0);
							if(length < 0)
							{
								throw new Exception("Length error.");
							}
							return (T)(object)new ByteArrayField(header.Tag,Read(length));
						}
						break;
					default:
						throw new ArgumentException("Unknown type " + header.Type + ".");
					}
					throw new ArgumentException("Type mismatch.");
				}
				throw new Exception("Tag not exists.");
			}
		}

		public byte[] ReadReverse(int count)
		{
			return Read(count).Reverse().ToArray();
		}

		public int ReadInt()
		{
			return BitConverter.ToInt32(ReadReverse(4),0);
		}

		protected Header PeakHeader()
		{
			Header result = ReadHeader();
			Seek(-result.Count);
			return result;
		}

		protected Header ReadHeader()
		{
			int i = ReadByte();
			Header result = new Header((i & 0xF0) >> 4,(byte)(i & 0xF));
			if(result.Tag == 15)
			{
				result.Tag = (0xFF & ReadByte());
				result.Count = 2;
			}
			else
			{
				result.Count = 1;
			}
			return result;
		}

		#endregion

		#region Write Functions

		public void WriteAll(StructField data)
		{
			foreach(JceField field in data.Data)
			{
				Write(field);
			}
		}

		public void WriteReverse(byte[] data)
		{
			Write(data.Reverse().ToArray());
		}

		public void Write(byte[] data)
		{
			Write(data,0,data.Length);
		}

		public void Write(JceField field)
		{
			if(field is ByteField)
			{
				ByteField converted = (ByteField)field;
				Write(converted.Data,converted.Tag);
			}
			else if(field is ShortField)
			{
				ShortField converted = (ShortField)field;
				Write(converted.Data,converted.Tag);
			}
			else if(field is IntField)
			{
				IntField converted = (IntField)field;
				Write(converted.Data,converted.Tag);
			}
			else if(field is LongField)
			{
				LongField converted = (LongField)field;
				Write(converted.Data,converted.Tag);
			}
			else if(field is FloatField)
			{
				FloatField converted = (FloatField)field;
				Write(converted.Data,converted.Tag);
			}
			else if(field is DoubleField)
			{
				DoubleField converted = (DoubleField)field;
				Write(converted.Data,converted.Tag);
			}
			else if(field is StringField)
			{
				StringField converted = (StringField)field;
				Write(converted.Data,converted.Tag);
			}
			else if(field is MapField)
			{
				Write((MapField)field);
			}
			else if(field is ListField)
			{
				Write((ListField)field);
			}
			else if(field is StructField)
			{
				Write((StructField)field);
			}
			else if(field is ZeroField)
			{
				Write((ZeroField)field);
			}
			else if(field is ByteArrayField)
			{
				Write((ByteArrayField)field);
			}
			else
			{
				throw new ArgumentException("Unknown field type or not implented.");
			}
		}

		public void Write(byte val,int tag)
		{
			if(val == 0x00)
			{
				WriteHeader(Type_Zero,tag);
			}
			else
			{
				WriteHeader(Type_Byte,tag);
				WriteByte(val);
			}
		}

		public void Write(short val,int tag)
		{
			if(val >= -128 && val <= 127)
			{
				Write((byte)val,tag);
			}
			else
			{
				WriteHeader(Type_Short,tag);
				WriteReverse(BitConverter.GetBytes(val));
			}
		}

		public void Write(int val,int tag)
		{
			if(val >= short.MinValue && val <= short.MaxValue)
			{
				Write((short)val,tag);
			}
			else
			{
				WriteHeader(Type_Int,tag);
				WriteReverse(BitConverter.GetBytes(val));
			}
		}

		public void Write(long val,int tag)
		{
			if(val >= int.MinValue && val <= int.MaxValue)
			{
				Write((int)val,tag);
			}
			else
			{
				WriteHeader(Type_Long,tag);
				WriteReverse(BitConverter.GetBytes(val));
			}
		}

		public void Write(float val,int tag)
		{
			WriteHeader(Type_Float,tag);
			WriteReverse(BitConverter.GetBytes(val));
		}

		public void Write(double val,int tag)
		{
			WriteHeader(Type_Double,tag);
			WriteReverse(BitConverter.GetBytes(val));
		}

		public void Write(string val,int tag)
		{
			byte[] data = ServerEncoding.GetBytes(val);
			if(data.Length <= 255)
			{
				WriteHeader(Type_String1,tag);
				WriteByte((byte)data.Length);
				Write(data);
			}
			else
			{
				WriteHeader(Type_String4,tag);
				WriteReverse(BitConverter.GetBytes(data.Length));
				Write(data);
			}
		}

		public void Write(MapField val)
		{
			WriteHeader(Type_Map,val.Tag);
			Write(val.Count,0);
			foreach(KeyValuePair<JceField,JceField> field in val)
			{
				Write(field.Key);
				Write(field.Value);
			}
		}

		public void Write(ListField val)
		{
			WriteHeader(Type_List,val.Tag);
			Write(val.Count,0);
			foreach(JceField field in val)
			{
				Write(field);
			}
		}

		public void Write(StructField val)
		{
			WriteHeader(Type_StructBegin,val.Tag);
			foreach(JceField field in val.Data)
			{
				Write(field);
			}
			WriteHeader(Type_StructEnd,0);
		}

		public void Write(ZeroField val)
		{
			WriteHeader(Type_Zero,val.Tag);
		}

		public void Write(ByteArrayField val)
		{
			WriteHeader(Type_ByteArray,val.Tag);
			WriteHeader(Type_Byte,0);
			Write(val.Data.Length,0);
			Write(val.Data);
		}

		public void WriteHexString(string val,int tag)
		{
			byte[] data = Binary.HexToBytes(val);
			if(data.Length <= 255)
			{
				WriteHeader(Type_String1,tag);
				WriteByte((byte)data.Length);
				Write(data);
			}
			else
			{
				WriteHeader(Type_String4,tag);
				WriteReverse(BitConverter.GetBytes(data.Length));
				Write(data);
			}
		}

		protected void WriteHeader(byte type,int tag)
		{
			if(tag < 15)
			{
				WriteByte((byte)(type | tag << 4));
			}
			else if(tag < 256)
			{
				WriteByte((byte)(type | 0xF0));
				WriteByte((byte)tag);
			}
			else
			{
				throw new ArgumentException("Tag " + tag + " is too large.");
			}
		}

		#endregion
	}
}
