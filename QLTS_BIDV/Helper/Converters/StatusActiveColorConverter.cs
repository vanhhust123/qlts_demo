//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Text;
//using Xamarin.Essentials;
//using Xamarin.Forms;

//namespace QLTS_BIDV.Helper.Converters
//{
//    public class StatusActiveColorConverterNew : IValueConverter
//    {
//        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            var currentStep = System.Convert.ToInt32(value);
//            var step = System.Convert.ToInt32(parameter);
//            if (currentStep == step)
//            {
//                return Color.White;
//            }
//            else
//            {
//                //if (!string.IsNullOrEmpty(Preferences.Get(StaticConsts.NewTicketColor, "")))
//                //{
//                //    return Color.FromHex(Preferences.Get(StaticConsts.NewTicketColor, ""));
//                //}else{ return Color.Yellow; }
//                return Color.White;
//            }
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            return value;
//        }
//    }

//    public class StatusActiveColorConverterPrepare : IValueConverter
//    {
//        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            var currentStep = System.Convert.ToInt32(value);
//            var step = System.Convert.ToInt32(parameter);
//            if (currentStep == step)
//            {
//                return Color.White;
//            }
//            else
//            {
//                if (!string.IsNullOrEmpty(Preferences.Get(StaticConsts.PreparedTicketColor, "")))
//                {
//                    return Color.FromHex(Preferences.Get(StaticConsts.PreparedTicketColor, ""));
//                }
//                else { return Color.Yellow; }
//            }
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            return value;
//        }
//    }

//    public class StatusActiveColorConverterComplete : IValueConverter
//    {
//        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            var currentStep = System.Convert.ToInt32(value);
//            var step = System.Convert.ToInt32(parameter);
//            if (currentStep == step)
//            {
//                return Color.White;
//            }
//            else
//            {
//                if (!string.IsNullOrEmpty(Preferences.Get(StaticConsts.CompletedTicketColor, "")))
//                {
//                    return Color.FromHex(Preferences.Get(StaticConsts.CompletedTicketColor, ""));
//                }
//                else { return Color.Yellow; }
//            }
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            return value;
//        }
//    }
//}
