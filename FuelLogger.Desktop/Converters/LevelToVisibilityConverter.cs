using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FuelLogger.Desktop.Converters
{
    public class LevelToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int level)
                return level == 1 ? Visibility.Visible : Visibility.Collapsed;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}