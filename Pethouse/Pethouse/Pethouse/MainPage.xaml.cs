using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pethouse.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pethouse
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Login login = new Login();
            if (!LoginInfo.LoggedIn)
            {
                Navigation.PushModalAsync(new LoginPage());
            }

            if (OnBackButtonPressed())
            {
                Navigation.PopToRootAsync();
            }
        }
    }
}