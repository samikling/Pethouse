using Newtonsoft.Json;
using Pethouse.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pethouse.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class EditPetPage : ContentPage
    {
        private List<Races> races = new List<Races>();
        private List<Breeds> breedsList = new List<Breeds>();
        public int? petRace;
        public int? petBreed;
        public int petId;
        public Pets pet = new Pets();
        public string photoString;
        public EditPetPage(int idParam)
        {

            InitializeComponent();
            petId = idParam;
            LoadPet(idParam, null); //PetID
            if (OnBackButtonPressed())
            {
                _ = Navigation.PushAsync(new MainPage());
            }

        }

        private async void LoadPet(object sender, EventArgs e)
        {
            int petId = (int)sender;

            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("https://pethouse.azurewebsites.net/")
            };
            //Load pet
            string jsonPet = await client.GetStringAsync("/api/pets/" + petId);
            pet = JsonConvert.DeserializeObject<Pets>(jsonPet);
            photoString = pet.Photo;
            petRace = pet.RaceId;
            petBreed = pet.BreedId;
            //Load breed information
            string jsonBreed = await client.GetStringAsync("/api/Breeds/" + pet.BreedId);
            Breeds breed = JsonConvert.DeserializeObject<Breeds>(jsonBreed);
            try
            {
                nameEntry.Text = pet.Petname;
                bdatePicker.Date = (DateTime)pet.Birthdate;
                racePicker.Title = pet.RaceId == 1 ? "Dog" : "Cat";
                breedPicker.Title = breed.Breedname;
                LoadRaces();

            }
            catch (Exception ex)
            {

                await DisplayAlert(ex.GetType().Name, ex.Message, "OK");
            }
        }

        public async void LoadRaces()
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://pethouse.azurewebsites.net/");
            string json = await client.GetStringAsync("/api/races/");
            races = JsonConvert.DeserializeObject<List<Races>>(json);

            List<string> raceList = new List<string>();
            foreach (Races race in races)
            {
                raceList.Add(race.Racename);
            }

            racePicker.ItemsSource = raceList;
            racePicker.SelectedIndex = (int)petRace - 1;

        }

        public async void LoadBreeds(Object sender, EventArgs e)
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://pethouse.azurewebsites.net/");
            string json = await client.GetStringAsync("/api/breeds/");
            breedsList = JsonConvert.DeserializeObject<List<Breeds>>(json);

            List<String> breed = new List<String>();
            foreach (Breeds breeds in breedsList)
            {
                if (racePicker.SelectedItem.Equals("Dog") || racePicker.SelectedItem.Equals(1))
                {
                    if (breeds.RaceId == 1)
                    {
                        breed.Add(breeds.Breedname);
                        photoString = "https://th.bing.com/th/id/OIP.IRgtzHgst1qB8jRQHvK-3QHaHY?pid=ImgDet&rs=1";
                    }

                }
                else if (racePicker.SelectedItem.Equals("Cat") || racePicker.SelectedItem.Equals(2))
                {

                    if (breeds.RaceId == 2)
                    {
                        breed.Add(breeds.Breedname);
                        photoString = "https://th.bing.com/th/id/R.bbc2d15a5fab1cc7a169dcaea2fa7392?rik=%2bRQy90HTXnBo%2fQ&riu=http%3a%2f%2fgetdrawings.com" +
              "%2fimg%2fblack-cat-silhouette-template-12.jpg&ehk=d1MnyeourS%2fp1gXKYl7%2foDDj9i6a3Vx%2bcG29dTRnMY8%3d&risl=&pid=ImgRaw&r=0";

                    }
                }
            }
            breedPicker.ItemsSource = breed;
            if (racePicker.SelectedItem.Equals("Cat"))
            {
                breedPicker.SelectedIndex = (int)petBreed - 386;
            }
            else
            {
                breedPicker.SelectedIndex = (int)petBreed - 1;
            }


        }

        private void racePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (racePicker.SelectedItem != null)
            {
                breedPicker.IsEnabled = true;
                LoadBreeds(null, null);
            }
            else
            {
                breedPicker.IsEnabled = false;
            }
        }

        public async void Save_Button_Clicked(object sender, EventArgs e)
        {
            int breedIdmem;
            int raceIdmem;

            if (racePicker.SelectedIndex == -1)
            {
                raceIdmem = (int)petRace;
            }
            else
            {
                raceIdmem = races[racePicker.SelectedIndex].RaceId;
            }
            if (breedPicker.SelectedIndex == -1)
            {
                breedIdmem = (int)petBreed;
            }
            else
            {
                if (racePicker.SelectedItem.Equals("Cat"))
                {
                    breedIdmem = breedsList[breedPicker.SelectedIndex].BreedId + 385;
                }
                else
                {
                    breedIdmem = breedsList[breedPicker.SelectedIndex].BreedId;
                }
            }




            if (nameEntry.Text != null)
            {
                try
                {
                    var pets = new Pets
                    {
                        UserId = LoginInfo.UserId,
                        PetId = petId,
                        Petname = nameEntry.Text,
                        Photo = photoString,
                        RaceId = raceIdmem,
                        BreedId = breedIdmem,
                        Birthdate = bdatePicker.Date
                    };

                    //Datan serialisointi ja vienti API:lle
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("https://pethouse.azurewebsites.net/");
                    string input = JsonConvert.SerializeObject(pets);
                    StringContent content = new StringContent(input, Encoding.UTF8, "application/json");

                    // Lähetetään serialisoitu objekti back-endiin Post pyyntönä
                    HttpResponseMessage message = await client.PutAsync("/api/pets/" + petId, content);

                    // Otetaan vastaan palvelimen vastaus
                    string reply = await message.Content.ReadAsStringAsync();

                    //Asetetaan vastaus serialisoituna success muuttujaan
                    bool success = JsonConvert.DeserializeObject<bool>(reply);

                    if (success)  // Näytetään ehdollisesti alert viesti
                    {
                        await DisplayAlert("Success", "Pet with id:" + petId + " edited and changes saved.", "Ok"); // (otsikko, teksti, kuittausnapin teksti)
                        MainPage mainPage = new MainPage();
                        mainPage.LoadPets(LoginInfo.UserId, null);
                        _ = Navigation.PopModalAsync();
                    }
                    else
                    {
                        await DisplayAlert("Error", "Edit unsuccessfull", "Close"); // Muutettu 4.5.
                    }
                }
                catch (Exception ex) // Otetaan poikkeus ex muuttujaan ja sijoitetaan errorMessageen
                {
                    string errorMessage1 = "Catch: " + ex.GetType().Name; // Poikkeuksen customoitu selvittäminen ja...
                    string errorMessage2 = "Catch: " + ex.Message;
                    await DisplayAlert("Error", errorMessage1, "Close"); // ...näyttäminen popup ikkunassa
                    await DisplayAlert("Error", errorMessage2, "Close");
                }
            }
            else
            {
                await DisplayAlert("Oopsie!", "Please fill all the fields.", "Ok");
            }
            

        }
    }
}