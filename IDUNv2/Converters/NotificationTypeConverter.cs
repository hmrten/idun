﻿using IDUNv2.Models;
using System;
using Windows.UI.Xaml.Data;

namespace IDUNv2.Converters
{
    public sealed class NotificationTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var n = value as Notification;
            if (n == null)
                return "";
            switch (n.Type)
            {
                case NotificationType.Error: return "\xE730";
                case NotificationType.Warning: return "\xE8C9";
                case NotificationType.Information: return "\xE8BD";
                case NotificationType.Tooltip: return "";
            }
            return "";
        }

        // Not used
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return NotificationType.Error;
        }
    }
}
