﻿using IDUNv2.DataAccess;
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
    public sealed partial class ImuTestPage : Page
    {
        double gxmax = double.MinValue;
        double gxmin = double.MaxValue;
        double gymax = double.MinValue;
        double gymin = double.MaxValue;
        double gzmax = double.MinValue;
        double gzmin = double.MaxValue;

        double axmax = double.MinValue;
        double axmin = double.MaxValue;
        double aymax = double.MinValue;
        double aymin = double.MaxValue;
        double azmax = double.MinValue;
        double azmin = double.MaxValue;


        private DispatcherTimer timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(100)
        };

        public ImuTestPage()
        {
            this.InitializeComponent();

            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            var r = DAL.ImuSensorWatcher.Readings;
            if (r.FusionPoseValid)
            {
                
                PitchAxis.Value = r.FusionPose.AsDegrees.X;
                PitchAxisValue.Text = r.FusionPose.AsDegrees.X.ToString("0.0000") + " °";
                RollAxis.Value = r.FusionPose.AsDegrees.Y;
                RollAxisValue.Text = r.FusionPose.AsDegrees.Y.ToString("0.0000") + " °";
                JawAxis.Value = r.FusionPose.AsDegrees.Z;
                JawAxisValue.Text = r.FusionPose.AsDegrees.Z.ToString("0.0000") + " °";
            }

            if (r.AccelerationValid)
            {
                AccelerationX.Value = r.Acceleration.X;
                if (axmax < r.Acceleration.X)
                    axmax = r.Acceleration.X;
                if (axmin > r.Acceleration.X)
                    axmin = r.Acceleration.X;

                AccelerationX.Minimum = axmin;
                AccelerationX.Maximum = axmax;

                AccelerationXValue.Text = r.Acceleration.X.ToString("0.0000") + " g";

                AccelerationY.Value = r.Acceleration.Y;
                if (aymax < r.Acceleration.Y)
                    aymax = r.Acceleration.Y;
                if (aymin > r.Acceleration.Y)
                    aymin = r.Acceleration.Y;

                AccelerationY.Minimum = aymin;
                AccelerationY.Maximum = aymax;

                AccelerationYValue.Text = r.Acceleration.Y.ToString("0.0000") + " g";

                AccelerationZ.Value = r.Acceleration.Z;
                if (azmax < r.Acceleration.Z)
                    azmax = r.Acceleration.Z;
                if (azmin > r.Acceleration.Z)
                    azmin = r.Acceleration.Z;

                AccelerationZ.Minimum = azmin;
                AccelerationZ.Maximum = azmax;

                AccelerationZValue.Text = r.Acceleration.Z.ToString("0.0000") + " g";
            }

            if (r.MagneticFieldValid)
            {
                MagnetX.Value = r.MagneticField.X;
                MagnetXValue.Text = r.MagneticField.X.ToString("0.0000") + " uT";
                MagnetY.Value = r.MagneticField.Y;
                MagnetYValue.Text = r.MagneticField.Y.ToString("0.0000") + " uT";
                MagnetZ.Value = r.MagneticField.Z;
                MagnetZValue.Text = r.MagneticField.Z.ToString("0.0000") + " uT";
            }


            if (r.GyroValid)
            {
                GyroX.Value = r.Gyro.X;
                if (gxmax < r.Gyro.X)
                    gxmax = r.Gyro.X;
                if (gxmin > r.Gyro.X)
                    gxmin = r.Gyro.X;

                GyroX.Minimum = gxmin;
                GyroX.Maximum = gxmax;

                GyroXValue.Text = r.Gyro.AsDegrees.X.ToString("0.0000") + " °/s";

                GyroY.Value = r.Gyro.Y;
                if (gymax < r.Gyro.Y)
                    gymax = r.Gyro.Y;
                if (gymin > r.Gyro.Y)
                    gymin = r.Gyro.Y;

                GyroY.Minimum = gymin;
                GyroY.Maximum = gymax;

                GyroYValue.Text = r.Gyro.AsDegrees.Y.ToString("0.0000") + " °/s";

                GyroZ.Value = r.Gyro.X;
                if (gzmax < r.Gyro.Z)
                    gzmax = r.Gyro.Z;
                if (gzmin > r.Gyro.Z)
                    gzmin = r.Gyro.Z;

                GyroZ.Minimum = gzmin;
                GyroZ.Maximum = gzmax;

                GyroZValue.Text = r.Gyro.AsDegrees.Z.ToString("0.0000") + " °/s";
            }
           
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            timer.Stop();
        }
    }
}