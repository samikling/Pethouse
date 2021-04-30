using Newtonsoft.Json;
using Pethouse.Models;
using System;
using System.Net.Http;
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

            petsList.ItemsSource = new object[] { };
            LoadPets(petsList.ItemsSource, null);

        }

            private async void LoadPets(object sender, EventArgs e)
            {
                HttpClient client = new HttpClient(); //Metodin alustus, jolla yhdistetään API:n
                client.BaseAddress = new Uri("https://pethouse.azurewebsites.net/"); //Kurssilla aiemmin luodun API:n osoite Azuressa
                string json = await client.GetStringAsync("/api/pets/user/"+LoginInfo.UserId); //Lähetetään GET pyyntö API:lle
                object[] pets = JsonConvert.DeserializeObject<object[]>(json); //Tuodaan tiedot objekti listalle
                petsList.ItemsSource = pets.ToString(); //Tuodaan Liista näkymälle
            }


        }
    }
