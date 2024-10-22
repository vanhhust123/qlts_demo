using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace QLTS_BIDV.Helper.Converters
{
    public class StatusVisibleConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currentStep = System.Convert.ToInt32(value.ToString());
            var step = System.Convert.ToInt32(parameter);
            if (step == currentStep)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
