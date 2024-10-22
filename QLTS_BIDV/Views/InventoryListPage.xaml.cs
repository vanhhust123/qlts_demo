using Newtonsoft.Json;
using QLTS_BIDV.Helper.Extensions;
using QLTS_BIDV.Helper.Validators;
using QLTS_BIDV.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
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
    public partial class InventoryListPage : ContentPage
    {
        public InventoryListPage(bool isDetail = false)
        {
            InitializeComponent();
            this.BindingContext = new InventoryListPageViewModel(this, isDetail);
        }

        protected override async void OnAppearing()
        {
            var context = this.BindingContext as InventoryListPageViewModel;
            MainThread.BeginInvokeOnMainThread(() =>
            {
                context.IntializeData();
            });
            base.OnAppearing();
        }
    }

    public class InventoryListPageViewModel : BaseViewModel
    {
        public ValidatableObject<bool> IsDetail { set; get; } = new ValidatableObject<bool> { Value = false };
        public InventoryListPage InventoryListPage { set; get; }
        public InventoryListPageViewModel(InventoryListPage InventoryListPage, bool isDetail = false)
        {
            this.InventoryListPage = InventoryListPage;
            this.SearchCommandInitialize();
            this.ActionCommandInitialize();
            this.OpenScanQRPageInitialize();
            this.IsDetail.Value = isDetail;
        }

        // Danh sách đợt kiểm kê
        public ObservableCollection<Inventory> inventories { get; set; } = new ObservableCollection<Inventory>();
        public ObservableCollection<Inventory> Inventories
        {
            get => this.inventories; set
            {
                this.inventories = value;
                this.OnPropertyChanged();
            }
        }


        // Tìm kiếm 
        public ObservableCollection<string> periodSearch { set; get; } = new ObservableCollection<string>();
        public ObservableCollection<string> PeriodSearch
        {
            get => this.periodSearch; set
            {
                this.periodSearch = value;
                this.OnPropertyChanged();
            }
        }

        public ValidatableObject<string> PeriodSelected { set; get; } = new ValidatableObject<string>() { Value = "" };


        // Tìm kiếm 
        public ObservableCollection<string> statusSearch { set; get; } = new ObservableCollection<string>();
        public ObservableCollection<string> StatusSearch
        {
            get => this.statusSearch; set
            {
                this.statusSearch = value;
                this.OnPropertyChanged();
            }
        }

        public ValidatableObject<string> StatusSelected { set; get; } = new ValidatableObject<string>() { Value = "" };



        public Command SearchCommand { set; get; }
        public void SearchCommandInitialize()
        {
            this.SearchCommand = new Command(async () =>
            {
                // Khởi tạo dữ liệu
                //this.IntializeData();
                this.IsBusy.Value = true;
                await Task.Delay(1000);

                if (this.StatusSelected.Value is null) this.StatusSelected.Value = "";
                if (this.PeriodSelected.Value is null) this.PeriodSelected.Value = "";

                var values = StaticConsts.GetInventories().Where(x =>
                    (x.Period == this.PeriodSelected.Value || string.IsNullOrEmpty(this.PeriodSelected.Value)) &&
                    (x.Status.GetAttribute<DisplayAttribute>().Name == this.StatusSelected.Value || string.IsNullOrEmpty(this.StatusSelected.Value))

                    );
                this.Inventories.Clear();
                values.ToList().ForEach(i =>
                {
                    this.Inventories.Add(i);
                });
                this.IsBusy.Value = false;
            });
        }

        public void IntializeData()
        {
            this.Inventories.Clear();
            StaticConsts.GetInventories().OrderBy(x => x.Id).ToList().ForEach(i =>
            {
                this.Inventories.Add(i);
            });

            this.PeriodSearch.Clear(); ;
            StaticConsts.GetInventories().Distinct().ToList().Select(x => x.Period).ToList().ForEach(p =>
            {
                this.PeriodSearch.Add(p);
            });
            this.PeriodSearch.Add("");

            this.StatusSearch.Clear();
            StaticConsts.GetInventories().Select(x => x.Status).Distinct().ToList().ForEach(s =>
            {
                this.StatusSearch.Add(s.GetAttribute<DisplayAttribute>().Name);
            });
            this.StatusSearch.Add("");
        }





        // Mở màn detail hoặc màn quét mã
        public ValidatableObject<string> InventoryCode { set; get; } = new ValidatableObject<string>()
        {
            Value = ""
        };
        public Command ActionCommand { set; get; }
        public void ActionCommandInitialize()
        {
            this.ActionCommand = new Command(async (obj) =>
            {
                var inventoryObject = obj as Inventory;
                this.InventoryCode.Value = inventoryObject.Code;
                this.IsBusy.Value = true;
                if (this.IsDetail.Value)
                {
                    await this.InventoryListPage.Navigation.PushAsync(new InventoryDetailPage(inventoryObject.Code));
                }
                else
                    this.OpenScanQRPageCommand.Execute(obj);



                this.IsBusy.Value = false;
            });
        }



        // QR Code ================================================================================================
        // QR Code ================================================================================================
        public Command OpenScanQRPageCommand { set; get; }
        public ValidatableObject<bool> IsProcessing { set; get; } = new ValidatableObject<bool>() { Value = true };
        public ZXingScannerView zXingScanner { set; get; }
        public void OpenScanQRPageInitialize()
        {
            this.OpenScanQRPageCommand = new Command(async (obj) =>
            {
                Inventory inventory = obj as Inventory;
                this.IsBusy.Value = true;
                await Task.Delay(1000);
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
                await this.InventoryListPage.Navigation.PushAsync(page);
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
                    if (this.IsProcessing.Value)
                        try
                        {
                            Asset parse = JsonConvert.DeserializeObject<Asset>(Encoding.UTF8.GetString(Convert.FromBase64String(result.Text)));
                            this.IsProcessing.Value = false;
                            await this.InventoryListPage.Navigation.PushAsync(new ScanResultPage(parse, this, this.InventoryCode.Value));
                            Vibration.Vibrate();
                        }
                        catch
                        {
                        }
                }
                catch (Exception e)
                {
                }
            });
        }

    }
}