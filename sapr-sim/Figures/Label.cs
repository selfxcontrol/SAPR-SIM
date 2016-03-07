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
    public class Label : UIEntity, ISerializable
    {

        private UIEntity owner;
        private FormattedText ft;

        private static readonly string DEFAULT_NAME = "Надпись";

        public Label(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            init(info.GetValue("owner", typeof(UIEntity)) as UIEntity, info.GetString("text"));

            if (owner != null)
            {
                this.MouseLeftButtonDown -= ((MainWindow)System.Windows.Application.Current.MainWindow).Shape_MouseLeftButtonDown;
                this.MouseMove -= ((MainWindow)System.Windows.Application.Current.MainWindow).Shape_MouseMove;
                this.MouseLeftButtonUp -= ((MainWindow)System.Windows.Application.Current.MainWindow).Shape_MouseLeftButtonUp;
            }
        }

        public Label(Canvas canvas) : base(canvas)
        { 
            name.Value = DEFAULT_NAME;
            init(null, DEFAULT_NAME);
        }

        public Label(UIEntity owner, Canvas canvas, double xPos, double yPos, string text) : base(canvas)
        {
            init(owner, text);

            name.Value = text;                 
            this.canvas = canvas;

            Canvas.SetLeft(this, xPos);
            Canvas.SetTop(this, yPos);
        }

        public string Text
        {
            get { return name.Value; }
        }

        public override void createAndDraw(double x, double y)
        {            
        }

        public override string iconPath()
        {
            return "Resources/Label.gif";
        }

        public override string description()
        {
            return "Надпись";
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("text", name.Value);
            info.AddValue("owner", owner);
        }

        protected override System.Windows.Media.Geometry DefiningGeometry
        {
            get
            {
                GeometryGroup gg = new GeometryGroup();
                gg.FillRule = FillRule.EvenOdd;
                gg.Children.Add(ft.BuildGeometry(new Point()));
                return gg;
            }
        }

        private void init(UIEntity owner, string text)
        {
            ft = new FormattedText(text,
                CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface("Arial"), 12, Brushes.Black);
            label = this;
            this.owner = owner;
            ft.MaxTextHeight = 100;
            ft.MaxLineCount = 2;
            ft.MaxTextWidth = 70;
        }
    }
}
