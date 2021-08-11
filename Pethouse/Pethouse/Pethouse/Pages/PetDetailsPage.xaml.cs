using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public partial class PetDetailsPage : ContentPage
    {
        Pethouse.Models.Breeds model = new Pethouse.Models.Breeds();
        int petId;
        public PetDetailsPage(int idParam)
        {
            InitializeComponent();
            petId = idParam;
            LoadDetails(null,null);
        }
        private async void LoadDetails(object sender, EventArgs e)
        {
            Pets pet = new Pets();
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("https://pethouse.azurewebsites.net/")
            };
            //Load pet
            string jsonPet = await client.GetStringAsync("/api/pets/" + petId);
            Pets pets = JsonConvert.DeserializeObject<Pets>(jsonPet);
            //Load breed information
            string jsonBreed = await client.GetStringAsync("/api/Breeds/" + pets.BreedId);
            Breeds breed = JsonConvert.DeserializeObject<Breeds>(jsonBreed);
            //TODO!!!
            //Implement to API:
            //GET Vaccines
            //GET Medicines


            //Load Vaccines
            string jsonVaccines = await client.GetStringAsync("/api/Vaccines/" + petId);
            //Vaccines vaccines = JsonConvert.DeserializeObject<Vaccines>(jsonVaccines);
            List<Vaccines> vaccines = JsonConvert.DeserializeObject<List<Vaccines>>(jsonVaccines);

            listVacs.BindingContext = vaccines;
            
            //Load Medicines
            string jsonMedications = await client.GetStringAsync("/api/Medications/" + petId);
            Medications medications = JsonConvert.DeserializeObject<Medications>(jsonMedications);
            try
            {
                petTable.BindingContext = pets;
                nameLabel.Text = pets.Petname;
                
                if (pets.RaceId == 1)
                {
                    petRace.Detail = "Dog";
                }
                else
                {
                    petRace.Detail = "Cat";
                }
                //Get breed function
                //!!!TODO!!!
                //Adding lines for testing Jira Branch functionality
                petBreed.Detail = breed.Breedname;
            }
            catch (Exception ex)
            {

                Debug.WriteLine("Virhe " + ex);
            }
        }
    }
}