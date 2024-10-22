using QLTS_BIDV.Helper.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QLTS_BIDV.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel(this);
        }
    }

    public class LoginViewModel : BaseViewModel
    {
        public LoginPage LoginPage { get; set; }
        public ValidatableObject<string> UserName { set; get; } = new ValidatableObject<string>();
        public ValidatableObject<string> Password { set; get; } = new ValidatableObject<string>();

        public Command LoginCommand { set; get; }
        public LoginViewModel(LoginPage LoginPage)
        {
            this.LoginPage = LoginPage;
            this.LoginCommandInitialize();
        }


        public void LoginCommandInitialize()
        {
            this.LoginCommand = new Command(async () =>
            {
                this.IsBusy.Value = true;
                if (this.UserName.Value == null) this.UserName.Value = ""; 
                if (this.Password.Value == null) this.Password.Value = ""; 
                if (this.UserName.Value.ToLower() != "admin" && this.Password.Value.ToLower() != "admin")
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Account Invalid", "Account Invalid", "Ok");
                    this.IsBusy.Value = false;
                    return;
                }

                Xamarin.Forms.Application.Current.MainPage = new NavigationPage(
                    //new ListPage()
                    new MenuPage()
                    )
                {
                };
                this.IsBusy.Value = false;
            });
        }





    }
}