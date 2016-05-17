﻿using Entities;
using EntityTransformator;
using Kernel;
using sapr_sim.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
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
using Statistics;
using System.Collections.ObjectModel;
using Statistics.Beans;
using Statistics.extractors;
using Statistics.Extractors.impl;
using DB;
using Entities.impl;
using DB.DAO;

namespace sapr_sim
{

    public partial class RunSimulation : Window
    {

        private Canvas canvas;
        private Controller controller;

        public RunSimulation(ProjectItem pi, Controller controller)
        {
            InitializeComponent();
            this.canvas = pi.Canvas;
            this.controller = controller;
            processName.Content = pi.Name;
            showChart();
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {

            Model.Instance.timeRestriction = TimeConverter.fromHumanToModel(Project.Instance.TimeRestiction);
            TimeTrackerEngine.clear();

            controller.simulate();

            TimeWithMeasure simulationTime = TimeConverter.fromModelToHuman(controller.SimulationTime);
            time.Content = Math.Round(simulationTime.doubleValue, 3) + " " + simulationTime.measure.Name;
            runStatus.Content = ProcessingStateMethods.GetDescription(controller.SimulationState);
            totalRunCount.Content = totalRunCount.Content == null ? 1 : int.Parse(totalRunCount.Content.ToString()) + 1;
            if (controller.SimulationState == ProcessingState.FINE)
                image.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/success.png", UriKind.Absolute));
            else
                image.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/failure.png", UriKind.Absolute));

            TimeTrackerEngine.fullTime = Timer.Instance.getTime();

            ProjectsStatisticsDE prjExt = new ProjectsStatisticsDE();
            ICollection<ProjectBean> projectStat = prjExt.extract();

            ProcedureStatisticsDE procExtr = new ProcedureStatisticsDE();
            ICollection<ProcedureBean> procedureStat = procExtr.extract();

            ResourceStatisticDE reExt = new ResourceStatisticDE();
            ICollection<ResourceBean> resourseStat = reExt.extract();

            taskTab.ItemsSource = projectStat;
            procedureTab.ItemsSource = procedureStat;
            resourceTab.ItemsSource = resourseStat;

            
        }
        private void showChart()
        {
            List<KeyValuePair<string, int>> MyValue = new List<KeyValuePair<string, int>>();
            MyValue.Add(new KeyValuePair<string, int>("Инженер", 120));
            MyValue.Add(new KeyValuePair<string, int>("Компьютер", 140));
            MyValue.Add(new KeyValuePair<string, int>("Помошник инженера", 50));
            MyValue.Add(new KeyValuePair<string, int>("Проектировщик", 20));
            //MyValue.Add(new KeyValuePair<string, int>("Руководитель", 140));

            List<KeyValuePair<string, int>> MyValue2 = new List<KeyValuePair<string, int>>();
            MyValue2.Add(new KeyValuePair<string, int>("Сбор требований", 3));
            MyValue2.Add(new KeyValuePair<string, int>("Задание параметров", 1));
            MyValue2.Add(new KeyValuePair<string, int>("Оптимизация связей", 2));
            MyValue2.Add(new KeyValuePair<string, int>("Разводка трасс", 1));


            ColumnChart1.DataContext = MyValue2;
            //AreaChart1.DataContext = MyValue2;
            PieChart1.DataContext = MyValue;

        }
    }
}
