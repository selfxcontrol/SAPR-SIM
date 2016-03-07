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
    public partial class CreateDiagramDialog : Window
    {
        public CreateDiagramDialog()
        {
            InitializeComponent();
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(name.Text))
            {
                MessageBox.Show("Введите имя");
                return;
            }
            if (Project.Instance.Items.Find(x => x.Name == name.Text) != null)
            {
                MessageBox.Show("Такое имя уже задано");
                return;
            }
            this.DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            name.SelectAll();
            name.Focus();
        }

        public string Name
        {
            get { return name.Text; }
        }
    }
}
