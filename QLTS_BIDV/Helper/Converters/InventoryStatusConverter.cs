using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using QLTS_BIDV.Models;

namespace QLTS_BIDV.Helper.Converters
{
    public class InventoryStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((InventoryStatus)value == InventoryStatus.DaThucHien)
                return Color.FromHex("#006969"); 
            if ((InventoryStatus)value == InventoryStatus.ChuaThucHien)
                return Color.FromHex("#0000FF"); 
            if ((InventoryStatus)value == InventoryStatus.DangThucHien)
                return Color.FromHex("#f56816"); 
            return Color.Yellow; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //throw new NotImplementedException();
            return value; 
        }
    }


    public class AssetInventoryStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((AssetInventoryStatus)value == AssetInventoryStatus.DaKiemKe)
                return Color.Orange;
            if ((AssetInventoryStatus)value == AssetInventoryStatus.ChuaKiemKe)
                return Color.Blue;
           
            return Color.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //throw new NotImplementedException();
            return value;
        }
    }
}
