﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TestInterface.TemperatureControl;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TestInterface
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TemperaturePage : Page
    {
        private Queue<TempControl> TempNdTime = new Queue<TempControl>();
        private string TempUnit;
        public double ConvertedTemp;


        public TemperaturePage()
        {
            this.InitializeComponent();

            TempChart.LegendItems.Clear();

            TempUnit = "°C";

            (Application.Current as TestInterface.App).TempCallbacks += TempCallback;

        }

        private void TempCallback(float temp)
        {
           ConvertedTemp = temp;
           btnCurrentTemp.Content = string.Format("Temperature:\n{0:f2} "+ TempUnit, ConvertedTemp);

            //Self-truncating Stack List
            if (TempNdTime.Count >= 15)
            {
                TempNdTime.Dequeue();

            }
            TempNdTime.Enqueue(new TempControl { Temperature = ConvertedTemp, DTReading =DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") });

            (TempChart.Series[0] as LineSeries).ItemsSource = TempNdTime.ToList();
        }

        private void btnBACK_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), null);
        }

        private void onLoadMUnitBtn(object sender, RoutedEventArgs e)
        {
            btnMUnit.Content = "Measuring\nUnit:\n" + TempUnit;
        }


    }
}