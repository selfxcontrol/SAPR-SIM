using sapr_sim.Figures;
using sapr_sim.Parameters;
using sapr_sim.WPFCustomElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace sapr_sim
{
    public partial class MainWindow : Window
    {

        // used in moving figure on canvas
        public UIEntity selected { get; set; }
        private bool captured = false;

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (selected != null && !captured)
            {
                selected.defaultBitmapEffect();
                propertiesPanel.Children.Clear();
                selected = null;
            }
        }

        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentCanvas.Cursor = Cursors.Arrow;
            currentEntity = null;
            firstConnect = true;
        }

        public void Shape_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                UndoRedoManager.putInUndoStack((ScrollableCanvas)currentCanvas);
                UndoRedoManager.clearRedoStack((ScrollableCanvas)currentCanvas);

                // clear bitmap effect for previous selected entity
                if (selected != null)
                    selected.defaultBitmapEffect();

                selected = sender as UIEntity;
                Mouse.Capture(selected);
                captured = true;

                double xCanvas = e.GetPosition(this).X;
                double yCanvas = e.GetPosition(this).Y;

                selected.putMovingCoordinate(selected,
                    VisualTreeHelper.GetOffset(selected).X,
                    VisualTreeHelper.GetOffset(selected).Y,
                    xCanvas, yCanvas);

                if (selected.Label != null)
                {
                    selected.putMovingCoordinate(selected.Label,
                        VisualTreeHelper.GetOffset(selected.Label).X,
                        VisualTreeHelper.GetOffset(selected.Label).Y,
                        xCanvas, yCanvas);
                }

                foreach (Port p in selected.getPorts())
                {
                    selected.putMovingCoordinate(
                        p,
                        VisualTreeHelper.GetOffset(p).X,
                        VisualTreeHelper.GetOffset(p).Y,
                        xCanvas, yCanvas);
                }

                if (!(selected is ConnectionLine && selected is Port))
                    selected.selectedBitmapEffect();

                ParameterProccesor.drawParameters(selected, propertiesPanel, false);
            }
            else if (e.ClickCount == 2 && !(sender is Port || sender is ConnectionLine))
                OpenShapeProperties_Click(sender, e);
        }

        public void Shape_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                // clear bitmap effect for previous selected entity
                if (selected != null)
                    selected.defaultBitmapEffect();

                selected = sender as UIEntity;
                if (!(selected is ConnectionLine && selected is Port))
                    selected.selectedBitmapEffect();
            }
        }

        public void Shape_MouseMove(object sender, MouseEventArgs e)
        {
            if (captured && !(selected is Port || selected is ConnectionLine))
            {
                double x = e.GetPosition(this).X;
                double y = e.GetPosition(this).Y;

                processCoordinatesHandler(selected, x, y);
                if (selected.Label != null) processCoordinatesHandler(selected.Label, x, y);                

                foreach (Port p in selected.getPorts())
                {
                    processCoordinatesHandler(p, x, y);
                }
                ModelChanged();
            }
        }

        public void Shape_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
            captured = false;
        }

        private void processCoordinatesHandler(UIEntity ent, double x, double y)
        {
            UIEntity.CoordinatesHandler ch = selected.getMovingCoordinate(ent);
            ch.xShape += x - ch.xCanvas;
            ch.yShape += y - ch.yCanvas;
            Canvas.SetLeft(ent, ch.xShape);
            Canvas.SetTop(ent, ch.yShape);
            ch.xCanvas = x;
            ch.yCanvas = y;
        }
    }
}
