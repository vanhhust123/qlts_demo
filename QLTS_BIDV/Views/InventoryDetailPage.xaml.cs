using Newtonsoft.Json;
using QLTS_BIDV.Helper.Validators;
using QLTS_BIDV.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class InventoryDetailPage : ContentPage
    {
        public InventoryDetailPage(string inventoryCode="")
        {
            InitializeComponent();
            this.BindingContext = new InventoryDetailPageViewModel(this, inventoryCode); 
        }
        protected override async void OnAppearing()
        {
            var context = this.BindingContext as InventoryDetailPageViewModel;
            context.SearchCommand.Execute(this); 
            base.OnAppearing();
            
        }
    }

    public class InventoryDetailPageViewModel : BaseViewModel
    {
        public InventoryDetailPage InventoryDetailPage { get; set; }
        public ValidatableObject<string> InventoryCode { get; set; } = new ValidatableObject<string>() { Value = "" }; 
        public InventoryDetailPageViewModel(InventoryDetailPage inventoryDetailPage, string inventoryCode = "")
        {
            this.InventoryDetailPage = inventoryDetailPage;
            this.CommandInitialize();
            this.OpenScanQRPageInitialize();

            //
            this.Departments.Add("");
            this.SearchCommandInitialize();
            this.AssetSearch.Add("");
            StaticConsts.Assets.Select(x => x.MaTs + " - " + x.TenTs).ToList().ForEach(i => this.AssetSearch.Add(i));
            this.InventoryCode.Value = inventoryCode;
        }

      

        public ObservableCollection<Asset> assets { get; set; } = new ObservableCollection<Asset>();
        public ObservableCollection<Asset> Assetss
        {
            get => this.assets; set
            {
                this.assets = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<string> assetSearch { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> AssetSearch
        {
            get => this.assetSearch;
            set
            {
                this.assetSearch = value;
                this.OnPropertyChanged();
            }
        }

        public ValidatableObject<string> AssetSelected { set; get; } = new ValidatableObject<string>() { Value = "" };

        // Tìm kiếm
        public ObservableCollection<string> departments { get; set; } = new ObservableCollection<string>(StaticConsts.Assets.Select(x => x.PhongBanTs).Distinct());
        public ObservableCollection<string> Departments
        {
            get => this.departments;
            set { this.departments = value; }
        }
        public ValidatableObject<string> DepartmentSelected { set; get; } = new ValidatableObject<string>() { Value = "" };



        public Command SearchCommand { set; get; }
        public void SearchCommandInitialize()
        {
            this.SearchCommand = new Command(async () =>
            {
                //await this.InventoryDetailPage.DisplayAlert("department selected:", this.DepartmentSelected.Value, "Ok");

                //this.Assetss = new ObservableCollection<Asset>(values);
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    this.IsBusy.Value = true;
                    await Task.Delay(1000);
                    if (this.DepartmentSelected.Value is null)
                        this.DepartmentSelected.Value = "";
                    if (this.AssetSelected.Value is null)
                        this.AssetSelected.Value = "";
                    var values = StaticConsts.GetAssetOfPeriod(this.InventoryCode.Value).Where(x =>
                        x.PhongBanTs.ToLower().Contains(this.DepartmentSelected.Value.ToLower())
                     && (this.AssetSelected.Value.Contains(x.TenTs) || this.AssetSelected.Value.Contains(x.MaTs) ||
                     string.IsNullOrEmpty(this.AssetSelected.Value)
                     )
                    ).ToList();
                    this.Assetss.Clear();
                    values.ForEach(item =>
                    {
                        this.Assetss.Add(item);
                    });
                    this.IsBusy.Value = false;
                });


            });
        }



        public Command OpenDetailPage { set; get; }
        public void CommandInitialize()
        {
            this.OpenDetailPage = new Command(async (obj) =>
            {
                Asset asset = obj as Asset;
                this.IsBusy.Value = true;
                await this.InventoryDetailPage.Navigation.PushAsync(new DetailPage(asset));
                this.IsBusy.Value = false;
            });
        }

        public Command OpenScanQRPageCommand { set; get; }

        public ZXingScannerView zXingScanner { set; get; }
        public void OpenScanQRPageInitialize()
        {
            this.OpenScanQRPageCommand = new Command(async () =>
            {
                this.IsBusy.Value = true;
                //await this.InventoryDetailPage.Navigation.PushAsync(new ScanQRPage()); 



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
                await this.InventoryDetailPage.Navigation.PushAsync(page);
                this.zXingScanner = zxing;
                this.IsBusy.Value = false;

            });
        }


        public void OnResult(ZXing.Result result)
        {
            //IBarcodeReader reader = new BarcodeReader();
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var parse = JsonConvert.DeserializeObject<Asset>(Encoding.UTF8.GetString(Convert.FromBase64String(result.Text)));
                    this.zXingScanner.IsScanning = false;
                    Vibration.Vibrate();
                    //await this.InventoryDetailPage.DisplayAlert("result", result.Text, "OK"); 
                    if (parse != null)
                    {
                        await this.InventoryDetailPage.Navigation.PushAsync(new DetailPage(parse));
                    }
                }
                catch (Exception e)
                {
                    await this.InventoryDetailPage.DisplayAlert("Error", e.Message, "OK");
                }
            });
        }
    }
}