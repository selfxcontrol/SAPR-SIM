using sapr_sim.Parameters;
using sapr_sim.Utils;
using sapr_sim.WPFCustomElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace sapr_sim.Figures
{
    [Serializable]
    public abstract class UIEntity : Shape, ISerializable, IEqualityComparer<UIEntity>
    {

        public static readonly string ENTITY_NAME_PARAM = "Имя";

        public Canvas canvas { get; set; }

        protected Dictionary<UIEntity, CoordinatesHandler> coordinates = new Dictionary<UIEntity, CoordinatesHandler>();
        protected List<Port> ports = new List<Port>();

        protected Label label;

        private string state = "original";

        protected UIParam<String> name = new UIParam<String>("Сущность", new StringParamValidator(), ENTITY_NAME_PARAM, "Отображаемое название блока");

        protected long id {get; set;}

        public UIEntity(SerializationInfo info, StreamingContext context)
        {                      
            try
            {
                init(info.GetValue("canvas", typeof(ScrollableCanvas)) as Canvas);

                this.name = info.GetValue("textParam", typeof(UIParam<String>)) as UIParam<String>;
                Canvas.SetLeft(this, info.GetDouble("x"));
                Canvas.SetTop(this, info.GetDouble("y"));
                ((MainWindow)System.Windows.Application.Current.MainWindow).attachMovingEvents(this);

                // label can be not found - it's normal behavior
                this.label = info.GetValue("label", typeof(Label)) as Label;

                this.state = "copyed";
            }
            catch (SerializationException e) {
                Console.Out.Write(e.StackTrace);
            }
        }

        protected UIEntity(Canvas canvas)
        {
            init(canvas);
        }

        public void defaultBitmapEffect()
        {
            // http://stackoverflow.com/questions/4022746/wpf-add-a-dropshadow-effect-to-an-element-from-code-behind
            DropShadowBitmapEffect myDropShadowEffect = new DropShadowBitmapEffect();
            // Set the color of the shadow to Black.
            Color myShadowColor = new Color();
            myShadowColor.ScA = 1;
            myShadowColor.ScB = 0;
            myShadowColor.ScG = 0;
            myShadowColor.ScR = 0;
            myDropShadowEffect.Color = myShadowColor;

            // Set the direction of where the shadow is cast to 320 degrees.
            myDropShadowEffect.Direction = 320;

            // Set the depth of the shadow being cast.
            myDropShadowEffect.ShadowDepth = 10;

            // Set the shadow softness to the maximum (range of 0-1).
            myDropShadowEffect.Softness = 0.5;
            // Set the shadow opacity to half opaque or in other words - half transparent.
            // The range is 0-1.
            myDropShadowEffect.Opacity = 0.5;

            BitmapEffect = myDropShadowEffect;
        }

        public void errorBitmapEffect()
        {
            BitmapEffect = new DropShadowBitmapEffect() { ShadowDepth = 10, Color = Colors.Red };
        }

        public void selectedBitmapEffect()
        {
            BitmapEffect = new DropShadowBitmapEffect() { ShadowDepth = 5, Color = Colors.Black };
        }

        public void putMovingCoordinate(UIEntity entity, double xShape, double yShape, double xCanvas, double yCanvas)
        {
            // avoiding 'System.ArgumentException' for existing key... double facepalm!!!
            coordinates.Remove(entity);
            coordinates.Add(entity, new CoordinatesHandler(xShape, yShape, xCanvas, yCanvas));
        }

        public CoordinatesHandler getMovingCoordinate(UIEntity entity)
        {
            return coordinates[entity];
        }

        public void updateText(string newText)
        {
            if (label != null)
            {
                canvas.Children.Remove(label);
                CoordinatesHandler ch = getMovingCoordinate(label);
                label = new Label(this, canvas, ch.xShape, ch.yShape, newText);
                
                // dirty hack :(
                if (this is Label) ((MainWindow)System.Windows.Application.Current.MainWindow).attachMovingEvents(label);

                canvas.Children.Add(label);
                ZIndexUtil.setCorrectZIndex(canvas, this);
            }             
        }

        public long Id 
        { 
            get { return id; } 
        }

        public Label Label
        {
            get { return label; }
        }

        public string EntityName
        {
            get { return name.Value; }
            set { name.Value = value; }
        }

        public bool ParametersExist
        {
            get { return getParams() != null && getParams().Count > 0; }
        }

        public List<Port> getPorts()
        {
            return ports;
        }

        public void removeAll()
        {
            foreach (Port p in ports)
            {
                if (canvas.Children.Contains(p))
                    canvas.Children.Remove(p);
            }
            ports.Clear();

            canvas.Children.Remove(label);
        }

        public abstract void createAndDraw(double x, double y);

        // duplicate! from App.xaml
        public abstract string iconPath();
        public abstract string description();

        public virtual List<UIParam> getParams()
        {
            List<UIParam> param = new List<UIParam>();
            param.Add(name);
            return param;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("canvas", canvas);
            info.AddValue("label", label);
            info.AddValue("textParam", name);

            if (VisualTreeHelper.GetOffset(this).X == 0.0 &&  VisualTreeHelper.GetOffset(this).Y == 0.0 &&
                Canvas.GetTop(this) != double.NaN && Canvas.GetLeft(this) != double.NaN)
            {
                info.AddValue("x", Canvas.GetLeft(this));
                info.AddValue("y", Canvas.GetTop(this));
            }
            else
            {
                info.AddValue("x", VisualTreeHelper.GetOffset(this).X);
                info.AddValue("y", VisualTreeHelper.GetOffset(this).Y);
            }  
        }

        public Port findPort(PortType type)
        {
            foreach (Port port in ports)
                if (port.PortType.Equals(type))
                    return port;
            return null;
        }

        private void init(Canvas canvas)
        {
            this.canvas = canvas;
            StrokeThickness = 1;
            Stroke = Brushes.Black;
            Fill = Brushes.LemonChiffon;
            defaultBitmapEffect();
            id = IdGenerator.GenerateId();
        }

        public struct CoordinatesHandler
        {
            public double xShape, yShape, xCanvas, yCanvas;

            public CoordinatesHandler(double xShape, double yShape, double xCanvas, double yCanvas)
            {
                this.xShape = xShape;
                this.yShape = yShape;
                this.xCanvas = xCanvas;
                this.yCanvas = yCanvas;
            }
        }

        public bool Equals(UIEntity x, UIEntity y)
        {
            return x.id == y.id;
        }

        public int GetHashCode(UIEntity obj)
        {
            return obj.GetHashCode();
        }
        

    }
}
