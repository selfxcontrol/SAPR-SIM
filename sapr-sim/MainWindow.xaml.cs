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
using System.Windows.Navigation;
using System.Windows.Shapes;

using sapr_sim.Figures;
using sapr_sim.WPFCustomElements;
using System.Windows.Media.Effects;
using sapr_sim.Parameters;
using sapr_sim.Utils;

namespace sapr_sim
{
    public partial class MainWindow : Window
    {

        private double leftColumnWidth;
        private double leftColumnMinWidth;
        private bool leftColumnHidden;
        private double bottomColumnHeight;
        private double bottomColumnMinHeight;
        private bool bottomColumnHidden;

        public UIEntity currentEntity { get; set; }
        private Canvas currentCanvas;
       
        private bool firstConnect;

        // 300 - width of left panel (it's a constant in xaml)
        // 100 - random +- value :)
        public static readonly int X_OFFSET = -300;
        public static readonly int Y_OFFSET = -100;

        private readonly string defaultTabName = "Новый процесс ";
        private readonly string modelChangedPostfix = "* "; 

        public MainWindow()
        {
            InitializeComponent();
            bindHotkeys();
            ButtonsActivation(false);
            DataContext = new ViewModel();
            this.FontFamily = new FontFamily("Segoe UI");
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (leftColumnHidden)
            {
                // Restore the widths.
                LeftColumn.MinWidth = leftColumnMinWidth;
                LeftColumn.Width = new GridLength(leftColumnWidth);
                LeftSplitterColumn.Width = new GridLength(0);
            }
            else
            {
                // Remember the user-set widths for the columns.
                leftColumnWidth = LeftColumn.Width.Value;
                leftColumnMinWidth = LeftColumn.MinWidth;

                // Remember to set the minimum width to 0 before changing the actual
                // width.
                LeftColumn.MinWidth = 0;
                LeftColumn.Width = new GridLength(0);
                LeftSplitterColumn.Width = new GridLength(0);
            }

            leftColumnHidden = !leftColumnHidden;
        }

        private void BottomToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (bottomColumnHidden)
            {
                // Restore the widths.
                BottomColumn.MinHeight = bottomColumnMinHeight;
                BottomColumn.Height = new GridLength(bottomColumnHeight);
                BottomSplitterColumn.Height = new GridLength(0);
            }
            else
            {
                // Remember the user-set widths for the columns.
                bottomColumnHeight = BottomColumn.Height.Value;
                bottomColumnMinHeight = BottomColumn.MinHeight;

                // Remember to set the minimum width to 0 before changing the actual
                // width.
                BottomColumn.MinHeight = 0;
                BottomColumn.Height = new GridLength(0);
                BottomSplitterColumn.Height = new GridLength(0);
            }

            bottomColumnHidden = !bottomColumnHidden;
        }

        private void Application_ClosingEvent(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (needSave())
            {
                MessageBoxResult result = MessageBox.Show("Имеются не сохраненные данные. Сохранить изменения перед закрытием?",
                        "Предупреждение", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        SaveAll_Click(null, null);
                        break;
                    case MessageBoxResult.No:
                        break;
                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }

        private void OnMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (currentEntity != null && e.LeftButton == MouseButtonState.Pressed)
            {
                if (currentEntity is ConnectionLine && selected != null && selected is Port)
                {
                    connect();
                }
                else if (currentEntity is SubDiagram)
                {
                    SubDiagram sd = currentEntity as SubDiagram;
                    ScrollableCanvas sc = currentCanvas as ScrollableCanvas;
                    if (sc.Equals(sc, sd.ProjectItem.Canvas as ScrollableCanvas))
                    {
                        currentEntity = null;
                        MessageBox.Show("Нельзя добавить процесс к самому себе");
                    }
                    else
                    {
                        UndoRedoManager.putInUndoStack((ScrollableCanvas)currentCanvas);
                        UndoRedoManager.clearRedoStack((ScrollableCanvas)currentCanvas);
                        sd.canvas = currentCanvas;
                        drawOnCanvas(e.GetPosition(this));
                    }
                }
                else if (!(currentEntity is ConnectionLine))
                {
                    UndoRedoManager.putInUndoStack((ScrollableCanvas)currentCanvas);
                    UndoRedoManager.clearRedoStack((ScrollableCanvas)currentCanvas);
                    drawOnCanvas(e.GetPosition(this));
                }                
            }
        }

        private void createNewDiagram(Canvas canvas, string tabName)
        {
            if (canvas == null) canvas = new ScrollableCanvas();

            ClosableTabItem theTabItem = new ClosableTabItem();
            ScrollViewer scrollViewer = new ScrollViewer();

            canvas.Background = Brushes.Transparent;
            canvas.MouseDown += OnMouseDown;
            canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas.MouseRightButtonDown += Canvas_MouseRightButtonDown;

            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            scrollViewer.Content = canvas;

            theTabItem.IsSelected = true;
            theTabItem.Content = scrollViewer;
            theTabItem.Title = tabName;
            tabs.Items.Add(theTabItem);
            tabs.SelectionChanged += TabControl_SelectionChanged;

            attachContextMenu(canvas);

            currentCanvas = canvas;
        }

        private void createNewDiagram(Canvas canvas)
        {
            createNewDiagram(canvas, defaultTabName + (tabs.Items.Count + 1));
        }

        private bool canConnect()
        {
            BindingExpression srcExp = currentEntity.GetBindingExpression(ConnectionLine.SourceProperty);
            BindingExpression dstExp = currentEntity.GetBindingExpression(ConnectionLine.DestinationProperty);
            if (dstExp == null)
            {
                //return !(selected as Port).Owner.Equals((srcExp.DataItem as Port).Owner);
                return !selected.Equals((selected as Port).Owner, (srcExp.DataItem as Port).Owner);
            }
            return !selected.Equals(selected, srcExp.DataItem as UIEntity) && !selected.Equals(selected, dstExp.DataItem as UIEntity);
            //return !selected.Equals(srcExp.DataItem) && !selected.Equals(dstExp.DataItem);
        }

        private void connect()
        {

            UndoRedoManager.putInUndoStack(currentEntity);
            UndoRedoManager.clearRedoStack(currentEntity);


            if (firstConnect)
            {
                setBinding(ConnectionLine.SourceProperty);
                firstConnect = false;
            }
            else if (canConnect())
            {
                setBinding(ConnectionLine.DestinationProperty);

                currentEntity.MouseLeftButtonDown += Shape_MouseLeftButtonDown;
                currentEntity.MouseLeftButtonUp += Shape_MouseLeftButtonUp;

                ZIndexUtil.setCorrectZIndex(currentCanvas, currentEntity);

                currentCanvas.Children.Add(currentEntity);
                currentEntity = Activator.CreateInstance(currentEntity.GetType(), currentCanvas) as UIEntity;
                firstConnect = true;
                ModelChanged();
            }
        }

        private void setBinding(DependencyProperty dp)
        {
            currentEntity.SetBinding(dp, new Binding()
            {
                Source = selected,
                Path = new PropertyPath(Port.AnchorPointProperty)
            });
        }

        public void drawOnCanvas(Point position)
        {
            attachMovingEvents(currentEntity);

            Canvas.SetLeft(currentEntity, position.X + X_OFFSET);
            Canvas.SetTop(currentEntity, position.Y + Y_OFFSET);

            currentCanvas.Children.Add(currentEntity);
            currentEntity.createAndDraw(position.X + X_OFFSET, position.Y + Y_OFFSET);

            ZIndexUtil.setCorrectZIndex(currentCanvas, currentEntity);

            foreach (Port p in currentEntity.getPorts())
            {
                p.MouseLeftButtonDown += Shape_MouseLeftButtonDown;
                p.MouseLeftButtonUp += Shape_MouseLeftButtonUp;
            }

            if (!(currentEntity is SubDiagram))
                currentEntity = Activator.CreateInstance(currentEntity.GetType(), currentCanvas) as UIEntity;
            else
                currentEntity = Activator.CreateInstance(currentEntity.GetType(), currentCanvas, 
                    (currentEntity as SubDiagram).ProjectItem) as UIEntity;
            
            ModelChanged();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl && tabs.SelectedIndex >= 0)
            {
                currentCanvas = ((tabs.Items[tabs.SelectedIndex] as ClosableTabItem).Content as ScrollViewer).Content as ScrollableCanvas;
            }
        }

        private void ArrowButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = null;
            currentCanvas.Cursor = Cursors.Arrow;
        }
        
        private void LabelButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = new sapr_sim.Figures.Label(currentCanvas);
            currentCanvas.Cursor = Cursors.Pen;
        }

        private void ProcedureButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = new Procedure(currentCanvas);
            currentCanvas.Cursor = Cursors.Pen;
        }

        private void ResourceButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = new WorkerResource(currentCanvas);
            currentCanvas.Cursor = Cursors.Pen;
        }

        private void SyncButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = new Sync(currentCanvas);
            currentCanvas.Cursor = Cursors.Pen;
        }

        private void ParallelButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = new sapr_sim.Figures.Parallel(currentCanvas);
            currentCanvas.Cursor = Cursors.Pen;
        }

        private void DecisionButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = new Decision(currentCanvas);
            currentCanvas.Cursor = Cursors.Pen;
        }

        private void CollectorButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = new Collector(currentCanvas);
            currentCanvas.Cursor = Cursors.Pen;
        }

        private void EntitySourceButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = new Source(currentCanvas);
            currentCanvas.Cursor = Cursors.Pen;
        }

        private void EntityDestinationButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = new Destination(currentCanvas);
            currentCanvas.Cursor = Cursors.Pen;
        }

        private void LineButton_Click(object sender, RoutedEventArgs e)
        {
            currentEntity = new ConnectionLine(currentCanvas);
            firstConnect = true;
            currentCanvas.Cursor = Cursors.Pen;
        }

        public void attachMovingEvents(UIEntity entity)
        {
            // Garantee that we have only one event http://stackoverflow.com/questions/136975/has-an-event-handler-already-been-added
            entity.MouseLeftButtonDown -= Shape_MouseLeftButtonDown;
            entity.MouseLeftButtonDown += Shape_MouseLeftButtonDown;
            entity.MouseMove -= Shape_MouseMove;
            entity.MouseMove += Shape_MouseMove;
            entity.MouseLeftButtonUp -= Shape_MouseLeftButtonUp;
            entity.MouseLeftButtonUp += Shape_MouseLeftButtonUp;
            entity.MouseRightButtonDown -= Shape_MouseRightButtonDown;
            entity.MouseRightButtonDown += Shape_MouseRightButtonDown;

            attachContextMenu(entity);
        }        

        private void printInformation(string info)
        {
            infoTextBlock.Text += info + Environment.NewLine;
        }

        private void changeTabName(ClosableTabItem ti, string newName)
        {
            if (ti != null)           
                ti.Title = newName;
        }

        private void changeTabName(string newName)
        {
            changeTabName(tabs.SelectedItem as ClosableTabItem, newName);
        }

        private void findAndOpenRelatedTab(ProjectItem pi)
        {
            ClosableTabItem cti = findTabItem(pi);
            if (cti == null)
                createNewDiagram(fs.open(pi.FullPath), pi.Name);
            else
                cti.IsSelected = true;
        }

        public void ModelChanged()
        {
            ClosableTabItem ti = tabs.SelectedItem as ClosableTabItem;
            if (!ti.Title.Contains(modelChangedPostfix))
                changeTabName(modelChangedPostfix + ti.Title);
        }

        public bool IsModelChanged()
        {
            return IsModelChanged(tabs.SelectedItem as ClosableTabItem);
        }

        public bool IsModelChanged(ClosableTabItem ti)
        {
            return ti.Title.Contains(modelChangedPostfix);
        }

        private void errorsListBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            object selected = (sender as ListBox).SelectedItem;

            // WTF? avoiding click for empty listbox
            if (selected != null)
            {
                resetUIShadows(currentCanvas.Children);
                ListBoxItemError error = selected as ListBoxItemError;
                if (error.FailedEntities != null)
                {
                    foreach(UIEntity ent in error.FailedEntities)
                    {
                        ent.errorBitmapEffect();
                    }
                }
            }
        }

        private void ButtonsActivation(bool activate)
        {
            // Файл
            newDiagramButton.IsEnabled = activate;
            openDiagramButton.IsEnabled = activate;
            saveButton.IsEnabled = activate;
            saveAsButton.IsEnabled = activate;
            saveAllButton.IsEnabled = activate;                   
            closeProjectButton.IsEnabled = activate;

            editMenuItem.IsEnabled = activate;
            instrumentMenuItem.IsEnabled = activate;
            formatMenuItem.IsEnabled = activate;
            projectMenuItem.IsEnabled = activate;

            toolBar.IsEnabled = activate;
            instrumentPanel.IsEnabled = activate;            
        }

        private bool needSave()
        {
            foreach (ClosableTabItem i in tabs.Items)
                if (IsModelChanged(i)) return true;
            return false;
        }

        public class ViewModel : System.ComponentModel.INotifyPropertyChanged
        {
            public ViewModel()
            {
                SetInitialState();
            }
            void SetInitialState()
            {
                buttonState = ButtonState.Shown;
                buttonStateBottom = ButtonStateBottom.Shown;
                GridVisibility = Visibility.Visible;
                GridVisibilityBottom = Visibility.Visible;
                ButtonText = "Hide";
                ButtonTextBottom = "Hide";
            }

            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
            private void OnPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this,
                        new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }

            private string buttonText;
            public string ButtonText
            {
                get
                {
                    return buttonText;
                }
                set
                {
                    buttonText = value;
                    OnPropertyChanged("ButtonText");
                }
            }

            private string buttonTextBottom;
            public string ButtonTextBottom
            {
                get
                {
                    return buttonTextBottom;
                }
                set
                {
                    buttonTextBottom = value;
                    OnPropertyChanged("ButtonTextBottom");
                }
            }

            private Visibility gridVisibility;
            public Visibility GridVisibility
            {
                get
                {
                    return gridVisibility;
                }
                set
                {
                    gridVisibility = value;
                    OnPropertyChanged("GridVisibility");
                }
            }

            private MyCommand showHideCommand;
            public MyCommand ShowHidecommand
            {
                get { return showHideCommand ?? (showHideCommand = new MyCommand((o) => OnShowHideCommand(o), () => true)); }
            }

            public void OnShowHideCommand(object obj)
            {
                if (buttonState == ButtonState.Shown)
                {
                    buttonState = ButtonState.Hidden;
                    GridVisibility = Visibility.Collapsed;
                    ButtonText = "Show";
                }
                else
                {
                    buttonState = ButtonState.Shown;
                    GridVisibility = Visibility.Visible;
                    ButtonText = "Hide";
                }

            }

            private Visibility gridVisibilityBottom;
            public Visibility GridVisibilityBottom
            {
                get
                {
                    return gridVisibilityBottom;
                }
                set
                {
                    gridVisibilityBottom = value;
                    OnPropertyChanged("GridVisibilityBottom");
                }
            }

            private MyCommand showHideCommandBottom;
            public MyCommand ShowHidecommandBottom
            {
                get { return showHideCommandBottom ?? (showHideCommandBottom = new MyCommand((o) => OnShowHideCommandBottom(o), () => true)); }
            }

            public void OnShowHideCommandBottom(object obj)
            {
                if (buttonStateBottom == ButtonStateBottom.Shown)
                {
                    buttonStateBottom = ButtonStateBottom.Hidden;
                    GridVisibilityBottom = Visibility.Collapsed;
                    ButtonTextBottom = "Show";
                }
                else
                {
                    buttonStateBottom = ButtonStateBottom.Shown;
                    GridVisibilityBottom = Visibility.Visible;
                    ButtonTextBottom = "Hide";
                }

            }

            ButtonState buttonState;
            enum ButtonState
            {
                Shown,
                Hidden
            }

            ButtonStateBottom buttonStateBottom;
            enum ButtonStateBottom
            {
                Shown,
                Hidden
            }
        }

        public class MyCommand : ICommand
        {
            Action<object> executeAction;
            Func<bool> canExecute;

            public MyCommand(Action<object> executeAction, Func<bool> canExecute)
            {
                this.executeAction = executeAction;
                this.canExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                if (canExecute != null)
                    return canExecute();
                else
                    return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                if (executeAction != null)
                    executeAction(parameter);
            }
        }

    }
}
