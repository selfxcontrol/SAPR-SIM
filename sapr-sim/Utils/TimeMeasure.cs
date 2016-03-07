using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sapr_sim.Utils
{
    [Serializable]
    public sealed class TimeMeasure : IConvertible
    {

        public static readonly TimeMeasure SECOND = new TimeMeasure(0, "сек.");
        public static readonly TimeMeasure MINUTE = new TimeMeasure(1, "мин.");
        public static readonly TimeMeasure HOUR = new TimeMeasure(2, "час.");
        public static readonly TimeMeasure DAY = new TimeMeasure(3, "дн.");

        public int Order { get; set; }
        public string Name {get; set; }

        public static List<IConvertible> list()
        {
            return new List<IConvertible>() { SECOND, MINUTE, HOUR, DAY };
        }

        public static TimeMeasure byOrder(int order)
        {
            switch(order)
            {
                case 0:
                    return SECOND;
                case 1:
                    return MINUTE;
                case 2:
                    return HOUR;
                case 3:
                    return DAY;
                default:
                    throw new Exception("WTF, dude!?");
            }
        }

        public TimeMeasure()
        {
            // for serialization only
        }

        private TimeMeasure(int order, string name)
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
