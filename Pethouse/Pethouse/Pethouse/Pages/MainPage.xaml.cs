using Newtonsoft.Json;
using Pethouse.Models;
using Pethouse.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            //Login login = new Login();
            if (!LoginInfo.LoggedIn)
            {
                _ = Navigation.PushModalAsync(new LoginPage());
            }

            if (OnBackButtonPressed())
            {
                _ = Navigation.PopToRootAsync();
            }

            if (LoginInfo.LoggedIn)
            {
                LoadPets(LoginInfo.UserId, null);
            }
        }

        //public List<Pets> YourPets { get; set; }

        private async void LoadPets(object sender, EventArgs e)
        {
            
            //YourPets = new List<Pets>();
            //Pets pets = new Pets();
            HttpClient client = new HttpClient(); //Metodin alustus, jolla yhdistetään API:n
            client.BaseAddress = new Uri("https://pethouse.azurewebsites.net/");
            string json = await client.GetStringAsync("/api/pets/user/" + LoginInfo.UserId);

            IEnumerable<Pets> pets = JsonConvert.DeserializeObject<Pets[]>(json);
            ObservableCollection<Pets> dataa = new ObservableCollection<Pets>(pets);
            petsList.ItemsSource = dataa;

            //If petlist is empty and user has no pets --- Button to add new pets
            if (dataa.Count == 0)
            {
                petsList.IsVisible = false;
                addPetBtn.IsVisible = true;
            }
        }

        private void petsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Pets pet = (Pets)petsList.SelectedItem;
            int id = pet.PetId;
            _ = Navigation.PushModalAsync(new PetDetailsPage(id));
        }
    }
}