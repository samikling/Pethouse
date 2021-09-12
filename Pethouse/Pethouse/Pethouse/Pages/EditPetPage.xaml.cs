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
        private Pets pets = new Pets();
        public int petId;
        /// <summary>
        /// Loads Pets details by PetId
        /// Details can then be edited by the user.
        /// </summary>
        /// <param name="idParam"></param>
        /// 

        //TODO:
        // Fix a problem where, when clicking the save button, if no race or no breed have been selected
        // there will be a no index selected error. 
        // Add a refresh function for the page, after the save has been successfull

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
        /// <summary>
        /// Loads pets details
        /// </summary>
        /// <param name="sender">PetId</param>
        /// <param name="e">placeholder</param>
        private async void LoadPet(object sender, EventArgs e)
        {
            int petId = (int)sender;

            Pets pet = new Pets();
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("https://pethouse.azurewebsites.net/")
            };
            //Load pet
            string jsonPet = await client.GetStringAsync("/api/pets/" + petId);
            pets = JsonConvert.DeserializeObject<Pets>(jsonPet);
            //Load breed information
            string jsonBreed = await client.GetStringAsync("/api/Breeds/" + pets.BreedId);
            Breeds breed = JsonConvert.DeserializeObject<Breeds>(jsonBreed);
            try
            {
                nameEntry.Text = pets.Petname;
                bdatePicker.Date = (DateTime)pets.Birthdate;
                racePicker.Title = pets.RaceId == 1 ? "Dog" : "Cat";
                breedPicker.Title = breed.Breedname;
                LoadRaces();

            }
            catch (Exception ex)
            {

                await DisplayAlert(ex.GetType().Name, ex.Message, "OK");
            }
        }
        /// <summary>
        /// Loads a list of pet races
        /// </summary>
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

        }
        /// <summary>
        /// Loads a list of breeds based on RaceId
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    }

                }
                else if (racePicker.SelectedItem.Equals("Cat") || racePicker.SelectedItem.Equals(2))
                {

                    if (breeds.RaceId == 2)
                    {
                        breed.Add(breeds.Breedname);
                    }
                }
            }
            breedPicker.ItemsSource = breed;


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
        /// <summary>
        /// Sends the edited information to the API
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void Save_Button_Clicked(object sender, EventArgs e)
        {
            int breedIdmem = 0;
            int raceIdmem = 0;
            raceIdmem = races[racePicker.SelectedIndex].RaceId;
            breedIdmem = breedsList[breedPicker.SelectedIndex].BreedId;

            debugEntry.Text = LoginInfo.UserId.ToString()
                + nameEntry.Text + bdatePicker.Date.ToString() +
                raceIdmem.ToString() + breedIdmem.ToString();

            var pets = new Pets
            {
                UserId = LoginInfo.UserId,
                Petname = nameEntry.Text,
                Birthdate = bdatePicker.Date,

                RaceId = raceIdmem,
                BreedId = breedIdmem
            };

            try
            {
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
                    await DisplayAlert("Pet with ID:" + petId + " - Edit", "Success", "Done"); // (otsikko, teksti, kuittausnapin teksti)
                    MainPage mainPage = new MainPage();
                    mainPage.LoadPets(LoginInfo.UserId, null);
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
    }
}