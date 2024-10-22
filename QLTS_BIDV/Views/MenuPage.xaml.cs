using Newtonsoft.Json;
using QLTS_BIDV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace QLTS_BIDV.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();
            this.BindingContext = new MenuPageViewModel(this);
        }
    }

    public class MenuPageViewModel : BaseViewModel
    {
        public MenuPage MenuPage { get; set; }
        public MenuPageViewModel(MenuPage menuPage)
        {
            this.MenuPage = menuPage;
            this.OpenScanQRCommandInitialize();
            this.OpenListPageCommandInitialize();
            this.OpenInventoryListPageInitialize();
            this.OpenInventoryListDetailPageCommandInitialize(); 
        }

        public ZXingScannerView zXingScanner { set; get; }
        public Command OpenScanQRCommand { set; get; }
        public void OpenScanQRCommandInitialize()
        {
            this.OpenScanQRCommand = new Command(async () =>
            {
                this.IsBusy.Value = true;
                //await this.ListPage.Navigation.PushAsync(new ScanQRPage()); 



                var zxing = new ZXingScannerView
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    AutomationId = "zxingScannerView",
                    Options = new ZXing.Mobile.MobileBarcodeScanningOptions()
                    {
                        DelayBetweenAnalyzingFrames = 1,
                        UseNativeScanning = false,
                        TryHarder = true,
                        DelayBetweenContinuousScans = 0,
                        TryInverted = true

                        //DisableAutofocus = true -- Tự động làm nét
                    }

                };
                zxing.OnScanResult += this.OnResult;
                zxing.IsAnalyzing = true;
                zxing.IsScanning = true;

                var overlay = new ZXingDefaultOverlay
                {
                    ShowFlashButton = false,
                    AutomationId = "zxingDefaultOverlay"
                };
                overlay.FlashButtonClicked += (sender, e) =>
                {
                    zxing.IsTorchOn = !zxing.IsTorchOn;
                };
                var grid = new Grid
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    WidthRequest = 350,
                    HeightRequest = 350
                };

                grid.Children.Add(zxing);
                grid.Children.Add(overlay);

                var page = new ContentPage() { BackgroundColor = Color.FromHex("006969") };
                page.Content = grid;
                NavigationPage.SetHasNavigationBar(page, false);
                await this.MenuPage.Navigation.PushAsync(page);
                this.zXingScanner = zxing;
                this.IsBusy.Value = false;
            });
        }
        public Command OpenListPageCommand { set; get; }

        // Quét
        public void OpenListPageCommandInitialize()
        {
            this.OpenListPageCommand = new Command(async () =>
            {
                this.IsBusy.Value = true;
                await this.MenuPage.Navigation.PushAsync(new ListPage());
                this.IsBusy.Value = false;
            });
        }


        public Command OpenInventoryListDetailPageCommand { set; get; }

        // Quét
        public void OpenInventoryListDetailPageCommandInitialize()
        {
            this.OpenInventoryListDetailPageCommand = new Command(async () =>
            {
                this.IsBusy.Value = true;
                await this.MenuPage.Navigation.PushAsync(new InventoryListPage(true));
                this.IsBusy.Value = false;
            });
        }


        public Command OpenInventoryListPageCommand { set; get; }
        public void OpenInventoryListPageInitialize()
        {
            this.OpenInventoryListPageCommand = new Command(async () =>
            {
                this.IsBusy.Value = true;
                await this.MenuPage.Navigation.PushAsync(new InventoryListPage()); 
                this.IsBusy.Value = false;
            });
        }



        // Callback
        public void OnResult(ZXing.Result result)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var parse = JsonConvert.DeserializeObject<Asset>(Encoding.UTF8.GetString(Convert.FromBase64String(result.Text)));
                    this.zXingScanner.IsScanning = false;
                    Vibration.Vibrate();
                    //await this.ListPage.DisplayAlert("result", result.Text, "OK"); 
                    if (parse != null)
                    {
                        //await this.MenuPage.Navigation.PushAsync(new ScanResultPage(parse, this.zXingScanner));
                    }
                }
                catch (Exception e)
                {
                    //await this.MenuPage.DisplayAlert("Error", e.Message, "OK");
                }
            });
        }
    }
}