using sapr_sim.WPFCustomElements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Serialization;

namespace sapr_sim.Utils
{
    public class FileService
    {

        public static readonly string PROJECT_EXTENSION = ".ssp";
        public static readonly string PROJECT_ITEM_EXTENSION = ".ssm";

        public static bool IsValidPath(string path)
        {
            return Regex.IsMatch(path, @"^(?:[\w]\:|\\)(\\[a-z_\-\s0-9\.]*)+");
        }

        public ScrollableCanvas open(string filepath)
        {
            using (FileStream fs = new FileStream(filepath, FileMode.Open))
            {
                return (ScrollableCanvas) new BinaryFormatter().Deserialize(fs);
            }
        }

        public void save(Canvas canvas, string filepath)
        {
            string path = filepath.Substring(0, filepath.LastIndexOf("\\"));
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path); 
            using (FileStream filestream = new FileStream(filepath, FileMode.OpenOrCreate))
            {
                new BinaryFormatter().Serialize(filestream, canvas);
            }
        }

        public void saveProject()
        {
            Project prj = Project.Instance;
            string pathToProject = prj.FullPath;
            if (!Directory.Exists(pathToProject))
                Directory.CreateDirectory(pathToProject);

            string projectFile = pathToProject + "\\" + prj.ProjectName + PROJECT_EXTENSION;
            XmlSerializer serializer = new XmlSerializer(typeof(Project));
            using (var writer = new StreamWriter(projectFile))
            {
                serializer.Serialize(writer, Project.Instance);
            }
        }

        public void openProject(string filepath)
        {
            if (String.IsNullOrWhiteSpace(filepath))
                throw new ArgumentException();
            if (!File.Exists(filepath) || !filepath.Contains(PROJECT_EXTENSION))
                throw new ProjectException("Указана неверная директория");

            XmlSerializer serializer = new XmlSerializer(typeof(Project));
            using (XmlReader reader = new XmlTextReader(filepath))
            {
                Project openedProject = (Project) serializer.Deserialize(reader);                
                reader.Close();

                string path = filepath.Substring(0, filepath.LastIndexOf("\\"));
                path = path.Substring(0, path.LastIndexOf("\\"));

                // we must set all properties from opened project to our project singletone              
                Project.Instance.ProjectName = openedProject.ProjectName;
                Project.Instance.ProjectPath = openedProject.ProjectPath == path ? openedProject.ProjectPath : path;
                Project.Instance.ResultPath = openedProject.ResultPath;
                Project.Instance.SaveResult = openedProject.SaveResult;
                Project.Instance.TimeRestiction = openedProject.TimeRestiction;
                foreach (ProjectItem item in openedProject.Items)
                {
                    item.Canvas = open(item.FullPath);
                    Project.Instance.addProjectItem(item);

                    if (openedProject.MainProjectItem != null && item.Name == openedProject.MainProjectItem.Name)
                        Project.Instance.MainProjectItem = item;
                }                
            }
        }

        public void renameProject(string newName)
        {
            File.Move(Project.Instance.FullName, Project.Instance.FullPath + "\\" + newName + PROJECT_EXTENSION);
            Directory.Move(Project.Instance.FullPath, Project.Instance.ProjectPath + "\\" + newName);
            Project.Instance.ProjectName = newName;
            saveProject();
        }

        public void renameProjectItem(ProjectItem pi, string newName)
        {
            string newPath = Project.Instance.SourcesFullPath + "\\" + newName + PROJECT_ITEM_EXTENSION;
            if (!File.Exists(newPath))
                File.Move(pi.FullPath, newPath);
            else
            {
                File.Delete(pi.FullPath);
            }
            pi.Name = newName;
            save(pi.Canvas, pi.FullPath);
            saveProject();
        }
    }
}
