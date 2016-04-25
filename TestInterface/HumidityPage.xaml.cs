using Syncfusion.UI.Xaml.Charts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

            (Application.Current as TestInterface.App).humiRead.CollectionChanged += HumiRead_CollectionChanged;
            
        }

        private void HumiRead_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            btnCurrentHumi.Content = string.Format("Relative\nHumidity:\n{0:f2} %", (Application.Current as TestInterface.App).humiRead.Last());

            if (HumNdTime.Count >= 15)
            {
                HumNdTime.Dequeue();

            }
            HumNdTime.Enqueue(new HumControl { Humidity = double.Parse((Application.Current as TestInterface.App).humiRead.Last().ToString()), DTReading = DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") });

            (sfchart.Series[0] as FastLineSeries).ItemsSource = HumNdTime.ToList();

        }


        private void btnBACK_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), null);
        }

    }
}
