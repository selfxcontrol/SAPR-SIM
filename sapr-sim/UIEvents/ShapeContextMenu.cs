using sapr_sim.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace sapr_sim
{
    public partial class MainWindow : Window
    {

        private void attachContextMenu(UIEntity ent)
        {
            if (ent.ContextMenu == null && !(ent is Port || ent is ConnectionLine))
            {
                ContextMenu menu = new ContextMenu();

                if (ent is SubDiagram)
                {
                    MenuItem open = new MenuItem()
                    {
                        Header = "Открыть",
                        InputGestureText = "Ctrl + V"
                    };
                    open.Click += GotoSubprocess_Click;
                    menu.Items.Add(open);
                    menu.Items.Add(new Separator());
                }

                MenuItem copy = new MenuItem()
                {
                    Header = "Копировать",
                    InputGestureText = "Ctrl + C"
                };
                copy.Click += Copy_Click;
                menu.Items.Add(copy);

                MenuItem cut = new MenuItem()
                {
                    Header = "Вырезать",
                    InputGestureText = "Ctrl + X"
                };
                cut.Click += Cut_Click;
                menu.Items.Add(cut);

                MenuItem delete = new MenuItem()
                {
                    Header = "Удалить",
                    InputGestureText = "Del"
                };
                delete.Click += Delete_Click;
                menu.Items.Add(delete);       

                if (!(ent is SubDiagram) && ent.ParametersExist)
                {
                    menu.Items.Add(new Separator());

                    MenuItem properties = new MenuItem()
                    {
                        Header = "Свойства"
                    };
                    properties.Click += OpenShapeProperties_Click;
                    menu.Items.Add(properties);
                }

                ent.ContextMenu = menu;
            }
        }

        private void GotoSubprocess_Click(object sender, RoutedEventArgs e)
        {
            if (selected != null)
            {
                SubDiagram sd = selected as SubDiagram;
                findAndOpenRelatedTab(sd.ProjectItem);
            }
        }

        private void OpenShapeProperties_Click(object sender, RoutedEventArgs e)
        {
            if (selected != null)
            {
                UIEntity ent = selected as UIEntity;
                if (ent.ParametersExist)
                    new ParameterDialog(ent.getParams(), ent).ShowDialog();
            }
        }

    }
}
