using Newtonsoft.Json;
using Pethouse.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

            if (LoginInfo.LoggedIn)
            {
                LoadPets(LoginInfo.UserId, null);
            }
        }

        public List<Pets> YourPets { get; set; }

        private async void LoadPets(object sender, EventArgs e)
        {
            Pets lemmikki = new Pets();
            YourPets = new List<Pets>();
            //Pets pets = new Pets();
            HttpClient client = new HttpClient(); //Metodin alustus, jolla yhdistetään API:n
            client.BaseAddress = new Uri("https://pethouse.azurewebsites.net/");
            string json = await client.GetStringAsync("/api/pets/"+LoginInfo.UserId);

            IEnumerable<Pets> pets = JsonConvert.DeserializeObject<Pets[]>(json);
            ObservableCollection<Pets> dataa = new ObservableCollection<Pets>(pets);
            petsList.ItemsSource = dataa;

            //If petlist is empty and user has no pets --- Button to add new pets
            if (dataa == null)
            {
                //do something
            }
            
        }

        private void petsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //New Info page --> add Meds, vacs and whatnot. --> Edit pet info
        }
    }
   
}






            //client.BaseAddress = new Uri("https://pethouse.azurewebsites.net/"); //Kurssilla aiemmin luodun API:n osoite Azuressa
            //string dogjson = await client.GetStringAsync("api/pets/user/dogs/"+LoginInfo.UserId);// +LoginInfo.UserId); //Lähetetään GET pyyntö API:lle
            //try
            //{
            //    //var pets = JsonConvert.DeserializeObject<List<Pets>>(json);
            //    string[] dogs = JsonConvert.DeserializeObject<string[]>(dogjson);
            //    dogListView.ItemsSource = dogs;
            //}
            //catch (Exception ex)
            //{

            //    Debug.WriteLine(@"ERROR{0}", ex);
            //}
            //string catjson = await client.GetStringAsync("api/pets/user/cats/"+LoginInfo.UserId);// +LoginInfo.UserId); //Lähetetään GET pyyntö API:lle
            //try
            //{
            //    //var pets = JsonConvert.DeserializeObject<List<Pets>>(json);
            //    string[] cats = JsonConvert.DeserializeObject<string[]>(catjson);
            //    catListView.ItemsSource = cats;
            //}
            //catch (Exception ex)
            //{

            //    Debug.WriteLine(@"ERROR{0}", ex);
            //}
            //string json = await client.GetStringAsync("api/pets/user/test/" + LoginInfo.UserId);// +LoginInfo.UserId); //Lähetetään GET pyyntö API:lle
            //try
            //{
            //    //var pets = JsonConvert.DeserializeObject<List<Pets>>(json);
            //    IEnumerable<Pets> pets = JsonConvert.DeserializeObject<IEnumerable<Pets>>(json);
            //    catListView.ItemsSource = pets;
            //}
            //catch (Exception ex)
            //{

            //    Debug.WriteLine(@"ERROR{0}", ex);
            //}


