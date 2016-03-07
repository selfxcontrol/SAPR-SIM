using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace sapr_sim.WPFCustomElements
{

    public sealed class ParameterConstants
    {
        public static readonly int WIDTH = 400;
        public static readonly int QUICK_PANEL_WIDTH = 100;

        private ParameterConstants() { }
    }

    public interface ParameterInput : ICloneable
    {
        void setValue(IConvertible c);
        IConvertible getValue();
    }

    public class ParameterTextBox : TextBox, ParameterInput
    {

        public ParameterTextBox(IConvertible value, bool enabled)
        {
            Text = value.ToString();
            HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            MaxWidth = ParameterConstants.WIDTH;
            MinWidth = ParameterConstants.WIDTH;
            IsEnabled = enabled;
        }

        public void setValue(IConvertible c)
        {
            Text = c.ToString();
        }

        public IConvertible getValue()
        {
            return Text;
        }

        public object Clone()
        {
            return new ParameterTextBox(Text, IsEnabled);
        }

    }

    public class ParameterCheckBox : CheckBox, ParameterInput
    {

        public ParameterCheckBox()
        {
            HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            MaxWidth = ParameterConstants.WIDTH;
            MinWidth = ParameterConstants.WIDTH;
        }

        public ParameterCheckBox(IConvertible value) : this()
        {
            IsChecked = Convert.ToBoolean(value);
        }

        public ParameterCheckBox(IConvertible value, bool enabled) : this(value)
        {
            IsEnabled = enabled;
        }

        public void setValue(IConvertible c)
        {
            IsChecked = Convert.ToBoolean(c);
        }

        public IConvertible getValue()
        {
            return IsChecked;
        }

        public object Clone()
        {
            return new ParameterCheckBox(IsChecked, IsEnabled);
        }
    }

    public class ParameterComboBox : ComboBox, ParameterInput
    {

        private ParameterComboBox()
        {
            HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            MaxWidth = ParameterConstants.WIDTH;
            MinWidth = ParameterConstants.WIDTH;
        }

        public ParameterComboBox(List<IConvertible> values) : this()
        {
            foreach (IConvertible value in values)
                Items.Add(value);
        }

        public ParameterComboBox(List<IConvertible> values, bool enabled) : this(values)
        {
            IsEnabled = enabled;
        }

        public void setValue(IConvertible value)
        {
            SelectedItem = value;
        }

        public IConvertible getValue()
        {
            return SelectedItem as IConvertible;
        }

        public object Clone()
        {
            List<IConvertible> res = new List<IConvertible>();
            foreach (var item in Items)
                res.Add(item as IConvertible);
            ParameterComboBox pcb = new ParameterComboBox(res, IsEnabled);
            pcb.setValue(this.getValue());
            return pcb;
        }
    }
    
}
