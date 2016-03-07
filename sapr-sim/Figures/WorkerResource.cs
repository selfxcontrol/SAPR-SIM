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
    public class WorkerResource : Resource, ISerializable
    {

        private Port bottomPort;

        private UIParam<Double> efficiency = new UIParam<Double>(1, new BetweenDoubleParamValidator(0.0, 1.0), "Производительность",
            "Производительность исполнителя. Может принимать вещественное значение на отрезке [0; 1]");
        private UIParam<Double> price = new UIParam<Double>(1, new BetweenDoubleParamValidator(0.0, 100000.0), "Зарплата (у.е)",
            "Зарплата исполнителя. Может принимать вещественное значение на отрезке [0; 100000]");

        public WorkerResource(Canvas canvas) : base(canvas)
        {
            init();
            name.Value = ResourceType.WORKER.Name;
            type.Value = ResourceType.WORKER;
        }

        public WorkerResource(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.bottomPort = info.GetValue("bottomPort", typeof(Port)) as Port;
            ports.Add(bottomPort);

            efficiency = info.GetValue("efficiency", typeof(UIParam<Double>)) as UIParam<Double>;
            price = info.GetValue("price", typeof(UIParam<Double>)) as UIParam<Double>;

            init();
        }

        public override void createAndDraw(double x, double y) 
        {
            base.createAndDraw(x, y);
            bottomPort = new Port(this, canvas, PortType.BOTTOM_RESOURCE, x + 42.5, y + 56.5);
            canvas.Children.Add(bottomPort);
            ports.Add(bottomPort);

            label = new Label(this, canvas, x + 14, y + 22, name.Value);
            canvas.Children.Add(label);
        }

        public override List<UIParam> getParams()
        {
            List<UIParam> param = base.getParams();
            param.Add(efficiency);
            param.Add(price);
            return param;
        }

        public double Efficiency
        {
            get { return efficiency.Value; }
            set { efficiency.Value = value; }
        }

        public double Price
        {
            get { return price.Value; }
            set { price.Value = value; }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("bottomPort", bottomPort);
            info.AddValue("efficiency", efficiency);
            info.AddValue("price", price);
        }

        protected override void init()
        {
            base.init();
            Fill = Brushes.Gray;
        }
    }
}
