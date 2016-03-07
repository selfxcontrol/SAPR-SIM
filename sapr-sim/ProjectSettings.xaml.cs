using sapr_sim.Parameters.Validators;
using sapr_sim.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace sapr_sim
{

    public partial class ProjectSettings : Window
    {
        public ProjectSettings()
        {
            InitializeComponent();
            
            Project prj = Project.Instance;
            projectName.Text = prj.ProjectName;
            projectDirectory.Text = prj.ProjectPath;

            //************************************************************************
            foreach (ProjectItem pi in prj.Items)
                mainProjectItem.Items.Add(pi.Name);

            if (prj.MainProjectItem != null)
                mainProjectItem.SelectedItem = prj.MainProjectItem.Name;
            else
                mainProjectItem.SelectedIndex = 0;

            //************************************************************************
            projectTimeRestriction.Text = prj.TimeRestiction.Time.ToString();
            foreach (TimeMeasure tm in TimeMeasure.list())
                projectTimeRestrictionMeasure.Items.Add(tm.ToString());

            projectTimeRestrictionMeasure.SelectedIndex = prj.TimeRestiction.Measure.Order;

            //************************************************************************
            saveResult.IsChecked = prj.SaveResult;
            saveResultDirectory.Text = prj.ResultPath;
            saveResultDirectory.IsEnabled = prj.SaveResult;
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            Project prj = Project.Instance;
            FileService fs = new FileService();

            int newPrjTime = 0;
            string resultPath = prj.ResultPath;

            if (new PositiveIntegerParamValidator().validate(projectTimeRestriction.Text))            
                newPrjTime = Int32.Parse(projectTimeRestriction.Text);
            else
            {
                MessageBox.Show("Параметр 'Время ограничения моделирования' указан неверно");
                return;
            }

            if (saveResult.IsChecked.Value)
            {
                if (FileService.IsValidPath(saveResultDirectory.Text))
                {
                    if (Directory.Exists(saveResultDirectory.Text))
                        resultPath = saveResultDirectory.Text;
                    else
                    {
                        MessageBoxResult result = MessageBox.Show("Указанная директория 'Сохранять результаты в' не существует. Создать?",
                        "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        switch (result)
                        {
                            case MessageBoxResult.Yes:
                                Directory.CreateDirectory(saveResultDirectory.Text);
                                resultPath = saveResultDirectory.Text;
                                break;
                            case MessageBoxResult.No:
                                return;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Параметр 'Сохранять результаты в' указан неверно");
                    return;
                }
            }

            if (projectName.Text != prj.ProjectName)
            {
                fs.renameProject(projectName.Text);
                prj.ProjectName = projectName.Text;
            }

            if (mainProjectItem.SelectedItem != null)
                prj.MainProjectItem = prj.Items.First(x => x.Name == mainProjectItem.SelectedItem.ToString());
            
            prj.TimeRestiction.Time = newPrjTime;
            prj.TimeRestiction.Measure = TimeMeasure.byOrder(projectTimeRestrictionMeasure.SelectedIndex);
            prj.SaveResult = saveResult.IsChecked.Value;
            prj.ResultPath = resultPath;
                        
            fs.saveProject();
            DialogResult = true;
        }

        private void saveResultDirectoryChanged(object sender, RoutedEventArgs e)
        {
            saveResultDirectory.IsEnabled = (sender as CheckBox).IsChecked.Value;
        }
    }
}
