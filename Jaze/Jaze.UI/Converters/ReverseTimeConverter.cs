using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Jaze.UI.Converters
{
    public class ReverseTimeConverter : ConverterBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                var diff = DateTime.Now - dateTime;
                if (diff.Days > 0)
                {
                    return $"{diff.Days}d ago";
                }

                if (diff.Hours > 0)
                {
                    return $"{diff.Hours}h ago";
                }

                if (diff.Minutes > 0)
                {
                    return $"{diff.Minutes}m ago";
                }
                return $"{diff.Seconds}s ago";
            }
            else
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}