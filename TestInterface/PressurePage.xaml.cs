using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using SenseHat;
using TestInterface.PressureControl;
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
    public sealed partial class PressurePage : Page
    {
        //private Queue<PressControl> PressNdTime = new Queue<PressControl>();

        public PressurePage()
        {
            this.InitializeComponent();

            PressChart.LegendItems.Clear();
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
            var press = data.Pressure;
            btnCurrentPress.Content = String.Format("Pressure:\n{0:f2} hPa", press);

            var items = from r in reader.Data
                        select new
                        {
                            Pressure = r.Pressure,
                            DTReading = r.Date.ToString("HH:mm:ss")
                        };

            (PressChart.Series[0] as LineSeries).ItemsSource = items;
        }

        private void btnBACK_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), null);
        }
    }
}
