using sapr_sim.Utils;
using sapr_sim.WPFCustomElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace sapr_sim.Parameters
{

    // Are C# generics better than Java generics? hmm...
    // http://stackoverflow.com/questions/353126/c-sharp-multiple-generic-types-in-one-list    
    [Serializable]
    public abstract class UIParam
    {
        protected ParamValidator validator;
        protected string displayedText;
        protected string tooltip;

        // default - textbox
        [NonSerialized]
        protected ParameterInput control;

        public ParamValidator Validator
        {
            get { return validator; }
        }

        public string DisplayedText
        {
            get { return displayedText; }
        }

        public string ToolTip
        {
            get { return tooltip; }
        }

        public ParameterInput ContentControl
        {
            get { return control; }
            set { this.control = value; }
        }

        public abstract IConvertible RawValue { get; set; }
    }

    // for T we should use next types:
    //      Boolean
    //      Byte
    //      Char
    //      DateTime
    //      Decimal
    //      Double
    //      Int (16, 32 and 64-bit)
    //      SByte
    //      Single (float)
    //      String
    //      UInt (16, 32 and 64-bit)
    // http://stackoverflow.com/questions/8745444/c-sharp-generic-constraints-to-include-value-types-and-strings
    [Serializable]
    public class UIParam<T> : UIParam where T : IConvertible 
    {
        
        private T value;

        public UIParam(T value, ParamValidator validator, string displayedText, string tooltip)
        {
            this.value = value;
            this.validator = validator;
            this.displayedText = displayedText;
            this.tooltip = tooltip;
        }

        public UIParam(T value, ParamValidator validator, string displayedText, string tooltip, ParameterInput input) 
            : this(value, validator, displayedText, tooltip)
        {
            this.control = input;
        }

        public T Value
        {
            get { return value; }
            set { this.value = value; }
        }

        // FU C#!!!
        public override IConvertible RawValue
        {
            get { return value; }
            set
            {
                Type valueType = this.value.GetType();
                if (valueType == typeof(string))                    
                    this.value = (T) (object) Convert.ToString(value);
                if (valueType == typeof(Double))
                    this.value = (T) (object)Convert.ToDouble(value);
                if (valueType == typeof(Boolean))
                    this.value = (T)(object)Convert.ToBoolean(value);
                if (valueType == typeof(Int32))
                    this.value = (T)(object)Convert.ToInt32(value);
                if (valueType == typeof(TimeMeasure) || valueType == typeof(ResourceType))
                    this.value = (T)(object)value;
                if (valueType == typeof(TimeParam))
                {
                    TimeParam tp = this.value as TimeParam;
                    tp.Time = Convert.ToInt32(value);
                    this.value = (T)(object)tp;
                }
            }
        }

    }
}
