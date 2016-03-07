using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sapr_sim.Utils
{
    [Serializable]
    public sealed class ResourceType : IConvertible
    {

        public static readonly ResourceType WORKER = new ResourceType(0, "Исполнитель");
        public static readonly ResourceType INSTRUMENT = new ResourceType(1, "Оборудование");
        public static readonly ResourceType MATERIAL = new ResourceType(2, "Расходник");

        public int Order { get; set; }
        public string Name { get; set; }

        public static List<IConvertible> list()
        {
            return new List<IConvertible>() { WORKER, INSTRUMENT, MATERIAL };
        }

        public static List<string> nameList()
        {
            return new List<string>() { WORKER.Name, INSTRUMENT.Name, MATERIAL.Name };
        }

        public static ResourceType byOrder(int order)
        {
            switch (order)
            {
                case 0:
                    return WORKER;
                case 1:
                    return INSTRUMENT;
                case 2:
                    return MATERIAL;
                default:
                    throw new Exception("WTF, dude!?");
            }
        }

        public ResourceType()
        {
            // for serialization only
        }

        private ResourceType(int order, string name)
        {
            this.Order = order;
            this.Name = name;
        }

        public override String ToString()
        {
            return Name;
        }


        public TypeCode GetTypeCode()
        {
            throw new NotImplementedException();
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public byte ToByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public char ToChar(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public double ToDouble(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public short ToInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public int ToInt32(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public long ToInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public float ToSingle(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public string ToString(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }
    }
}
