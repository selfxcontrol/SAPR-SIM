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
    public class Sync : UIEntity, ISerializable
    {
        private Rect bound;

        private Port inputPort1, inputPort2, outputPort;

        private static readonly string DEFAULT_NAME = "Синхронизация";

        public Sync(Canvas canvas) : base(canvas)
        {
            init();
            name.Value = DEFAULT_NAME;
        }

        public Sync(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.inputPort1 = info.GetValue("inputPort1", typeof(Port)) as Port;
            this.inputPort2 = info.GetValue("inputPort2", typeof(Port)) as Port;
            this.outputPort = info.GetValue("outputPort", typeof(Port)) as Port;
            ports.Add(inputPort1);
            ports.Add(inputPort2);
            ports.Add(outputPort);
            
            init();
        }

        public override void createAndDraw(double x, double y)
        {
            inputPort1 = new Port(this, canvas, PortType.INPUT, x - 4, y + 22.5);
            inputPort2 = new Port(this, canvas, PortType.INPUT, x - 4, y + 67.5);
            outputPort = new Port(this, canvas, PortType.OUTPUT, x + 7, y + 45);
            canvas.Children.Add(inputPort1);
            canvas.Children.Add(inputPort2);
            canvas.Children.Add(outputPort);
            ports.Add(inputPort1); 
            ports.Add(inputPort2);
            ports.Add(outputPort);

            label = new Label(this, canvas, x - 30, y - 20, name.Value);
        }

        public override string iconPath()
        {
            return "Resources/Sync.gif";
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
            info.AddValue("inputPort1", inputPort1);
            info.AddValue("inputPort2", inputPort2);
            info.AddValue("outputPort", outputPort);
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
