using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Runtime.Serialization;
using sapr_sim.Figures;
using sapr_sim.Utils;

namespace sapr_sim.WPFCustomElements
{
    [Serializable]
   public class ScrollableCanvas : Canvas, ISerializable, IEqualityComparer<ScrollableCanvas>  
   {
        static ScrollableCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ScrollableCanvas), new FrameworkPropertyMetadata(typeof(ScrollableCanvas)));
        }

        public long id {get; set;} 

        public ScrollableCanvas(SerializationInfo info, StreamingContext context)
        {
            for (int i = 0; i < info.MemberCount - 1; i++)
            {
                UIEntity ent = info.GetValue("Child" + i, typeof(UIEntity)) as UIEntity;
                try
                {

                    // hack for copy paste serialization
                    if (ent.EntityName != null)
                    {
                        Children.Add(ent);
                        ZIndexUtil.setCorrectZIndex(this, ent);
                    }
                }
                catch (Exception ex)
                {
                    Console.Out.Write(ex);
                }
            }
            this.id = info.GetInt32("id");
        }

        public ScrollableCanvas()
        {
            id = IdGenerator.GenerateId();
        }
        //Функция для  определения размера рабочей области исходя из размера графа
        protected override Size MeasureOverride(Size constraint)
        {
            double bottomMost = 0d;
            double rightMost = 0d;

            foreach(object obj in Children)
            {
                FrameworkElement child = obj as FrameworkElement;

                if(child != null)
                {
                    child.Measure(constraint);
                    bottomMost = Math.Max(bottomMost, VisualTreeHelper.GetOffset(child).Y + child.DesiredSize.Height);
                    rightMost = Math.Max(rightMost, VisualTreeHelper.GetOffset(child).X + child.DesiredSize.Width);
                }
            }
            return new Size(rightMost, bottomMost);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                UIEntity ent = Children[i] as UIEntity;
                info.AddValue("Child" + i, ent);
            }
            info.AddValue("id", id);
        }

        public bool Equals(ScrollableCanvas x, ScrollableCanvas y)
        {
            if (x == null || y == null) return false;
            return x.id == y.id;
        }

        public int GetHashCode(ScrollableCanvas obj)
        {
            return id.GetHashCode();
        }
    }
}
