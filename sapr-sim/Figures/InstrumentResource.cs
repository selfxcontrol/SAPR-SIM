using sapr_sim.Parameters;
using sapr_sim.Parameters.Validators;
using sapr_sim.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace sapr_sim.Figures
{
    [Serializable]
    public class InstrumentResource : Resource, ISerializable
    {

        private Port bottomPort;

        private UIParam<Double> power = new UIParam<Double>(1, new BetweenDoubleParamValidator(0.0, 1000.0), "Мощность", 
            "Мощность данного оборудования. Может принимать вещественное значение на отрезке [0; 1000]");
        private UIParam<Double> price = new UIParam<Double>(1, new BetweenDoubleParamValidator(0.0, 1000.0), "Цена",
            "Цена данного оборудования. Может принимать вещественное значение на отрезке [0; 1000]");

        public InstrumentResource(Canvas canvas) : base(canvas)
        {
            init();
            name.Value = ResourceType.INSTRUMENT.Name;
            type.Value = ResourceType.INSTRUMENT;
        }

        public InstrumentResource(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.bottomPort = info.GetValue("bottomPort", typeof(Port)) as Port;
            ports.Add(bottomPort);

            price = info.GetValue("price", typeof(UIParam<Double>)) as UIParam<Double>;
            power = info.GetValue("power", typeof(UIParam<Double>)) as UIParam<Double>;

            init();
        }

        public override void createAndDraw(double x, double y)
        {
            base.createAndDraw(x, y);
            bottomPort = new Port(this, canvas, PortType.BOTTOM_RESOURCE, x + 42.5, y + 56.5);
            canvas.Children.Add(bottomPort);
            ports.Add(bottomPort);

            label = new Label(this, canvas, x + 10, y + 22, name.Value);
            canvas.Children.Add(label);
        }

        public override List<UIParam> getParams()
        {
            List<UIParam> param = base.getParams();
            param.Add(price);
            param.Add(power);
            return param;
        }

        public double Price
        {
            get { return price.Value; }
            set { price.Value = value; }
        }

        public double Power
        {
            get { return power.Value; }
            set { power.Value = value; }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("bottomPort", bottomPort);
            info.AddValue("price", price);
            info.AddValue("power", power);
        }

        protected override void init()
        {
            base.init();
            Fill = Brushes.Honeydew;
        }
    }
}
