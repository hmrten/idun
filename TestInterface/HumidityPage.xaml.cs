using System;
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
        //private Queue<HumControl> HumNdTime = new Queue<HumControl>();

        public HumidityPage()
        {
            this.InitializeComponent();
            //Removing Legend Item from the Chart
            HumChart.LegendItems.Clear();            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            (Application.Current as TestInterface.App).SensorReader.Tick += SensorReader_Tick;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            (Application.Current as TestInterface.App).SensorReader.Tick -= SensorReader_Tick;
        }

        private void SensorReader_Tick(SensorReader reader, SensorData data)
        {
            var humi = data.Humidity;
            btnCurrentHumi.Content = String.Format("Relative\nHumidity:\n{0:f2} %", humi);


            //Self-truncating Stack List
            //if (HumNdTime.Count >= 15)
            //{
            //    HumNdTime.Dequeue();

            //}
            //HumNdTime.Enqueue(new HumControl { Humidity = double.Parse(humi.ToString()), DTReading = DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") });

            //var now = DateTime.Now;
            var items = from r in reader.Data
                        select new
                        {
                            Humidity = r.Humidity,
                            DTReading = r.Date.ToString("HH:mm:ss")
                        };

            foreach (var ihumi in items)
            {
                (Application.Current as TestInterface.App).MaxHumi = ihumi.Humidity > (Application.Current as TestInterface.App).MaxHumi ? ihumi.Humidity : (Application.Current as TestInterface.App).MaxHumi;
                (Application.Current as TestInterface.App).MinHumi = ihumi.Humidity < (Application.Current as TestInterface.App).MinHumi ? ihumi.Humidity : (Application.Current as TestInterface.App).MinHumi;
            }
            btnMaxHumi.Content = "Maximal\nMeasured\nHumidity:\n" + string.Format("{0:f2} %", (Application.Current as TestInterface.App).MaxHumi);
            btnMinHumi.Content = "Minimal\nMeasured\nHumidity:\n" + string.Format("{0:f2} %", (Application.Current as TestInterface.App).MinHumi);

            (HumChart.Series[0] as LineSeries).ItemsSource = items;
        }

        private void btnBACK_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), null);
        }

    }
}
