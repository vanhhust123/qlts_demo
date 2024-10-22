using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QLTS_BIDV.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanQRPage : ContentPage
    {
        public ScanQRPage()
        {
            InitializeComponent();
        }

        private void scannerView_OnScanResult(ZXing.Result result)
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    this.DisplayAlert("Result", result.Text, "OK").GetAwaiter().GetResult();
                });
                //this.scannerView.IsScanning = false;
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                     this.DisplayAlert("error", ex.Message, "OK");
                });
            }

        }
    }
}