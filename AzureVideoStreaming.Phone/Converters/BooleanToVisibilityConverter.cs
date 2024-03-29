﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AzureVideoStreaming.Phone.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isVisible = false;
            if (value != null)
            {
                if (value is bool && (bool)value)
                {
                    isVisible = true;
                }
            }

            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility && (Visibility)value == Visibility.Visible;
        }
    }
}
