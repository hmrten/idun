using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using SenseHat;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SenseHatIMU.src;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TestInterface
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    

    public sealed partial class IMURead : Page
    {
        private readonly SensorThread _sensorThread = new SensorThread();
        private readonly DispatcherTimer _timer = new DispatcherTimer();


        public IMURead()
        {
            this.InitializeComponent();

            _timer.Interval = TimeSpan.FromSeconds(0.1);
            _timer.Tick += (s, e) => UpdateValues();
            _timer.Start();

        }



        private void UpdateValues()
        {
            ImuSensorData imuSensorData = _sensorThread.GetImuSensorData();

            ImuInitiated.Text = imuSensorData.Initiated ? "Yes" : "No";

         
            ImuGyro.Text = ReadingToString(imuSensorData.Readings.GyroValid, imuSensorData.Readings.Gyro, false, "radians/s");
            ImuAccel.Text = ReadingToString(imuSensorData.Readings.AccelerationValid, imuSensorData.Readings.Acceleration, false, "g");
            ImuMag.Text = ReadingToString(imuSensorData.Readings.MagneticFieldValid, imuSensorData.Readings.MagneticField, false, "\u00B5T");

            FusionPose.Text = ReadingToString(imuSensorData.Readings.FusionPoseValid, imuSensorData.Readings.FusionPose, true, "");
            FusionQPose.Text = ReadingToString(imuSensorData.Readings.FusionQPoseValid, imuSensorData.Readings.FusionQPose, "");

            
        }

        private static string ReadingToString(bool valid, Vector3 vector, bool asDegrees, string unit)
        {
            return !valid ? "N/A" : vector.ToString(asDegrees) + " " + unit;
        }

        private static string ReadingToString(bool valid, Quaternion quaternion, string unit)
        {
            return !valid ? "N/A" : quaternion + " " + unit;
        }

        private static string ReadingToString(bool valid, double value, string unit)
        {
            return !valid ? "N/A" : $"{value:0.0000} {unit}";
        }

        private void btnBACK_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            _sensorThread.Dispose();
            this.Frame.Navigate(typeof(AdditionalApps), null);
        }
    }
}
