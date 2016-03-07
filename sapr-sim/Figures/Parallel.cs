using sapr_sim.Parameters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace sapr_sim.Figures
{
    [Serializable]
    public class Parallel : UIEntity, ISerializable
    {
        private Rect bound;
        private Port inputPort, outputPort1, outputPort2;

        private static readonly string DEFAULT_NAME = "Распараллеливание";

        public Parallel(Canvas canvas) : base(canvas)
        {
            init();
            name.Value = DEFAULT_NAME;
        }

        public Parallel(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.inputPort = info.GetValue("inputPort", typeof(Port)) as Port;
            this.outputPort1 = info.GetValue("outputPort1", typeof(Port)) as Port;
            this.outputPort2 = info.GetValue("outputPort2", typeof(Port)) as Port;
            ports.Add(inputPort);
            ports.Add(outputPort1);
            ports.Add(outputPort2);
            
            init();
        }

        public override void createAndDraw(double x, double y)
        {
            inputPort = new Port(this, canvas, PortType.INPUT, x - 4, y + 45);
            outputPort1 = new Port(this, canvas, PortType.OUTPUT, x + 7, y + 22.5);
            outputPort2 = new Port(this, canvas, PortType.OUTPUT, x + 7, y + 67.5);
            canvas.Children.Add(inputPort);
            canvas.Children.Add(outputPort1);
            canvas.Children.Add(outputPort2);
            ports.Add(inputPort);
            ports.Add(outputPort1);
            ports.Add(outputPort2);

            label = new Label(this, canvas, x - 45, y - 20, name.Value);
        }

        public override string iconPath()
        {
            return "Resources/Parallel.gif";
        }

        public override string description()
        {
            return "";
        }

        public override List<UIParam> getParams()
        {
            return null;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("inputPort", inputPort);
            info.AddValue("outputPort1", outputPort1);
            info.AddValue("outputPort2", outputPort2);
        }

        protected override System.Windows.Media.Geometry DefiningGeometry
        {
            get 
            {
                GeometryGroup gg = new GeometryGroup();
                gg.FillRule = FillRule.Nonzero;
                gg.Children.Add(new RectangleGeometry(bound));
                return gg;
            }
        }

        private void init()
        {
            Fill = Brushes.Black;
            StrokeThickness = .5;
            bound = new Rect(new Size(10, 90));
        }
    }
}
