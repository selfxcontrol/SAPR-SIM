using sapr_sim.Parameters;
using sapr_sim.Parameters.Validators;
using sapr_sim.Utils;
using sapr_sim.WPFCustomElements;
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
using System.Windows.Shapes;

namespace sapr_sim.Figures
{
    [Serializable]
    public class Procedure : UIEntity, ISerializable
    {
        // not used in current version...
        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register("Size", typeof(Double), typeof(Procedure));

        private Rect bound;
        private Port inputPort, outputPort, resourcePort;

        private UIParam<Double> time = new UIParam<Double>(1, new PositiveDoubleParamValidator(), "Продолжительность",
            "Временная продолжительность процедуры. Может принимать положительное целочисленное значение");
        private UIParam<TimeMeasure> timeMeasure = new UIParam<TimeMeasure>(TimeMeasure.SECOND, new DefaultParamValidator(), "Единицы измерения",
            "Единица измерения времени", new ParameterComboBox(TimeMeasure.list()));
        private UIParam<Int32> scatter = new UIParam<Int32>(0, new BetweenIntegerParamValidator(0, 100), "Разброс (%)",
            "Вводит стохастичность в процедуры. Продолжительность процедуры начинает измеряться по формуле: \"Продолжительность\" ± \"Разброс\". " + 
            "Может принимать целочисленное значение на отрезке [0; 100]");

        private static readonly string DEFAULT_NAME = "Процедура";

        public Procedure(Canvas canvas) : base(canvas)
        {
            init();
            name.Value = DEFAULT_NAME;
        }

        public Procedure(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.inputPort = info.GetValue("inputPort", typeof(Port)) as Port;
            this.outputPort = info.GetValue("outputPort", typeof(Port)) as Port;
            this.resourcePort = info.GetValue("resourcePort", typeof(Port)) as Port;
            ports.Add(inputPort);
            ports.Add(outputPort);
            ports.Add(resourcePort);

            time = info.GetValue("time", typeof(UIParam<Double>)) as UIParam<Double>;
            scatter = info.GetValue("scatter", typeof(UIParam<Int32>)) as UIParam<Int32>;

            timeMeasure = info.GetValue("timeMeasure", typeof(UIParam<TimeMeasure>)) as UIParam<TimeMeasure>;
            timeMeasure.ContentControl = new ParameterComboBox(TimeMeasure.list()) { SelectedIndex = timeMeasure.Value.Order };

            init();
        }

        public double Size
        {
            get { return (double)this.GetValue(SizeProperty); }
            set { this.SetValue(SizeProperty, value); }
        }

        public override void createAndDraw(double x, double y)
        {
            inputPort = new Port(this, canvas, PortType.INPUT, x - 4, y + 26.5);
            outputPort = new Port(this, canvas, PortType.OUTPUT, x + 86, y + 26.5);
            resourcePort = new Port(this, canvas, PortType.BOTTOM_RESOURCE, x + 42.5, y + 55.5);
            canvas.Children.Add(inputPort);
            canvas.Children.Add(outputPort);
            canvas.Children.Add(resourcePort);
            ports.Add(inputPort);
            ports.Add(outputPort);
            ports.Add(resourcePort);

            label = new Label(this, canvas, x + 18, y + 22, name.Value);
            canvas.Children.Add(label);
        }

        public override string iconPath()
        {
            return "Resources/Procedure.gif";
        }

        public override string description()
        {
            return "Блок \"Процедура\" описывает некоторую деятельность над задачей проектирования," + 
                " которая требует время и занимает подключенные ресурсы. Задаваемое время является" + 
                " стохастической величиной и зависит от параметра \"Разброс (%)\"";
        }

        public override List<UIParam> getParams()
        {
            List<UIParam> param = base.getParams();
            param.Add(time);
            param.Add(timeMeasure);
            param.Add(scatter);
            return param;
        }

        public double Time
        {
            get { return time.Value; }
            set { time.Value = value; }
        }

        public TimeMeasure TimeMeasure
        {
            get { return timeMeasure.Value; }
            set { timeMeasure.Value = value; }
        }

        public int Scatter
        {
            get { return scatter.Value; }
            set { scatter.Value = value; }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("inputPort", inputPort);
            info.AddValue("outputPort", outputPort);
            info.AddValue("resourcePort", resourcePort);
            info.AddValue("time", time);
            info.AddValue("timeMeasure", timeMeasure);
            info.AddValue("scatter", scatter);
        }

        protected override System.Windows.Media.Geometry DefiningGeometry
        {
            get 
            {
                GeometryGroup gg = new GeometryGroup();
                gg.FillRule = FillRule.EvenOdd;
                RectangleGeometry rg = new RectangleGeometry(bound, 10, 10);
                gg.Children.Add(rg);
                return gg;
            }
        }

        private void init()
        {
            Fill = Brushes.LemonChiffon;
            bound = new Rect(new Size(90, 60));
        }
    }
}
