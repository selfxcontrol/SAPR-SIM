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

        private void attachContextMenu(Canvas canvas)
        {
            if (canvas.ContextMenu == null)
            {
                ContextMenu menu = new ContextMenu();

                MenuItem runSimulation = new MenuItem()
                {
                    Header = "Запустить моделирование"
                };
                runSimulation.Click += SimulateLocalButton_Click;
                menu.Items.Add(runSimulation);

                MenuItem paste = new MenuItem()
                {
                    Header = "Вставить",
                    InputGestureText = "Ctrl + V"
                };
                paste.Click += Paste_Click;
                menu.Items.Add(paste);

                menu.Items.Add(new MenuItem()
                {
                    Header = "Выделить все",
                    IsEnabled = false
                });

                canvas.ContextMenu = menu;
            }
        }

    }
}
