using Newtonsoft.Json;
using Pethouse.Models;
using System;
using System.Diagnostics;
using System.Net.Http;
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
            LoadDetails(null, null);
        }
        /// <summary>
        /// public void LoadDetails(object, EventArgs)
        /// Mystisesti tämä hajosi kun lemmikin lisäys toisella sivulla alkoi toimia. Tutki ja korjaa....
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            //Load Vaccines
            string jsonVaccines = await client.GetStringAsync("/api/Vaccines/" + petId);
            Vaccines vaccines = JsonConvert.DeserializeObject<Vaccines>(jsonVaccines);



            //Load Grooming
            string jsonGrooming = await client.GetStringAsync("/api/Grooming/" + petId);
            Grooming grooming = JsonConvert.DeserializeObject<Grooming>(jsonGrooming);



            //Load Medicines
            string jsonMedications = await client.GetStringAsync("/api/Medications/" + petId);
            Medications medications = JsonConvert.DeserializeObject<Medications>(jsonMedications);



            //Set parameters to view
            try
            {
                //Setting basic info
                petTable.BindingContext = pets;
                nameLabel.Text = pets.Petname;

                if (pets.RaceId == 1)
                {
                    petRace.Detail = "Dog";
                }
                else if (pets.RaceId == 2)
                {
                    petRace.Detail = "Cat";
                }
                else
                {
                    petRace.Detail = "Unknown";
                }
                petBreed.Detail = breed.Breedname.ToString();

                //Setting Treatment info
                //Vaccines
                if (vaccines != null)
                {
                    txtCellVacName.Detail = vaccines.Vacname;
                }
                else
                {
                    txtCellVacName.IsEnabled = false;
                    //txtCellVacName.Detail = "Unknown";
                }
                if (vaccines != null)
                {
                    txtCellVacDate.Detail = vaccines.VacDate.ToString();
                }
                else
                {
                    txtCellVacDate.IsEnabled = false;
                }
                //Medications
                if (medications != null)
                {
                    txtCellMedName.Detail = medications.Medname.ToString();
                }
                else
                {
                    txtCellMedName.IsEnabled = false;
                }
                if (medications != null)
                {
                    txtCellMedDate.Detail = medications.MedDate.ToString();
                }
                else
                {
                    txtCellMedDate.IsEnabled = false;
                }
                //Treatments
                if (grooming != null)
                {
                    txtCellGroomName.Detail = grooming.Groomname.ToString();
                }
                else
                {
                    txtCellGroomName.IsEnabled = false;
                }
                if (grooming != null)
                {
                    txtCellGroomDate.Detail = grooming.GroomDate.ToString();
                }
                else
                {
                    txtCellGroomDate.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine("Virhe " + ex);
            }
        }

        private async void Delete_Button_Clicked(object sender, EventArgs e)
        {
            bool response = await DisplayAlert("Delete " + petId + "?", "Really Delete?", "Yes", "No");
            Console.WriteLine("Save data: " + response);
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("https://pethouse.azurewebsites.net/")
            };
            //Delete pet
            try
            {
                if (response)
                {
                    HttpResponseMessage message = await client.DeleteAsync("/api/pets/" + petId);
                    if (message.IsSuccessStatusCode)
                    {
                        await DisplayAlert("OK", "The pet with id " + petId + " was deleted.", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Cancelled", "The pet with id " + petId + " was not deleted.", "Ok");
                }

            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
                await DisplayAlert("Error", error, "Ok");
            }
        }

        private void Edit_Button_Clicked(object sender, EventArgs e)
        {
            _ = Navigation.PushModalAsync(new EditPetPage(petId));
        }

        private async void addTreatmentButton_Clicked(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("What type of treatment would you like to add?", "Cancel", null, "Vaccine", "Medication", "Treatment");
            if (action == "Vaccine")
            {
                _ = Navigation.PushModalAsync(new VaccinesPage(petId));

            }else if (action == "Medication")
            {
                _ = Navigation.PushModalAsync(new MediacationsPage()); //add pet id as parameter
            }
            else if (action == "Treatment")
            {
                _ = Navigation.PushModalAsync(new GrooomingPage());  //add pet id as parameter
            }
            
        }
    }
}
/*!!!TODO:
 * 
 * OLD:
 * Näkyy vähän hölmösti tiedot tällä hetkellä.
 * Voisi yrittää saada niitä näkymään vähän järkevämmin
 * 
 * Pitää miettiä mikä on järkevin tapa, että käyttäjä lähtee muokkaamaan ja syöttämään uusia tietoja
 * Onko se painamalla rokote kohtaa, vai lisätäänkö erillinen nappi tätä varten´?
 * 
 * 27.8.2021
 * Delete nappi toimii. Seuraavaksi täytyy muuttaa toimintoa niin, että deletoimisen jälkeen palataan
 * main view sivulle ja päivitetäään lista. Listan päivitys täytyy lisätä myös lemmikin lisäyksen ja muokkauksen jälkeen.
 * Muita tekemättömiä asioita:
 * - Login ikkunan piilotus kirjautumisen jälkeen
 * - Uuden käyttäjän luominen
 * - Lemmikin editoiminen
 * - Rokotteiden, lääkkeiden ja hoitojen lisäys, muokkaus ja poisto
 * - Muistutustoiminto
 * - Valokuvatoiminto tai vastaava
 * 
 *
 * 
 */