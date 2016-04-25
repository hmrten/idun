using Syncfusion.UI.Xaml.Charts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TestInterface
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PressurePage : Page
    {
        private Queue<PressControl> PressNdTime = new Queue<PressControl>();

        public PressurePage()
        {
            this.InitializeComponent();

            (Application.Current as TestInterface.App).presRead.CollectionChanged += PresRead_CollectionChanged;
        }

        private void PresRead_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            btnCurrentPress.Content = String.Format("Pressure:\n{0:f2} hPa", (Application.Current as TestInterface.App).presRead.Last());

            //Self-truncating Stack List
            if (PressNdTime.Count >= 15)
            {
                PressNdTime.Dequeue();

            }
            PressNdTime.Enqueue(new PressControl { Pressure = double.Parse((Application.Current as TestInterface.App).presRead.Last().ToString()), DTReading = DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") });

            (sfchart.Series[0] as FastLineSeries).ItemsSource = PressNdTime.ToList();
        }


        private void btnBACK_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), null);
        }
    }
}
