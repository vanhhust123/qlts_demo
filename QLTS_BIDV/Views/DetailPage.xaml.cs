using Newtonsoft.Json;
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
    public partial class DetailPage : ContentPage
    {
        public Asset Asset { get; set; } = new Asset();
        public DetailPage(Asset asset = null, InventoryListPageViewModel model = null)
        {
            StackLayout a = new StackLayout();
            

            InitializeComponent();
            this.BindingContext = new DetailPageViewModel(this, asset, model);
            this.Asset = asset;
        }
        protected override void OnAppearing()
        {
            var context = this.BindingContext as DetailPageViewModel;
            context.Asset = this.Asset;
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            var context = this.BindingContext as DetailPageViewModel;
            base.OnDisappearing(); 
        }
    }

    public class DetailPageViewModel : BaseViewModel
    {
        public DetailPage DetailPage { get; set; }
        public InventoryListPageViewModel InventoryListPageViewModel { set; get; }
        public DetailPageViewModel(DetailPage DetailPage, Asset asset = null, InventoryListPageViewModel model = null)
        {
            this.DetailPage = DetailPage;
            this.Asset = asset;
            this.InventoryListPageViewModel = model;
            this.OpenQRPageCommandInitialize();
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
                await this.DetailPage.Navigation.PushAsync(new QRPage(Convert.ToBase64String(Encoding.UTF8.GetBytes(
                    JsonConvert.SerializeObject(this.Asset)
                    ))));
            });
        }
    }
}