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
    public class Collector : UIEntity, ISerializable
    {
        private Rect bound;

        private Port inputPort, outputPort, backPort;

        public Collector(Canvas canvas) : base(canvas)
        {
            init();
        }

        public Collector(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.inputPort = info.GetValue("inputPort", typeof(Port)) as Port;
            this.outputPort = info.GetValue("outputPort", typeof(Port)) as Port;
            this.backPort = info.GetValue("backPort", typeof(Port)) as Port;
            ports.Add(inputPort);
            ports.Add(outputPort);
            ports.Add(backPort);
            
            init();
        }

        public override void createAndDraw(double x, double y)
        {
            inputPort = new Port(this, canvas, PortType.INPUT, x - 12, y + 19);
            outputPort = new Port(this, canvas, PortType.OUTPUT, x + 47.5, y + 19);
            backPort = new Port(this, canvas, PortType.UNDEFINED, x + 19, y - 12.5);
            canvas.Children.Add(inputPort);
            canvas.Children.Add(outputPort);
            canvas.Children.Add(backPort);
            ports.Add(inputPort);
            ports.Add(outputPort);
            ports.Add(backPort);
        }

        public override string iconPath()
        {
            return "Resources/Collector.gif";
        }

        public override string description()
        {
            return "Блок объединения сущностей";
        }

        public override List<UIParam> getParams()
        {
            return new List<UIParam>();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("inputPort", inputPort);
            info.AddValue("outputPort", outputPort);
            info.AddValue("backPort", backPort);
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
            bound = new Rect(new Size(45, 45));
            RenderTransform = new RotateTransform(45, bound.Width / 2, bound.Height / 2);
        }

    }
}
