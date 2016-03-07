using sapr_sim.Parameters;
using sapr_sim.Parameters.Validators;
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
    public class Source : UIEntity, ISerializable
    {
        protected Port port;
        private Rect bound;

        private UIParam<Int32> projectsCount = new UIParam<Int32>(1, new PositiveIntegerParamValidator(), "Количество задач",
            "Количество задач процесса проектирования, участвующих в имитационном моделировании. Может принимать положительное целочисленное значение");
        private UIParam<Int32> complexity = new UIParam<Int32>(1, new PositiveIntegerParamValidator(), "Сложность проектов",
            "Качественная оценка задачи процесса проектирования. Может принимать положительное целочисленное значение");

        private static readonly string DEFAULT_NAME = "Начало";

        public Source(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.port = info.GetValue("port", typeof(Port)) as Port;
            ports.Add(port);

            projectsCount = info.GetValue("projectsCount", typeof(UIParam<Int32>)) as UIParam<Int32>;
            complexity = info.GetValue("complexity", typeof(UIParam<Int32>)) as UIParam<Int32>;

            init();
        }

        public Source(Canvas canvas) : base(canvas)
        {
            init();
            name.Value = DEFAULT_NAME;
        }

        public override void createAndDraw(double x, double y)
        {
            port = new Port(this, canvas, PortType.OUTPUT, x + 26, y + 12);
            canvas.Children.Add(port);
            ports.Add(port);

            label = new Label(this, canvas, x + 12, y + 23, name.Value);
        }

        public override string iconPath()
        {
            return "Resources/Sink.gif";
        }

        public override string description()
        {
            return "Блок \"СТАРТ\". Является входной точкой процесса проектирования.";
        }

        public override List<UIParam> getParams()
        {
            return new List<UIParam>() { projectsCount, complexity };
        }

        public int ProjectsCount
        {
            get { return projectsCount.Value; }
            set { projectsCount.Value = value; }
        }

        public int Complexity
        {
            get { return complexity.Value; }
            set { complexity.Value = value; }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("port", port);
            info.AddValue("projectsCount", projectsCount);
            info.AddValue("complexity", complexity);
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                GeometryGroup gg = new GeometryGroup();
                gg.FillRule = FillRule.EvenOdd;
                EllipseGeometry eg = new EllipseGeometry(bound);
                gg.Children.Add(eg);
                return gg;
            }
        }

        private void init()
        {
            Fill = Brushes.LightGreen;
            bound = new Rect(new Size(30, 30));
        }
    }
}
