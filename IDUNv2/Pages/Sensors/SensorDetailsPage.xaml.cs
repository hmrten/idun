﻿using IDUNv2.Common;
using IDUNv2.DataAccess;
using IDUNv2.Models;
using IDUNv2.SensorLib;
using IDUNv2.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public class SensorDetailsViewModel : NotifyBase
    {
        private Sensor _sensor;
        private float _bias;

        public Sensor Sensor
        {
            get { return _sensor; }
            set { _sensor = value; Notify(); }
        }

        public float Bias
        {
            get { return _bias; }
            set { _bias = value; Notify(); DAL.SetSensorBias(Sensor.Id, value); }
        }
    }

    public sealed partial class SensorDetailsPage : Page
    {
        private DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(10) };
        private Random rnd = new Random();
        private SensorDetailsViewModel viewModel = new SensorDetailsViewModel();

        public SensorDetailsPage()
        {
            this.InitializeComponent();
            this.DataContext = viewModel;

            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            float v = viewModel.Sensor.Value;
            SG.AddDataPoint(v);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var sensor = e.Parameter as Sensor;

            SG.SetRange(sensor.RangeMin, sensor.RangeMax);
            SG.SetDanger(sensor.DangerLo, sensor.DangerHi);

            viewModel.Sensor = sensor;

            var cmdBarItems = new CmdBarItem[]
            {
                new CmdBarItem(Symbol.Setting, "Settings", o => Frame.Navigate(typeof(SensorSettingsPage), sensor)),
                new CmdBarItem(Symbol.Repair, "Repair", o => DAL.ClearSensorFaultState(viewModel.Sensor.Id)),
                new CmdBarItem(Symbol.Clear, "Clear Bias", o => viewModel.Bias = 0)
            };

            DAL.PushNavLink(new NavLinkItem(viewModel.Sensor.Id.ToString(), GetType(), e.Parameter));
            DAL.SetCmdBarItems(cmdBarItems);

            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
        }

        private void CompositionTarget_Rendering(object sender, object e)
        {
            SG.Render();
        }
    }
}