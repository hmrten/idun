﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using SenseHat;
using TestInterface.HumidityControl;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TestInterface
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HumidityPage : Page
    {
        private Queue<HumControl> HumNdTime = new Queue<HumControl>();

        public HumidityPage()
        {
            this.InitializeComponent();
            //Removing Legend Item from the Chart
            HumChart.LegendItems.Clear();            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            (Application.Current as TestInterface.App).SenseHatReader.Tick += SenseHatReaderTick;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            (Application.Current as TestInterface.App).SenseHatReader.Tick -= SenseHatReaderTick;
        }

        private void SenseHatReaderTick(SenseHatReader reader, SenseHatReading reading)
        {
            var humi = reading.Humidity;
            btnCurrentHumi.Content = String.Format("Relative\nHumidity:\n{0:f2} %", humi);

            //Self-truncating Stack List
            if (HumNdTime.Count >= 15)
            {
                HumNdTime.Dequeue();

            }
            HumNdTime.Enqueue(new HumControl { Humidity = double.Parse(humi.ToString()), DTReading = DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") });

            (HumChart.Series[0] as LineSeries).ItemsSource = HumNdTime.ToList();
        }

        private void btnBACK_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), null);
        }

    }
}
