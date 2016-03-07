using sapr_sim.Figures;
using sapr_sim.Utils;
using sapr_sim.WPFCustomElements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace sapr_sim
{
    public partial class MainWindow : Window
    {

        private void attachProjectEvents(TreeViewItem root)
        {
            root.PreviewMouseRightButtonDown += OnPreviewMouseRightButtonDown;

            ContextMenu menu = new ContextMenu();

            MenuItem addNewDgm = new MenuItem()
            {
                Header = "Добавить новый процесс",
                Icon = new Image()
                {
                    Source = new BitmapImage(new Uri("pack://application:,,/Resources/add_diagram.png"))
                }
            };
            addNewDgm.Click += AddNewDiagram_RootMenuClick;
            menu.Items.Add(addNewDgm);

            MenuItem openDgm = new MenuItem()
            {
                Header = "Открыть процесс",
                Icon = new Image()
                {
                    Source = new BitmapImage(new Uri("pack://application:,,/Resources/folder.png"))
                }
            };
            openDgm.Click += OpenDiagram_RootMenuClick;
            menu.Items.Add(openDgm);

            MenuItem runSimulationItem = new MenuItem() 
            { 
                Header = "Запустить моделирование", 
                Icon = new Image()
                {
                    Source = new BitmapImage(new Uri("pack://application:,,/Resources/play-circle.png"))
                }
            };
            runSimulationItem.Click += SimulateButton_Click;
            menu.Items.Add(runSimulationItem);

            MenuItem renameRoot = new MenuItem()
            {
                Header = "Переименовать",
                Icon = new Image()
                {
                    Source = new BitmapImage(new Uri("pack://application:,,/Resources/rename.png"))
                }
            };
            renameRoot.Click += Rename_RootMenuClick;
            menu.Items.Add(renameRoot);

            MenuItem settings = new MenuItem()
            {
                Header = "Настройки",
                Icon = new Image()
                {
                    Source = new BitmapImage(new Uri("pack://application:,,/Resources/settings.png"))
                }
            };
            settings.Click += ProjectSettings_Click;
            menu.Items.Add(settings);
            
            root.ContextMenu = menu;
        }

        private void attachProjectItemEvents(TreeViewItem item)
        {
            item.MouseDoubleClick += OnProjectItemMouseDoubleClick;
            item.PreviewMouseRightButtonDown += OnPreviewMouseRightButtonDown;

            ContextMenu menu = new ContextMenu();

            MenuItem addToDiagram = new MenuItem() 
            { 
                Header = "Добавить как подпроцесс",
                Icon = new Image()
                {
                    Source = new BitmapImage(new Uri("pack://application:,,/Resources/add_diagram.png"))
                }
            };
            addToDiagram.Click += AddToDiagram_MenuClick;
            menu.Items.Add(addToDiagram);

            MenuItem runSimulationItem = new MenuItem() { Header = "Запустить моделирование", Icon = new Image() 
            {
                Source = new BitmapImage(new Uri("pack://application:,,/Resources/play-circle.png"))
            } 
            };
            runSimulationItem.Click += RunSimulation_MenuClick;
            menu.Items.Add(runSimulationItem);

            MenuItem removeItem = new MenuItem() 
            { 
                Header = "Убрать из проекта", 
                Icon = new Image() 
                {
                    Source = new BitmapImage(new Uri("pack://application:,,/Resources/delete.png"))
                } 
            };
            removeItem.Click += RemoveItem_MenuClick;
            menu.Items.Add(removeItem);

            MenuItem renameItem = new MenuItem()
            {
                Header = "Переименовать",
                Icon = new Image()
                {
                    Source = new BitmapImage(new Uri("pack://application:,,/Resources/rename.png"))
                }
            };
            renameItem.Click += Rename_RootMenuClick;
            menu.Items.Add(renameItem);

            item.ContextMenu = menu;
        }

        private void OnProjectItemMouseDoubleClick(object sender, MouseButtonEventArgs args)
        {
            ProjectTreeViewItem tvi = sender as ProjectTreeViewItem;
            findAndOpenRelatedTab(tvi.ProjectItem);
        }

        private void AddNewDiagram_RootMenuClick(object sender, RoutedEventArgs e)
        {
            CreateNewDiagram_Click(sender, e);
        }

        private void OpenDiagram_RootMenuClick(object sender, RoutedEventArgs e)
        {
            Open_Click(null, null);
        }

        private void AddToDiagram_MenuClick(object sender, RoutedEventArgs e)
        {
            ProjectTreeViewItem item = projectStructure.SelectedItem as ProjectTreeViewItem;
            currentEntity = new SubDiagram(null, item.ProjectItem);
            currentCanvas.Cursor = Cursors.Pen;
        }

        private void RunSimulation_MenuClick(object sender, RoutedEventArgs e)
        {
            ProjectTreeViewItem item = projectStructure.SelectedItem as ProjectTreeViewItem;
            ClosableTabItem cti = findTabItem(item.ProjectItem);
            if (cti == null)
                createNewDiagram(item.ProjectItem.Canvas, item.ProjectItem.Name);
            else
                cti.IsSelected = true;
            SimulateLocalButton_Click(null, null);
        }

        private void RemoveItem_MenuClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены?",
                    "Убрать файл из проекта", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                ProjectTreeViewItem item = projectStructure.SelectedItem as ProjectTreeViewItem;

                ClosableTabItem cti = findTabItem(item.ProjectItem);
                if (cti != null) cti.button_close_Click(null, null);

                (projectStructure.Items[0] as TreeViewItem).Items.Remove(item);
                Project.Instance.removeProjectItem(item.ProjectItem);
                fs.saveProject();
            }          
        }
        
        private void Rename_RootMenuClick(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = projectStructure.SelectedItem as TreeViewItem;
            string name = ProjectTreeViewItem.unpack(item.Header as StackPanel);

            StackPanel sp = null;
            if (item is ProjectTreeViewItem)
                sp = ProjectTreeViewItem.packProjectItem(name, true);
            else
                sp = ProjectTreeViewItem.packProject(name, true);

            TextBox tb = sp.Children[1] as TextBox;
            tb.KeyDown += TextBox_KeyUp;
            tb.LostFocus += TextBox_LostFocus;

            tb.IsVisibleChanged += new DependencyPropertyChangedEventHandler(TextBox_IsVisibleChanged);

            tb.CaretIndex = tb.Text.Length;
            item.Header = sp;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = projectStructure.SelectedItem as TreeViewItem;
            if (item != null && item.Header is StackPanel)
            {
                StackPanel sp = item.Header as StackPanel;
                if (sp.Children.Count == 2 && sp.Children[1] is TextBox)
                    renameTreeViewItem(sender);
            }
        }

        private void TextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;
            e.Handled = true;
            renameTreeViewItem(sender);
        }

        private void OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);
            e.Handled = true;
            if (treeViewItem != null)
            {
                treeViewItem.Focus();
                e.Handled = true;
            }
        }

        private TreeViewItem VisualUpwardSearch(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem))
                source = VisualTreeHelper.GetParent(source);

            return source as TreeViewItem;
        }

        private void renameTreeViewItem(object sender)
        {
            string newName = (sender as TextBox).Text;
            if (String.IsNullOrWhiteSpace(newName))
            {
                MessageBox.Show("Введено некорректное имя");
                return;
            }
            TreeViewItem selected = projectStructure.SelectedItem as TreeViewItem;
            
            ProjectItem pi = Project.Instance.Items.Find(x => x.Name == newName);
            if (pi != null)
            {
                // true = avoiding NOT Renaming (when user clicked "Rename" and name not changed)
                if (selected is ProjectTreeViewItem && (selected as ProjectTreeViewItem).ProjectItem.Equals(pi))
                    selected.Header = ProjectTreeViewItem.packProjectItem(newName, false);
                else
                    MessageBox.Show("Процесс с таким именем уже подключен");
                return;
            }


            if (File.Exists(Project.Instance.FullPath + "\\" + newName + FileService.PROJECT_ITEM_EXTENSION))
            {
                MessageBoxResult result = MessageBox.Show("Файл с таким именем уже есть в проектной директории. Перезаписать файл?",
                    "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.No) return;
            }

            if (selected is ProjectTreeViewItem)
            {
                ProjectTreeViewItem item = selected as ProjectTreeViewItem;
                item.Header = ProjectTreeViewItem.packProjectItem(newName, false);
                fs.renameProjectItem(item.ProjectItem, newName);
                changeTabName(findTabItem(item.ProjectItem), newName);
            }
            else 
            {
                TreeViewItem root = projectStructure.Items[0] as TreeViewItem;
                root.Header = ProjectTreeViewItem.packProject(newName, false);
                fs.renameProject(newName);
            }            
        }

        // fucking magic for set focus...
        // http://www.codeproject.com/Tips/478376/Setting-focus-to-a-control-inside-a-usercontrol-in
        private void TextBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(
                    delegate()
                    {
                        (sender as TextBox).Focus();
                    }
                ));
            }
        }  
    }
}
