﻿
using Newtonsoft.Json;
using System;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Pethouse.Models;
using System.Collections.Generic;
using Xamarin.Forms.Internals;
using System.Text;

namespace Pethouse.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPetPage : ContentPage
    /*
    General notes for solution:
    Prequisites

    Firstly to create a get function that queries the database for all the avaiable breeds, so that they can be show on the selection menu.
    It should propably be of type List<Breeds>
    UserId = static(should allready exist, see login)
    Petname
    Birthdate
    Photo
    Race_id -  Dropdown box, either cat or a dog
    Breed_id - Based on race selection show all breeds for that race
    */
    {
        private List<Races> races = new List<Races>();
        private List<Breeds> breedsList = new List<Breeds>();
        public AddPetPage()
        {
            InitializeComponent();
            LoadRaces();





        }
        public async void LoadRaces()
        {
            
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://pethouse.azurewebsites.net/");
            string json = await client.GetStringAsync("/api/races/");
            races = JsonConvert.DeserializeObject<List<Races>>(json);
            
            List<string> raceList = new List<string>();
            foreach(Races race in races)
            {
                raceList.Add(race.Racename);
            }
            racePicker.ItemsSource = raceList;
        }
        public async void LoadBreeds(object sender, EventArgs e)
        {
            
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://pethouse.azurewebsites.net/");
            string json = await client.GetStringAsync("/api/breeds/");
            breedsList = JsonConvert.DeserializeObject<List<Breeds>>(json);
      
            List<String> breed = new List<String>();
            foreach(Breeds breeds in breedsList)
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
        /*
         *--------------------------------------------------------------------------
         * 25.08.2021-Lisätty toiminto joka hakee Pickkereiden indeksin perusteella
         * BreedId ja RaceId tiedon. Lisätty Debug tekstikenttä. Lisätty ADD nappi.
         * Lisätty Rodun hakutoiminto.
         * -------------------------------------------------------------------------
         * TODO:
         * -------------------------------------------------------------------------
         * Seuraavaksi selvitys, miten tiedot viedään kantaan, nyt kun kaikki tar-
         * vittavat tiedot saadaan poimittua käyttöliittymästä. Pitää myös selvit-
         * tää mitä tehdään valokuva toiminnon kanssa.
         * -------------------------------------------------------------------------
         */
        public async void AddPet(object sender, EventArgs e)
        {
            int breedIdmem = 0;
            int raceIdmem = 0;
            raceIdmem = races[racePicker.SelectedIndex].RaceId;
            breedIdmem = breedsList[breedPicker.SelectedIndex].BreedId;
            
            debugEntry.Text = LoginInfo.UserId.ToString()
                + nameEntry.Text + bdatePicker.Date.ToString() +
                raceIdmem.ToString() + breedIdmem.ToString();


            Pets pets = new Pets()
            {
                UserId = LoginInfo.UserId,
                Petname = nameEntry.Text,
                Birthdate = bdatePicker.Date,
                //Photo = photoButton.Text,
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
                HttpResponseMessage message = await client.PostAsync("/api/pets/user", content);

                // Otetaan vastaan palvelimen vastaus
                string reply = await message.Content.ReadAsStringAsync();

                //Asetetaan vastaus serialisoituna success muuttujaan
                bool success = JsonConvert.DeserializeObject<bool>(reply);
                

                if (success)  // Näytetään ehdollisesti alert viesti
                {

                    await DisplayAlert("New pet added", "Success", "Done"); // (otsikko, teksti, kuittausnapin teksti)


                }
                else
                {
                    await DisplayAlert("Työn lopetus", "Työtä ei voitu lopettaa koska sitä ei ole aloitettu.", "Sulje"); // Muutettu 4.5.
                }
            }
            catch (Exception ex) // Otetaan poikkeus ex muuttujaan ja sijoitetaan errorMessageen
            {

                string errorMessage1 = ex.GetType().Name; // Poikkeuksen customoitu selvittäminen ja...
                string errorMessage2 = ex.Message;
                debugEntry.Text = errorMessage1; // ..näyttäminen list viewissä
                debugEntry2.Text = errorMessage2;
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
    }
}
