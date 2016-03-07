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
    public class SubDiagram : UIEntity, ISerializable
    {

        private Rect bound;
        private Port inputPort, outputPort;

        private ProjectItem projectItem;

        public SubDiagram(Canvas canvas, ProjectItem item) : base(canvas)
        {
            init();
            this.projectItem = item;
            name.Value = item.Name;
        }

        public SubDiagram(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.inputPort = info.GetValue("inputPort", typeof(Port)) as Port;
            this.outputPort = info.GetValue("outputPort", typeof(Port)) as Port;
            this.projectItem = info.GetValue("projectItem", typeof(ProjectItem)) as ProjectItem;
            ports.Add(inputPort);
            ports.Add(outputPort);

            init();
        }

        public override void createAndDraw(double x, double y)
        {
            inputPort = new Port(this, canvas, PortType.INPUT, x - 4, y + 26.5);
            outputPort = new Port(this, canvas, PortType.OUTPUT, x + 86, y + 26.5);
            canvas.Children.Add(inputPort);
            canvas.Children.Add(outputPort);
            ports.Add(inputPort);
            ports.Add(outputPort);

            label = new Label(this, canvas, x + 22, y + 22, name.Value);
            canvas.Children.Add(label);
        }

        public override string iconPath()
        {
            return "Resources/Subprocess.gif";
        }

        public override string description()
        {
            return "Подпроцесс проектирования";
        }

        public ProjectItem ProjectItem
        {
            get { return projectItem; }
        }

        public override List<UIParam> getParams()
        {
            List<UIParam> param = base.getParams();
            return param;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("inputPort", inputPort);
            info.AddValue("outputPort", outputPort);
            info.AddValue("projectItem", projectItem);
        }

        protected override System.Windows.Media.Geometry DefiningGeometry
        {
            get 
            {
                GeometryGroup gg = new GeometryGroup();
                gg.FillRule = FillRule.EvenOdd;
                RectangleGeometry rg = new RectangleGeometry(bound, 5, 10);
                RectangleGeometry eg1 = new RectangleGeometry(new Rect(new Size(60, 3)), 35, 15) { Transform = new TranslateTransform(15, 15) };
                RectangleGeometry eg2 = new RectangleGeometry(new Rect(new Size(60, 3)), 35, 15) { Transform = new TranslateTransform(15, 45) };

                gg.Children.Add(rg);
                gg.Children.Add(eg1);
                gg.Children.Add(eg2);
                return gg;
            }
        }

        private void init()
        {
            Fill = Brushes.CornflowerBlue;
            bound = new Rect(new Size(90, 60));
        }
    }
}
