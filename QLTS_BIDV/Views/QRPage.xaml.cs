using QLTS_BIDV.Helper.Validators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace QLTS_BIDV.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QRPage : ContentPage
    {
        public QRPage(string assetJson = "")
        {
            InitializeComponent();
            this.BindingContext = new QRPageViewModel(this, assetJson);
            //// Tạo một view để hiển thị QR Code
            //var barcode = new ZXingBarcodeImageView
            //{
            //    HorizontalOptions = LayoutOptions.FillAndExpand,
            //    VerticalOptions = LayoutOptions.FillAndExpand,
            //    BarcodeFormat = ZXing.BarcodeFormat.QR_CODE,
            //    BarcodeOptions = new ZXing.Common.EncodingOptions
            //    {
            //        Width = 300,
            //        Height = 300,
            //        Margin = 10, PureBarcode = true
            //    }
            //};
            //// Thiết lập nội dung mã QR
            //barcode.BarcodeValue = Convert.ToBase64String(Encoding.UTF8.GetBytes("\"đây là tiếNGG vIỆTTTT Â Ấ Ê Ế ĐẾ ĐẾA\""));
            //// Tạo và cấu hình layout chứa mã QR
            //var stack = new StackLayout
            //{
            //    VerticalOptions = LayoutOptions.FillAndExpand,
            //    HorizontalOptions = LayoutOptions.FillAndExpand
            //};

            //stack.Children.Add(barcode);

            //// Thiết lập Content của trang là stack layout đã tạo
            //Content = stack;
        }
    }

    public class QRPageViewModel : BaseViewModel
    {
        public QRPage QRPage { get; set; }
        public string assetJson { set; get; }
        public QRPageViewModel(QRPage qRPage, string assetJson = "")
        {
            QRPage = qRPage;
            this.ChangeBarcodeValueCommandInitialize();
            this.BarcodeValue.Value = assetJson;
        }

        public ValidatableObject<string> BarcodeValue { set; get; } = new ValidatableObject<string>() { Value = "abc123" };
        public Command ChangeBarcodeValueCommand { get; set; }
        public void ChangeBarcodeValueCommandInitialize()
        {
            this.ChangeBarcodeValueCommand = new Command(() =>
            {
                this.BarcodeValue.Value = $"abc1234 def {DateTime.Now.ToString("HH:MM:ss")}";
            });
        }
    }
}