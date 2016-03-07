using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using sapr_sim.WPFCustomElements.UserControls;
using sapr_sim.Utils;

namespace sapr_sim.WPFCustomElements
{
    // http://www.codeproject.com/Articles/84213/How-to-add-a-Close-button-to-a-WPF-TabItem
    public class ClosableTabItem : TabItem
    {

        private ClosableHeader closableTabHeader;

        public ClosableTabItem()
        {
            closableTabHeader = new ClosableHeader();
            // Assign the usercontrol to the tab header
            this.Header = closableTabHeader;

            // Attach to the CloseableHeader events
            // (Mouse Enter/Leave, Button Click, and Label resize)
            closableTabHeader.button_close.MouseEnter +=
               new MouseEventHandler(button_close_MouseEnter);
            closableTabHeader.button_close.MouseLeave +=
               new MouseEventHandler(button_close_MouseLeave);
            closableTabHeader.button_close.Click +=
               new RoutedEventHandler(button_close_Click);
            closableTabHeader.label_TabTitle.SizeChanged +=
               new SizeChangedEventHandler(label_TabTitle_SizeChanged);
        }

        public string Title
        {
            get { return ((ClosableHeader)this.Header).label_TabTitle.Content.ToString(); }
            set { ((ClosableHeader)this.Header).label_TabTitle.Content = value; }
        }

        protected override void OnSelected(RoutedEventArgs e)
        {
            base.OnSelected(e);
            ((ClosableHeader)this.Header).button_close.Visibility = Visibility.Visible;
        }

        protected override void OnUnselected(RoutedEventArgs e)
        {
            base.OnUnselected(e);
            ((ClosableHeader)this.Header).button_close.Visibility = Visibility.Hidden;
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            ((ClosableHeader)this.Header).button_close.Visibility = Visibility.Visible;
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (!this.IsSelected)
            {
                ((ClosableHeader)this.Header).button_close.Visibility = Visibility.Hidden;
            }
        }

        // Button MouseEnter - When the mouse is over the button - change color to Red
        void button_close_MouseEnter(object sender, MouseEventArgs e)
        {
            ((ClosableHeader)this.Header).button_close.Foreground = Brushes.Red;
        }

        // Button MouseLeave - When mouse is no longer over button - change color back to black
        void button_close_MouseLeave(object sender, MouseEventArgs e)
        {
            ((ClosableHeader)this.Header).button_close.Foreground = Brushes.Black;
        }

        // Button Close Click - Remove the Tab - (or raise
        // an event indicating a "CloseTab" event has occurred)
        public void button_close_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow) System.Windows.Application.Current.MainWindow;
            if (main.IsModelChanged(this))
            {
                MessageBoxResult result = MessageBox.Show("Процесс изменился. Сохранить изменения перед закрытием?", 
                    "Предупреждение", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        ScrollableCanvas canvas = (Content as ScrollViewer).Content as ScrollableCanvas;
                        ProjectItem item = Project.Instance.byCanvas(canvas);
                        new FileService().save(canvas, item.FullPath);
                        ((TabControl)this.Parent).Items.Remove(this);
                        break;
                    case MessageBoxResult.No:                        
                        ((TabControl)this.Parent).Items.Remove(this);
                        break;
                    case MessageBoxResult.Cancel:                        
                        break;
                }
            }
            else
            {
                ((TabControl)this.Parent).Items.Remove(this);
            }
            
        }

        // Label SizeChanged - When the Size of the Label changes
        // (due to setting the Title) set position of button properly
        void label_TabTitle_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((ClosableHeader)this.Header).button_close.Margin = new Thickness(
               ((ClosableHeader)this.Header).label_TabTitle.ActualWidth + 5, 3, 4, 0);
        }
    }
}
