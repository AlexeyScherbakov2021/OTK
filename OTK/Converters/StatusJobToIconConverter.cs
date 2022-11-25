using OTK.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace OTK.Converters
{
    internal class StatusJobToIconConverter : IValueConverter
    {
        BitmapImage Source = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is EnumStatusJob status)
            {

                switch (status)
                {
                    case EnumStatusJob.Closed:
                        Source = new BitmapImage(new Uri("/OTK;component/Resource/закрыт.png", UriKind.Relative));
                        break;

                    case EnumStatusJob.ReqConfirm:
                        Source = new BitmapImage(new Uri("/OTK;component/Resource/проверка.png", UriKind.Relative));
                        break;

                    case EnumStatusJob.InWork:
                        Source = new BitmapImage(new Uri("/OTK;component/Resource/рассмотр.png", UriKind.Relative));
                        break;

                    case EnumStatusJob.Complete:
                        Source = new BitmapImage(new Uri("/OTK;component/Resource/выполнен.png", UriKind.Relative));
                        break;

                    default:
                        Source = null;
                        break;
                }



            }

            return Source;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
