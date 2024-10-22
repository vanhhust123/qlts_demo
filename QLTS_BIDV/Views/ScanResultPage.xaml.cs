using Newtonsoft.Json;
using QLTS_BIDV.Helper.Validators;
using QLTS_BIDV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QLTS_BIDV.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanResultPage : ContentPage
    {
        public Asset Asset { get; set; } = new Asset();
        public string InventoryCode { set; get; }
        public ScanResultPage(Asset asset = null, InventoryListPageViewModel model = null, string inventoryCode = "")
        {
            InitializeComponent();
            this.BindingContext = new ScanResultPageViewModel(this, asset, model, inventoryCode);
            this.Asset = asset;
            this.InventoryCode = inventoryCode;
        }
        protected override void OnAppearing()
        {
            var context = this.BindingContext as ScanResultPageViewModel;
            context.Asset = this.Asset;
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            var context = this.BindingContext as ScanResultPageViewModel;
            context.InventoryListPageViewModel.IsProcessing.Value = true;
            base.OnDisappearing();
        }
    }

    public class ScanResultPageViewModel : BaseViewModel
    {
        public ScanResultPage ScanResultPage { get; set; }
        public InventoryListPageViewModel InventoryListPageViewModel { set; get; }
        public ValidatableObject<string> InventoryCode { set; get; } = new ValidatableObject<string>() { Value = "" };
        public ScanResultPageViewModel(ScanResultPage ScanResultPage, Asset asset = null,
            InventoryListPageViewModel model = null, string inventoryCode = "")
        {
            this.ScanResultPage = ScanResultPage;
            this.Asset = asset;
            this.InventoryListPageViewModel = model;
            this.InventoryCode.Value = inventoryCode;
            this.OpenQRPageCommandInitialize();
            this.CheckCommandInitialize();
            this.GoMenuCommandInitialize();
            this.ContinueScanCommandInitialize(); 
        }

        public Asset asset { set; get; }
        public Asset Asset
        {
            get => this.asset; set
            {
                this.asset = value;
                this.OnPropertyChanged();
            }
        }

        public Command OpenQRPageCommand { set; get; }
        public void OpenQRPageCommandInitialize()
        {
            this.OpenQRPageCommand = new Command(async () =>
            {
                await this.ScanResultPage.Navigation.PushAsync(new QRPage(Convert.ToBase64String(Encoding.UTF8.GetBytes(
                    JsonConvert.SerializeObject(this.Asset)
                    ))));
            });
        }
        public Command CheckCommand { set; get; }
        public void CheckCommandInitialize()
        {
            this.CheckCommand = new Command(async () =>
            {
                this.IsBusy.Value = true;
                await Task.Delay(1000);
                StaticConsts.AddInventoryAsset(this.InventoryCode.Value, this.Asset);
                this.IsActionChoose.Value = true;
                this.IsBusy.Value = false;
            });
        }

        public ValidatableObject<bool> IsActionChoose { set; get; } = new ValidatableObject<bool> { Value = false };
        public Command ContinueScanCommand { set; get; }
        public void ContinueScanCommandInitialize()
        {
            this.ContinueScanCommand = new Command(async () =>
            {
                this.IsBusy.Value = true; 
                await Task.Delay(1000);
                this.InventoryListPageViewModel.IsProcessing.Value = true;
                await this.ScanResultPage.Navigation.PopAsync();
            });
        }
        public Command GoMenuCommand { set; get; }
        public void GoMenuCommandInitialize()
        {
            this.GoMenuCommand = new Command(async () =>
            {
                this.IsBusy.Value = true;
                await Task.Delay(1000);
                await this.ScanResultPage.Navigation.PopToRootAsync(); 
            });
        }
    }
}