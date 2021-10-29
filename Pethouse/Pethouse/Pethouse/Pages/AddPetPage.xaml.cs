
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
    public partial class AddPetPage : ContentPage

    {
        public string photoString;
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
            foreach (Races race in races)
            {
                raceList.Add(race.Racename);
            }
            racePicker.ItemsSource = raceList;
        }
        public async void LoadBreeds()//object sender, EventArgs e
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


        }
        /*
         *--------------------------------------------------------------------------
         * 25.08.2021-Lisätty toiminto joka hakee Pickkereiden indeksin perusteella
         * BreedId ja RaceId tiedon. Lisätty Debug tekstikenttä. Lisätty ADD nappi.
         * Lisätty Rodun hakutoiminto.
         * 26.10.2021 Muokattu ulkoasua, poistettu turhat debug laatikot, sekä valo-
         * kuva nappi toistaiseksi.
         * -------------------------------------------------------------------------
         * TODO:
         * -------------------------------------------------------------------------
         * Pitää myös selvittää mitä tehdään valokuva toiminnon kanssa.
         * -------------------------------------------------------------------------
         */
        public async void AddPet(object sender, EventArgs e)
        {

            int breedIdmem = 0;
            int raceIdmem = 0;
            if (racePicker.SelectedIndex < 0)
            {
                await DisplayAlert("Error","Select Race and Breed","Ok");

            }
            else if (breedPicker.SelectedIndex < 0)
            {
                await DisplayAlert("Error", "Select Race and Breed", "Ok");
            }
            else
            {
                raceIdmem = races[racePicker.SelectedIndex].RaceId;
                breedIdmem = breedsList[breedPicker.SelectedIndex].BreedId;


                if (nameEntry.Text != null && racePicker.SelectedItem != null && breedPicker.SelectedItem != null)
                {
                    Pets pets = new Pets()
                    {
                        UserId = LoginInfo.UserId,
                        Petname = nameEntry.Text,
                        Birthdate = bdatePicker.Date,
                        Photo = photoString,
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
                            _ = Navigation.PopModalAsync();


                        }
                        else
                        {
                            await DisplayAlert("Error", "Error", "Error"); // Muutettu 4.5.
                        }
                    }
                    catch (Exception ex) // Otetaan poikkeus ex muuttujaan ja sijoitetaan errorMessageen
                    {

                        string errorMessage1 = ex.GetType().Name; // Poikkeuksen customoitu selvittäminen ja...
                        string errorMessage2 = ex.Message;
                        //debugEntry.Text = errorMessage1; // ..näyttäminen list viewissä
                        //debugEntry2.Text = errorMessage2;
                    }
                }
                else
                {
                    await DisplayAlert("Oopsie!", "You left some of the fields empty. Please fill in all the fields and try again.", "Ok");
                }
            }
            
            

           
        }








        private void racePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (racePicker.SelectedItem != null)
            {
                breedPicker.IsEnabled = true;
                LoadBreeds();
            }
            else
            {
                breedPicker.IsEnabled = false;
            }
        }
    }
}
