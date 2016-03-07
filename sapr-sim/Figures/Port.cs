using sapr_sim.Parameters;
using System;
using System.Collections.Generic;
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
    public enum PortType
    {
        INPUT,
        OUTPUT,
        TOP_RESOURCE,
        BOTTOM_RESOURCE,
        UNDEFINED
    }

    [Serializable]
    public class Port : UIEntity, ISerializable
    {

        public static readonly DependencyProperty AnchorPointProperty =
                DependencyProperty.Register(
                    "AnchorPoint", typeof(Point), typeof(Port),
                        new FrameworkPropertyMetadata(new Point(0, 0),
                        FrameworkPropertyMetadataOptions.AffectsMeasure));

        private Rect bound;
        private UIEntity owner;
        private PortType portType;

        public Port(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            init(info.GetValue("owner", typeof(UIEntity)) as UIEntity);
            this.portType = (PortType) info.GetValue("portType", typeof(PortType));
            this.MouseMove -= ((MainWindow)System.Windows.Application.Current.MainWindow).Shape_MouseMove;            
        }

        public Port(UIEntity owner, Canvas canvas, PortType portType, double xPos, double yPos) : base(canvas)
        {
            init(owner);
            this.canvas = canvas;
            this.portType = portType;

            Canvas.SetLeft(this, xPos);
            Canvas.SetTop(this, yPos);
        }

        public override void createAndDraw(double x, double y)
        {            
        }

        public override string iconPath()
        {
            return null;
        }

        public override string description()
        {
            return "";
        }

        public override List<UIParam> getParams()
        {
            return null;
        }

        public UIEntity Owner
        {
            get { return owner; }
        }

        public PortType PortType
        {
            get { return portType; }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("owner", owner);
            info.AddValue("portType", portType);
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                return new EllipseGeometry(bound);
            }
        }

        protected void UIEntity_LayoutUpdated(object sender, EventArgs e)
        {
            if (canvas != null)
            {
                Size size = RenderSize;
                Point ofs = new Point(size.Width / 2, size.Height / 2);

                // TODO why ofs with X=0.0 & Y=0.0 doesn't work?
                if (ofs.X == 0.0 && ofs.Y == 0.0) return;

                // TODO for safety removing shapes from canvas
                if (this.Parent == null) return;

                SetValue(AnchorPointProperty, TransformToVisual(canvas).Transform(ofs));
            }
        }

        private void init(UIEntity owner)
        {
            Fill = Brushes.Red;
            bound = new Rect(new Size(7.5,7.5));
            this.owner = owner;
            this.LayoutUpdated += UIEntity_LayoutUpdated;
        }

    }
}
