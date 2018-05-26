using Central_LED.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Central_LED.Converter
{
    public class LineControlGridVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if(value.GetType() == typeof(int))
            {
                var convertedValue = (LineType)value;
                var convertedparameter = (LineType)parameter;

                if(convertedparameter == convertedValue)
                {
                    return Visibility.Visible;
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }       
    }
}
