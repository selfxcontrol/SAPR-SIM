using sapr_sim.Utils;
using sapr_sim.WPFCustomElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace sapr_sim
{
    [Serializable]
    public class ProjectItem
    {

        private Canvas canvas;
        private string name;

        public ProjectItem()
        {
            // for xml serialization only
        }

        public ProjectItem(string name)
        {
            this.name = name;
        }

        public ProjectItem(Canvas canvas, string name)
        {
            this.canvas = canvas;
            this.name = name;
        }

        [XmlIgnore]
        public Canvas Canvas
        {
            get { return canvas; }
            set { canvas = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string FullPath
        {
            get { return Project.Instance.FullPath + "\\" + Project.SRC_FOLDER_NAME + "\\" + name + FileService.PROJECT_ITEM_EXTENSION; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(ProjectItem)) return false;

            ProjectItem another = obj as ProjectItem;

            // C#... 
            ScrollableCanvas thisCanvas = canvas as ScrollableCanvas;
            ScrollableCanvas anotherCanvas = another.Canvas as ScrollableCanvas;

            return thisCanvas.Equals(thisCanvas, anotherCanvas);
        }

        public override int GetHashCode()
        {
            return canvas.GetHashCode();
        }
    }
}
