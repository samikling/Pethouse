using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pethouse.Models;
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
        public EditPetPage(int idParam)
        {
            InitializeComponent();
            LoadPet(idParam,null); //PetID
        }

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
                //bdatePicker.Date = (DateTime)pets.Birthdate;
                //racePicker.SelectedItem = pets.RaceId == 1 ? "Dog" : "Cat";
                //breedPicker.SelectedItem = breed;
                LoadRaces();

            }
            catch (Exception ex)
            {

              await  DisplayAlert(ex.GetType().Name, ex.Message,"OK");
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
            LoadBreeds();
        }
        public async void LoadBreeds()
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
        public async void Save_Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}