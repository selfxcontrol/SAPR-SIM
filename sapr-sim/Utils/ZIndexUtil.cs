using sapr_sim.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace sapr_sim.Utils
{
    public class ZIndexUtil
    {
        public static void setCorrectZIndex(Canvas currentCanvas, UIEntity currentEntity)
        {
            if (currentEntity is ConnectionLine)
            {
                currentEntity.SetValue(Canvas.ZIndexProperty, 0);
            }
            else if (currentCanvas.Children.Count > 1)
            {
                var maxZ = currentCanvas.Children.OfType<UIElement>()
                                .Where(x => x != currentEntity)
                                .Select(x => Panel.GetZIndex(x))
                                .Max();
                maxZ = maxZ == 0 ? 1 : maxZ;

                foreach (Port port in currentEntity.getPorts())
                    port.SetValue(Canvas.ZIndexProperty, maxZ + 2);

                if (currentEntity.Label != null)
                    currentEntity.Label.SetValue(Canvas.ZIndexProperty, maxZ + 1);

                currentEntity.SetValue(Canvas.ZIndexProperty, maxZ);
            }
        }

    }
}
