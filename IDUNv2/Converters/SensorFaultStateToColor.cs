﻿using IDUNv2.SensorLib;
using System;
using Windows.UI.Xaml.Data;

namespace IDUNv2.Converters
{
    public class SensorFaultStateToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return "Cyan";
            var state = (SensorFaultState)value;
            if (state == SensorFaultState.Faulted)
                return "Red";
            else
                return "#FF00CC00";
        }

        // Not used
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return SensorFaultState.Normal;
        }
    }
}
