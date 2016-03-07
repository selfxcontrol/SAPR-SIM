using sapr_sim.Parameters;
using sapr_sim.Utils;
using sapr_sim.WPFCustomElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace sapr_sim
{
    public class Project
    {

        public static readonly string SRC_FOLDER_NAME = "src";
        private static Project instance = new Project();

        public string ProjectName { get; set; }
        public string ProjectPath { get; set; }
        public ProjectItem MainProjectItem { get; set; } 
        public TimeParam TimeRestiction { get; set; }
        public bool SaveResult { get; set; }
        public string ResultPath { get; set; }

        private List<ProjectItem> items = new List<ProjectItem>();

        private Project() 
        {
            TimeRestiction = new TimeParam(60, TimeMeasure.SECOND);
        }

        public static Project Instance
        {
            get { return instance; }
        }

        public string SourcesFullPath
        {
            get { return FullPath + "\\" + SRC_FOLDER_NAME; }
        }

        public string FullPath
        {
            get { return ProjectPath + "\\" + ProjectName; }
        }

        public string FullName
        {
            get { return FullPath + "\\" + ProjectName + FileService.PROJECT_EXTENSION; }
        }

        public bool IsLoaded
        {
            get { return !String.IsNullOrEmpty(ProjectPath) && !String.IsNullOrEmpty(ProjectName); }
        }

        public List<ProjectItem> Items
        {
            get { return items; }
            set { items = value; }
        }

        public string PathToItem(ProjectItem item)
        {
            return FullPath + "\\" + SRC_FOLDER_NAME + "\\" + item.Name + FileService.PROJECT_ITEM_EXTENSION;
        }

        public void addProjectItem(ProjectItem item)
        {
            items.Add(item);
            if (items.Count == 1 && MainProjectItem == null)
                MainProjectItem = item;
        }

        public void removeProjectItem(ProjectItem item)
        {
            items.Remove(item);

            if (item.Equals(MainProjectItem))
                if (items.Count > 0)
                    MainProjectItem = items[0];
                else
                    MainProjectItem = null;
        }

        public ProjectItem byCanvas(ScrollableCanvas canvas)
        {
            return items.Where(i => canvas.Equals(i.Canvas as ScrollableCanvas, canvas)).First();
        }

        public void Close()
        {
            items.Clear();
            ProjectName = "";
            ProjectPath = "";
        }
    }
}
