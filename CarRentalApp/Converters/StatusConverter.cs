using System;
using System.Globalization;
using System.Windows.Data;

namespace CarRentalApp.Converters
{
    public class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isAvailable)
                return isAvailable ? "Dostępny" : "Wypożyczony";
            return "Nieznany";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}