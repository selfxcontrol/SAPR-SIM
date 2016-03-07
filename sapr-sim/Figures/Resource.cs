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
    public abstract class Resource : UIEntity, ISerializable
    {


        private static readonly string DEFAULT_NAME = "Ресурс";

        protected UIParam<ResourceType> type = new UIParam<ResourceType>(ResourceType.WORKER, new DefaultParamValidator(), "Тип ресурса",
            "Тип ресурса", new ParameterComboBox(ResourceType.list()));
        protected UIParam<int> count = new UIParam<int>(1, new PositiveIntegerParamValidator(), "Количество",
            "Количество экземпляров данного ресурса. Может принимать положительное целочисленное значение");
        protected UIParam<Boolean> isShared = new UIParam<Boolean>(true, new DefaultParamValidator(), "Разделяемый",
            "Позволяет ресурсу заниматься произвольным числом действий", new ParameterCheckBox(true));    

        private Rect bound;
        private Rect topExternalBound;
        private Rect bottomExternalBound;

        private Port topPort;    

        public Resource(Canvas canvas) : base(canvas)
        {
            init();
            name.Value = DEFAULT_NAME;
        }

        public Resource(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.topPort = info.GetValue("topPort", typeof(Port)) as Port;
            ports.Add(topPort);

            type = info.GetValue("type", typeof(UIParam<ResourceType>)) as UIParam<ResourceType>;
            type.ContentControl = new ParameterComboBox(ResourceType.list()) { SelectedIndex = type.Value.Order };

            isShared = info.GetValue("isShared", typeof(UIParam<Boolean>)) as UIParam<bool>;
            isShared.ContentControl = new ParameterCheckBox(isShared.Value);
            count = info.GetValue("count", typeof(UIParam<int>)) as UIParam<int>;

            init();
        }

        public override void createAndDraw(double x, double y)
        {
            topPort = new Port(this, canvas, PortType.TOP_RESOURCE, x + 42.5, y - 3.5);
            canvas.Children.Add(topPort);
            ports.Add(topPort);
        }

        public override string iconPath()
        {
            return "Resources/Resource.gif";
        }

        public override string description()
        {
            return "Блок \"Ресурс\" состоит из списка атомарных ресурсов, используемых при выполнении" + 
                " действий над задачей проектирования. Все атомарные ресурсы образуют составной ресурс," + 
                " т.е. могут быть заняты или освобождены только одновременно. Для неразделяемого ресурса" + 
                " параметр \"Количество\" отвечает за количество экземпляров данного ресурса. Разделяемый" + 
                " ресурс может быть занят произвольным числом действий.";
        }

        public override List<UIParam> getParams()
        {
            List<UIParam> param = base.getParams();
            param.Add(type);
            param.Add(isShared);
            param.Add(count);
            return param;
        }        

        public Boolean IsShared
        {
            get { return isShared.Value; }
            set { isShared.Value = value; }
        }

        public int Count
        {
            get { return count.Value; }
            set { count.Value = value; }
        }

        public ResourceType Type
        {
            get { return type.Value; }
            set { type.Value = value; }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("topPort", topPort);
            info.AddValue("isShared", isShared);
            info.AddValue("count", count);
            info.AddValue("type", type);
        }

        protected override System.Windows.Media.Geometry DefiningGeometry
        {
            get 
            {
                GeometryGroup gg = new GeometryGroup();
                gg.FillRule = FillRule.Nonzero;
                gg.Children.Add(new RectangleGeometry(bound));
                gg.Children.Add(new RectangleGeometry(topExternalBound));
                gg.Children.Add(new RectangleGeometry(bottomExternalBound));
                return gg;
            }
        }

        protected virtual void init()
        {
            bound = new Rect(new Size(90, 60));
            topExternalBound = new Rect(new Point(0, 0), new Point(90, 10));
            bottomExternalBound = new Rect(new Point(0, 50), new Point(90, 60));
        }
    }
}
