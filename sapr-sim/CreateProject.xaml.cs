using sapr_sim.Utils;
using System;
using System.Collections.Generic;
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
    public partial class CreateProject : Window
    {

        public CreateProject()
        {
            InitializeComponent();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
                directory.Text = dialog.SelectedPath;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(name.Text))
            {
                MessageBox.Show("Параметр 'Имя проекта' заполнен неверно", "Ошибка");
                return;
            }
            if (FileService.IsValidPath(directory.Text))
            {
                Project project = Project.Instance;
                project.ProjectName = name.Text;
                project.ProjectPath = directory.Text;

                if (createNewModel.IsChecked.Value)
                {
                    if (String.IsNullOrWhiteSpace(modelName.Text))
                    {
                        MessageBox.Show("Заполните все параметры", "Ошибка");
                        return;
                    }
                    project.addProjectItem(new ProjectItem(modelName.Text));
                }

                DialogResult = true;
                this.Close();
            }
            else
                MessageBox.Show("Параметр 'Директория проекта' заполнен неверно", "Ошибка");
        }

        private void createNewProcessChanged(object sender, RoutedEventArgs e)
        {
            if (modelName != null)
                modelName.IsEnabled = (sender as CheckBox).IsChecked.Value;
        }
    }
}
