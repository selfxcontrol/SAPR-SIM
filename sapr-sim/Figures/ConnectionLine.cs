using sapr_sim.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace sapr_sim.Figures
{
    [Serializable]
    public sealed class ConnectionLine : UIEntity, ISerializable
    {

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(Point), typeof(ConnectionLine),
                new FrameworkPropertyMetadata(default(Point)));

        public ConnectionLine(Canvas cancas) : base(cancas)
        {
        }

        public ConnectionLine(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            SetBinding(SourceProperty, new Binding()
            {
                Source = info.GetValue("SourcePort", typeof(Port)) as Port,
                Path = new PropertyPath(Port.AnchorPointProperty)
            });

            SetBinding(DestinationProperty, new Binding()
            {
                Source = info.GetValue("DestinationPort", typeof(Port)) as Port,
                Path = new PropertyPath(Port.AnchorPointProperty)
            });

            this.MouseMove -= ((MainWindow)System.Windows.Application.Current.MainWindow).Shape_MouseMove; 
        }

        public static readonly DependencyProperty DestinationProperty =
            DependencyProperty.Register(
                "Destination", typeof(Point), typeof(ConnectionLine),
                    new FrameworkPropertyMetadata(default(Point)));

        public Port SourcePort
        { 
            get 
            {
                BindingExpression be = GetBindingExpression(SourceProperty);
                return be != null ? be.DataItem as Port : null;
            } 
        }

        public Port DestinationPort
        {
            get
            {
                BindingExpression be = GetBindingExpression(DestinationProperty);
                return be != null ? be.DataItem as Port : null;
            }
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

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("SourcePort", SourcePort);
            info.AddValue("DestinationPort", DestinationPort);
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                LineSegment segment = new LineSegment(default(Point), true);
                PathFigure figure = new PathFigure(default(Point), new[] { segment }, false);
                PathGeometry geometry = new PathGeometry(new[] { figure });
                
                BindingBase sourceBinding = new Binding { Source = this, Path = new PropertyPath(SourceProperty) };
                BindingBase destinationBinding = new Binding { Source = this, Path = new PropertyPath(DestinationProperty) };
                BindingOperations.SetBinding(figure, PathFigure.StartPointProperty, sourceBinding);
                BindingOperations.SetBinding(segment, LineSegment.PointProperty, destinationBinding);

                LineDefinition ld = new LineDefinition(SourcePort.Owner, DestinationPort.Owner);

                StrokeThickness = ld.Thickness;
                Stroke = ld.Color;

                return geometry;
            }
        }

        private class LineDefinition
        {
            private int thickness = 3;
            private Brush brush = Brushes.Black;

            public LineDefinition(UIEntity from, UIEntity to)
            {
                if (from is Resource || to is Resource)
                {
                    thickness = 3;
                    brush = from is Resource && to is Resource ? Brushes.DeepSkyBlue : Brushes.Violet;
                }
            }

            public int Thickness
            {
                get { return thickness; }
            }

            public Brush Color
            {
                get { return brush; }
            }
        }
    }
}
