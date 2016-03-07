using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace sapr_sim.WPFCustomElements
{
    public class ProjectTreeViewItem : TreeViewItem
    {

        public ProjectItem ProjectItem { get; set; }

        public static StackPanel packProjectItem(string name, bool editMode)
        {
            return pack(name, editMode, "pack://application:,,/Resources/diagram.png");
        }

        public static StackPanel packProject(string name, bool editMode)
        {
            return pack(name, editMode, "pack://application:,,/Resources/project.png");
        }

        public static string unpack(StackPanel sp)
        {
            return sp.Children[1].ToString();
        }

        private static StackPanel pack(string name, bool editMode, string iconPath)
        {
            StackPanel sp = new StackPanel() { Orientation = Orientation.Horizontal };
            sp.Children.Add(new Image() { Source = new BitmapImage(new Uri(iconPath)) });
            if (editMode)
                sp.Children.Add(new ProjectItemTextBox() { Text = name });
            else
                sp.Children.Add(new ProjectItemLabel() { Content = name });
            return sp;
        }

    }

}
