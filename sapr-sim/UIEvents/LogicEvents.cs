using Entities;
using EntityTransformator;
using Kernel;
using sapr_sim.Figures;
using sapr_sim.WPFCustomElements;
using sapr_sim.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Statistics;

namespace sapr_sim
{
    public partial class MainWindow : Window
    {
        private void SimulateButton_Click(object sender, RoutedEventArgs e)
        {
            if (Project.Instance.MainProjectItem != null)
            {
                ClosableTabItem cti = findTabItem(Project.Instance.MainProjectItem);
                if (cti == null)
                    createNewDiagram(Project.Instance.MainProjectItem.Canvas, Project.Instance.MainProjectItem.Name);
                else
                    cti.IsSelected = true;
                simulate(Project.Instance.MainProjectItem);
            }
            else
            {
                MessageBox.Show("Не указан главный процесс. Укажите главный процесс в Проект -> Настройки проекта");
            }
        }

        private void SimulateLocalButton_Click(object sender, RoutedEventArgs e)
        {
            simulate(Project.Instance.byCanvas(currentCanvas as ScrollableCanvas));
        }

        private void simulate(ProjectItem pi)
        {
            TransformerService ts = new TransformerService();
            List<Entity> entities = ts.transform(pi.Canvas.Children);
            Controller controller = new Controller(entities, ts.getResources());

            try
            {
                resetUIShadows(Project.Instance.MainProjectItem.Canvas.Children);
                SaveAll_Click(null, null);
                errorsListBox.Items.Clear();
                controller.validate();
                new RunSimulation(pi, controller).ShowDialog();
            }
            catch (ValidationException ex)
            {
                errorsTab.IsSelected = true;
                foreach (var err in ex.Errors)
                    errorsListBox.Items.Add(new ListBoxItemError(err.Message, ts.transform(err.Entities)));
            }
        }

        private void resetUIShadows(UIElementCollection col)
        {
            foreach (UIElement el in col)
            {
                (el as UIEntity).defaultBitmapEffect();
            }
        }
    }
}

