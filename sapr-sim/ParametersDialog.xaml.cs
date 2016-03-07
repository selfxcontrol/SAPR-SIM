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

using sapr_sim.Parameters;
using sapr_sim.WPFCustomElements;
using System.Reflection;
using sapr_sim.Utils;
using sapr_sim.Figures;

namespace sapr_sim
{
    
    public partial class ParameterDialog : Window
    {

        private List<UIParam> parameters = new List<UIParam>();
        private sapr_sim.Figures.UIEntity owner;

        public ParameterDialog(List<UIParam> param, sapr_sim.Figures.UIEntity owner)
        {
            InitializeComponent();
            this.parameters = param;
            this.owner = owner;

            image.Source = new BitmapImage(new Uri(@"pack://application:,,,/" + owner.iconPath(), UriKind.Absolute));
            description.Text = owner.description();

            ParameterProccesor.drawParameters(owner, sp, true);
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            bool result;
            if (ParameterProccesor.newResource == null || ParameterProccesor.newResource.GetType() == owner.GetType())
            {
                result = applyNewParams(parameters);
                ParameterProccesor.newResource = null;
            }
            else
            {
                MainWindow mw = (MainWindow)System.Windows.Application.Current.MainWindow;

                Point p = new Point(VisualTreeHelper.GetOffset(owner).X + MainWindow.X_OFFSET * (-1),
                                    VisualTreeHelper.GetOffset(owner).Y + MainWindow.Y_OFFSET * (-1));

                result = applyNewParams(ParameterProccesor.newResource.getParams());

                mw.currentEntity = ParameterProccesor.newResource;
                mw.drawOnCanvas(p);
                mw.selected = ParameterProccesor.newResource;

                restoreConnectionLines(mw);
                mw.deleteEntity(owner);
                
                ParameterProccesor.drawParameters(ParameterProccesor.newResource, sp, false);
                ParameterProccesor.newResource = null;
                mw.currentEntity = null;
            }

            if (result)
            {
                this.Close();
                ((MainWindow)System.Windows.Application.Current.MainWindow).ModelChanged();
            }
        }

        private bool applyNewParams(List<UIParam> parameters)
        {

            UndoRedoManager.putInUndoStack(owner);
            UndoRedoManager.clearRedoStack(owner);

            foreach (UIElement el in sp.Children)
            {
                if (el is DockPanel)
                {
                    DockPanel dp = el as DockPanel;
                    System.Windows.Controls.Label l = dp.Children[0] as System.Windows.Controls.Label;
                    ParameterInput input = dp.Children[1] as ParameterInput;
                    UIParam param = parameters.Find(p => p.DisplayedText == l.Content.ToString());
                    if (!param.Validator.validate(input.getValue().ToString()))
                    {
                        MessageBox.Show("Параметр '" + l.Content + "' задан не верно");
                        return false;
                    }
                    if (sapr_sim.Figures.UIEntity.ENTITY_NAME_PARAM.Equals(param.DisplayedText) && !param.RawValue.Equals(input.getValue().ToString()))
                        owner.updateText(input.getValue().ToString());
                    param.RawValue = input.getValue();
                }
            }
            return true;
        }

        private void restoreConnectionLines(MainWindow mw)
        {
            List<ConnectionLine> connections = ConnectorFinder.find(owner.canvas.Children, owner);
            foreach (ConnectionLine cl in connections)
            {
                // can use ==, because it's same object instance 
                UIEntity connectedTo = cl.SourcePort.Owner != owner ? cl.SourcePort.Owner : cl.DestinationPort.Owner;
                PortType srcPortType = cl.SourcePort.Owner != owner ? cl.SourcePort.PortType : cl.DestinationPort.PortType;
                PortType dstPortType = cl.SourcePort.Owner != owner ? cl.DestinationPort.PortType : cl.SourcePort.PortType;

                if (!(ParameterProccesor.newResource.GetType() == typeof(MaterialResource) && PortType.BOTTOM_RESOURCE.Equals(dstPortType)))
                {
                    ConnectionLine newCl = new ConnectionLine(owner.canvas);

                    newCl.SetBinding(ConnectionLine.SourceProperty, new Binding()
                    {
                        Source = connectedTo.findPort(srcPortType),
                        Path = new PropertyPath(Port.AnchorPointProperty)
                    });
                    newCl.SetBinding(ConnectionLine.DestinationProperty, new Binding()
                    {
                        Source = ParameterProccesor.newResource.findPort(dstPortType),
                        Path = new PropertyPath(Port.AnchorPointProperty)
                    });


                    newCl.MouseLeftButtonDown += mw.Shape_MouseLeftButtonDown;
                    newCl.MouseLeftButtonUp += mw.Shape_MouseLeftButtonUp;

                    ZIndexUtil.setCorrectZIndex(newCl.canvas, newCl);

                    owner.canvas.Children.Add(newCl);
                }
            }
        }

    }

    public static class ParameterProccesor
    {

        public static Resource newResource;

        private static sapr_sim.Figures.UIEntity entity;
        private static StackPanel panel;

        public static void drawParameters(sapr_sim.Figures.UIEntity owner, StackPanel drawPanel, bool paramsEnabled)
        {
            drawPanel.Children.Clear();
            if (owner != null && owner.getParams() != null)
            {
                entity = owner;
                panel = drawPanel;
                foreach (UIParam entry in owner.getParams())
                {

                    DockPanel sprow = new DockPanel() { LastChildFill = true, Margin = new Thickness(0, 2, 0, 5), Height = 28 };
                    System.Windows.Controls.Label l = new System.Windows.Controls.Label() { Content = entry.DisplayedText };
                    ParameterInput input = entry.ContentControl;
                    UIElement uiControl = null;

                    if (input == null)
                    {
                        input = new ParameterTextBox(entry.RawValue, paramsEnabled);
                        uiControl = input as UIElement;
                    }
                    else
                    {
                        input.setValue(entry.RawValue);
                        input = input.Clone() as ParameterInput;
                        uiControl = input as UIElement;
                        uiControl.IsEnabled = paramsEnabled;                        
                    }

                    if (!paramsEnabled)
                    {
                        (uiControl as FrameworkElement).MinWidth = ParameterConstants.QUICK_PANEL_WIDTH;
                        (uiControl as FrameworkElement).MaxWidth = ParameterConstants.QUICK_PANEL_WIDTH;
                    }

                    if (entry.DisplayedText == "Тип ресурса" && input is ParameterComboBox)
                        (input as ComboBox).SelectionChanged += ParameterProccesor_SelectionChanged;
                    
                    sprow.Children.Add(l);
                    sprow.Children.Add(uiControl);

                    if (paramsEnabled)
                    {
                        l.Width = 150;
                        Image tooltip = new Image() 
                        { 
                            Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/param_tooltip.png", UriKind.Absolute)) 
                        };

                        tooltip.ToolTip = new ToolTip() { Content = new TextBox() { Text = entry.ToolTip, TextWrapping = TextWrapping.Wrap, Width = 250 }};
                        tooltip.MouseEnter += tooltip_MouseEnter;
                        tooltip.MouseLeave += tooltip_MouseLeave;

                        sprow.Children.Add(tooltip);
                    }
                    
                    drawPanel.Children.Add(sprow);
                }
            }
        }

        private static void tooltip_MouseLeave(object sender, MouseEventArgs e)
        {
            ((ToolTip)((FrameworkElement)sender).ToolTip).IsOpen = false;
        }

        private static void tooltip_MouseEnter(object sender, MouseEventArgs e)
        {
            ((ToolTip)((FrameworkElement)sender).ToolTip).IsOpen = true;
        }

        static void ParameterProccesor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResourceType oldRt = (e.RemovedItems[0] as ResourceType);
            ResourceType newRt = (e.AddedItems[0] as ResourceType);

            Resource res = entity as Resource;

            if (ResourceType.WORKER.Equals(newRt))
            {
                newResource = new WorkerResource(res.canvas) { Count = res.Count, IsShared = res.IsShared };                   
            }
            else if (ResourceType.INSTRUMENT.Equals(newRt))
            {
                newResource = new InstrumentResource(res.canvas) { Count = res.Count, IsShared = res.IsShared };
            }
            else if (ResourceType.MATERIAL.Equals(newRt))
            {
                newResource = new MaterialResource(res.canvas) { Count = res.Count, IsShared = res.IsShared };
            }

            if (!ResourceType.nameList().Contains(res.EntityName))
                newResource.EntityName = res.EntityName;

            ParameterProccesor.drawParameters(newResource, panel, true);
        }
    }
}
