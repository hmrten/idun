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
using TestInterface.TemperatureControl;
using System.Collections.ObjectModel;
using System.ServiceModel.Channels;
using static System.Collections.Specialized.BitVector32;
using System.Threading.Tasks;
using System.Threading;
using TestInterface.Report;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TestInterface
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public string RHumi;
        public string RPres;
        public string RTemp;

        public MainPage()
        {
            this.InitializeComponent();

            (Application.Current as TestInterface.App).humiRead.CollectionChanged += HumiRead_CollectionChanged;
            (Application.Current as TestInterface.App).tempRead.CollectionChanged += TempRead_CollectionChanged;
            (Application.Current as TestInterface.App).presRead.CollectionChanged += PresRead_CollectionChanged;
           
        }

        private void PresRead_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            btnPressure.Content = String.Format("Pressure:\n{0:f2} hPa ", (Application.Current as TestInterface.App).presRead.Last());
        }

        private void TempRead_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            btnTemp.Content = string.Format("Temperature:\n{0:f2} °C", (Application.Current as TestInterface.App).tempRead.Last());
        }

        private void HumiRead_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            btnHumidity.Content = String.Format("Relative\nHumidity:\n{0:f2} % ", (Application.Current as TestInterface.App).humiRead.Last());
        }


        private void btnTemp_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(TemperaturePage), null);
        }

        private void btnPressure_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PressurePage), null);
            
        }

        private void btnHumidity_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HumidityPage), null);
        }

        private void btnStatus_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StatusBar), null); 
        }


        private void onLoadMCounter(object sender, RoutedEventArgs e)
        {
            btnMCounter.Content = "Usage Calls:\n" + (Application.Current as TestInterface.App).currentNrofServiceCalls + "\nout of\n" + (Application.Current as TestInterface.App).MaxNrBfrMaintenance;
        }

        private void barOnLoad(object sender, RoutedEventArgs e)
        {
            //Progress Bar on Load
            int barMaxint = (Application.Current as TestInterface.App).MaxNrBfrMaintenance;
            double barMax = double.Parse(barMaxint.ToString());
            ProgBarforStatus.Maximum = barMax;

            int barCurrentint = (Application.Current as TestInterface.App).currentNrofServiceCalls;
            double barCur = double.Parse(barCurrentint.ToString());
            ProgBarforStatus.Value = barCur;
        }

        private void btnAAS_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AdditionalApps), null);
        }

        private void btnMCounterClick(object sender, RoutedEventArgs e)
        {
            //Global number of Servie Calls
            if ((Application.Current as TestInterface.App).MaxNrBfrMaintenance > (Application.Current as TestInterface.App).currentNrofServiceCalls)
            {
                ++(Application.Current as TestInterface.App).currentNrofServiceCalls;
            }
            //Service Call after Current Number of Service Calls reaches its' maximum
            if ((Application.Current as TestInterface.App).MaxNrBfrMaintenance == (Application.Current as TestInterface.App).currentNrofServiceCalls)
            {
                //Data Collection for Global List
                string Datum = " " + DateTime.Now.Year.ToString("0000") + "-" + DateTime.Now.Month.ToString("00") + "-" + DateTime.Now.Day.ToString("00") + " on " + DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00") + " ";
                RHumi = " Humidity: " + string.Format("0:0.00", (Application.Current as TestInterface.App).humiRead.Last().ToString()) + "% ";
                RPres = " Pressure: " + string.Format("0:0.00", (Application.Current as TestInterface.App).presRead.Last().ToString()) + "hPa ";
                RTemp = " Temperature: " + string.Format("0:0.00", (Application.Current as TestInterface.App).tempRead.Last().ToString()) + "°C ";
                //Global List with Service Calls
                (Application.Current as TestInterface.App).ReportForMain.Insert(0, new Report.ReportList { DTofServiceCall = Datum, SCHumidity = RHumi, SCPressure = RPres, SCTemperature = RTemp, MaxNr = " Maintenance After " + (Application.Current as TestInterface.App).currentNrofServiceCalls.ToString() + " Run(s) ", Note = " Note: Automatic Insertion " });
                (Application.Current as TestInterface.App).currentNrofServiceCalls = 0;

                //Local List with Service Calls
                ListViewTest.ItemsSource = (Application.Current as TestInterface.App).ReportForMain;

                //Button with Report
                button.Content = "Service Called " + (Application.Current as TestInterface.App).ReportForMain.Count + " time(s)";
            }
            //Button with Current Calls Value
            btnMCounter.Content = "Usage Calls:\n" + (Application.Current as TestInterface.App).currentNrofServiceCalls + "\nout of\n" + (Application.Current as TestInterface.App).MaxNrBfrMaintenance;

            //Progress Bar
            int barMaxint = (Application.Current as TestInterface.App).MaxNrBfrMaintenance;
            double barMax = double.Parse(barMaxint.ToString());
            ProgBarforStatus.Maximum = barMax;
            int barCurrentint = (Application.Current as TestInterface.App).currentNrofServiceCalls;
            double barCur = double.Parse(barCurrentint.ToString());
            ProgBarforStatus.Value = barCur;
        }

        private void onLoadList(object sender, RoutedEventArgs e)
        {
            //List on Load
            ListViewTest.ItemsSource = (Application.Current as TestInterface.App).ReportForMain;
        }

        private void onLoadSCall(object sender, RoutedEventArgs e)
        {
            button.Content = "Service Called " + (Application.Current as TestInterface.App).ReportForMain.Count + " time(s)";
        }
    }

}
