using SenseHat;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private SensorReader sensorReader = new DummyReader(1000, 15);

        public MainPage()
        {
            this.InitializeComponent();
            sensorReader.Init();
        }

        private void SensorReader_Tick(SensorReader reader, SensorData data)
        {
            listBox.ItemsSource = reader.Data.Select(r =>
                String.Format("{0:F2} C, {1:F2} %, {2:F3} hPa",
                r.Temperature, r.Humidity, r.Pressure));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            sensorReader.Tick += SensorReader_Tick;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            sensorReader.Tick -= SensorReader_Tick;
        }
    }
}
