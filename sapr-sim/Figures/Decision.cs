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
    public class Decision : UIEntity, ISerializable
    {
        private Rect bound;

        private Port inputPort, yesPort, noPort;

        private UIParam<String> inputProbabilityParams = new UIParam<String>("", new StringParamValidator(), "Параметры входа", "");

        private static readonly string DEFAULT_NAME = "?";

        public Decision(Canvas canvas) : base(canvas)
        {
            init();
            name.Value = DEFAULT_NAME;
        }

        public Decision(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.inputPort = info.GetValue("inputPort", typeof(Port)) as Port;
            this.yesPort = info.GetValue("yesPort", typeof(Port)) as Port;
            this.noPort = info.GetValue("noPort", typeof(Port)) as Port;
            ports.Add(inputPort);
            ports.Add(yesPort);
            ports.Add(noPort);
            
            init();
        }

        public override void createAndDraw(double x, double y)
        {
            inputPort = new Port(this, canvas, PortType.INPUT, x - 12, y + 19);
            yesPort = new Port(this, canvas, PortType.UNDEFINED, x + 19, y - 12.5);
            noPort = new Port(this, canvas, PortType.UNDEFINED, x + 19, y + 47.5);
            canvas.Children.Add(inputPort);
            canvas.Children.Add(yesPort);
            canvas.Children.Add(noPort);
            ports.Add(inputPort);
            ports.Add(yesPort);
            ports.Add(noPort);

            label = new Label(this, canvas, x + 20, y + 16, name.Value);
            canvas.Children.Add(label);
        }

        public override string iconPath()
        {
            return "Resources/decision.gif";
        }

        public override string description()
        {
            return "Блок принятия решений. Задается вероятность, при которой задача имитации" + 
                " будет передана в ветку \"ДА\". В противном случае - ветка \"НЕТ\"";
        }

        public override List<UIParam> getParams()
        {
            List<UIParam> param = base.getParams();
            param.Add(inputProbabilityParams);
            return param;
        }

        public string InputProbabilityParams
        {
            get { return inputProbabilityParams.Value; }
            set { inputProbabilityParams.Value = value; }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("inputPort", inputPort);
            info.AddValue("yesPort", yesPort);
            info.AddValue("noPort", noPort);
        }

        protected override System.Windows.Media.Geometry DefiningGeometry
        {
            get 
            {
                GeometryGroup gg = new GeometryGroup();
                gg.FillRule = FillRule.Nonzero;

                RectangleGeometry rg = new RectangleGeometry(bound);
                rg.Transform = new RotateTransform(45, bound.Width / 2, bound.Height / 2);

                gg.Children.Add(rg);
                return gg;
            }
        }

        private void init()
        {
            Fill = Brushes.LemonChiffon;
            bound = new Rect(new Size(45, 45));
        }
    }
}
