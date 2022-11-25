using OTK.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace OTK.Converters
{
    internal class StatusJobToStringConverter : IValueConverter
    {
        string result = "";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is EnumStatusJob status)
            {
                result = ClassStatusJob.NameStatus[(int)status];
            }

            return result;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
