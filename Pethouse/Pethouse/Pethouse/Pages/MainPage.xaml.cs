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
            //cheks if user is logged in
            
            //if (LoginInfo.LoggedIn)
            //{

            //    OnAppearing();
                
            //    LoadPets(LoginInfo.UserId, null); //if true, load pets

            //}


            OnBackButtonPressed();
            OnAppearing();
            //LoadPets(LoginInfo.UserId, null); //load pets, might be useless and hogging performance, delete if unnecessary




            //Initializing the refresh command
            System.Windows.Input.ICommand refreshCommand = new Command(() =>
            {
                // IsRefreshing is true
                // Refresh data here
                LoadPets(LoginInfo.UserId, null);
                petsList.IsRefreshing = false;
            });
            petsList.RefreshCommand = refreshCommand; //refresh command

        }
            protected override bool OnBackButtonPressed()
            {
                //_ = Navigation.PopToRootAsync();
                return true;
            }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //if (petsList.ItemsSource.Equals(null))
            //{
            //    LoadPets(LoginInfo.UserId, null);
            //}
            System.Windows.Input.ICommand refreshCommand = new Command(() =>
            {
                // IsRefreshing is true
                // Refresh data here
                LoadPets(LoginInfo.UserId, null);
                petsList.IsRefreshing = false;
            });
            petsList.RefreshCommand = refreshCommand;
            if (!LoginInfo.LoggedIn)
            {
                //_ = Navigation.PopAsync();
                _ = Navigation.PushAsync(new LoginPage()); //If not true, push a new login page and close the mainpage.
            }
            else
            {
                LoadPets(LoginInfo.UserId, null);
            }
        }



        public async void LoadPets(object sender, EventArgs e)
        {
            //Make sure that the user is logged in and avoid errors.
            if (LoginInfo.UserId != 0)
            {
                //Connection and query to api
                HttpClient client = new HttpClient(); //Metodin alustus, jolla yhdistetään API:n
                client.BaseAddress = new Uri("https://pethouse.azurewebsites.net/");
                string json = await client.GetStringAsync("/api/pets/user/" + LoginInfo.UserId);

                IEnumerable<Pets> pets = JsonConvert.DeserializeObject<Pets[]>(json);
                ObservableCollection<Pets> dataa = new ObservableCollection<Pets>(pets);
                petsList.ItemsSource = dataa;

                //If petlist is empty and user has no pets --- Button to add new pets
                if (dataa.Count == 0)
                {
                    petsList.IsVisible = true;
                    addPetBtn.IsVisible = true;
                }

            }
        }

        //Open pet details page
        private void petsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Pets pet = (Pets)petsList.SelectedItem;
            int id = pet.PetId;
            _ = Navigation.PushModalAsync(new PetDetailsPage(id));
        }

        //Open add pet page
        private void addPetBtn_Clicked(object sender, EventArgs e)
        {
            _ = Navigation.PushModalAsync(new AddPetPage());
        }
    }
}