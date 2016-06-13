﻿using IDUNv2.Common;
using IDUNv2.DataAccess;
using IDUNv2.SensorLib;
using IDUNv2.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace IDUNv2.Pages
{
    public sealed partial class SensorOverviewPage : Page
    {
        private DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
        private Random rnd = new Random();
        private SensorOverviewViewModel viewModel = new SensorOverviewViewModel();

        private Action<object> ShowDetailsFor(Sensor s)
        {
            return o => Frame.Navigate(typeof(Pages.SensorDetailsPage), s);
        }

        public SensorOverviewPage()
        {
            this.InitializeComponent();

            viewModel.TemperatureSensor.Command = new ActionCommand<object>(ShowDetailsFor(DAL.GetSensor(SensorId.Temperature)));
            viewModel.HumiditySensor.Command = new ActionCommand<object>(ShowDetailsFor(DAL.GetSensor(SensorId.Humidity)));
            viewModel.PressureSensor.Command = new ActionCommand<object>(ShowDetailsFor(DAL.GetSensor(SensorId.Pressure)));

            this.DataContext = viewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            timer.Tick -= Timer_Tick;
            timer.Stop();
        }

        private void Timer_Tick(object sender, object e)
        {
            //SensorReadings readings;

            //if (DAL.HasSensors())
            //{
            //    readings = DAL.GetSensorReadings();
            //}
            //else
            //{
            //    readings.Temperature = (float)(30.0 + rnd.NextDouble() * 2.1);
            //    readings.Humidity = (float)(30.0 + rnd.NextDouble() * 5.1);
            //    readings.Pressure = 1000.0f + (float)(50.0 - rnd.NextDouble() * 100.0);
            //}

            //readings.Temperature += viewModel.BiasTemp;
            //readings.Humidity += viewModel.BiasHumid;
            //readings.Pressure += viewModel.BiasPress;

            //DAL.UpdateSensors(readings);
        }
    }
}
