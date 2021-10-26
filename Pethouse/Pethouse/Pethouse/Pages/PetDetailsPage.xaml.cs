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
            OnAppearing();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

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
                    try
                    {
                        vacSection.BindingContext = vaccines;

                    }
                    catch (Exception ex)
                    {

                        await DisplayAlert("Error", ex.ToString() , "ok");
                    }
                    //txtCellVacName.Detail = vaccines.Vacname.ToString();
                }
                else if (vaccines != null)
                {
                    txtCellVacName.Detail = vaccines.Vacname;
                    txtCellVacDate.Detail = vaccines.VacDate.ToString();
                }
                else
                {
                    txtCellVacName.IsEnabled = false;
                    txtCellVacDate.IsEnabled = false;
                    //txtCellVacName.Detail = "Unknown";
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
            

            //Delete pets vacs if any
            string jsonRequestVacCount = await client.GetStringAsync("/api/vaccines/count/" + petId);
            int vacCount = JsonConvert.DeserializeObject<int>(jsonRequestVacCount);
            if (vacCount > 0)
            {
                //await DisplayAlert("Pet " + petId, "has " + vacCount.ToString() + " Vaccines.", "Ok");
                try
                {
                    for (int i = 0; i < vacCount; i++)
                    {
                        try
                        {
                            HttpResponseMessage message = await client.DeleteAsync("/api/vaccines/list/" + petId);

 }
                        catch (Exception ex)
                        {
                            string error = ex.Message.ToString();
                            await DisplayAlert("Error", error, "Ok");
                        }
                    }

                }
                catch (Exception ex)
                {
                    string error = ex.Message.ToString();
                    await DisplayAlert("Error", error, "Ok");
                }

            }

            //Delete pets meds if any
            string jsonRequestMedCount = await client.GetStringAsync("/api/medications/count/" + petId);
            int medCount = JsonConvert.DeserializeObject<int>(jsonRequestMedCount);
            if (medCount > 0)
            {
                //await DisplayAlert("Pet " + petId, "has " + medCount.ToString() + " Medications.", "Ok");
                try
                {
                    for (int i = 0; i < medCount; i++)
                    {
                        try
                        {
                            HttpResponseMessage message = await client.DeleteAsync("/api/medications/list/" + petId);

                        }
                        catch (Exception ex)
                        {
                            string error = ex.Message.ToString();
                            await DisplayAlert("Error", error, "Ok");
                        }
                    }

                }
                catch (Exception ex)
                {
                    string error = ex.Message.ToString();
                    await DisplayAlert("Error", error, "Ok");
                }

            }
            //Delete pets treatments if any
            string jsonRequestGroomCount = await client.GetStringAsync("/api/grooming/count/" + petId);
            int groomCount = JsonConvert.DeserializeObject<int>(jsonRequestGroomCount);
            if (groomCount > 0)
            {
                //await DisplayAlert("Pet " + petId, "has " + medCount.ToString() + " Treatmens.", "Ok");
                try
                {
                    for (int i = 0; i < groomCount; i++)
                    {
                        try
                        {
                            HttpResponseMessage message = await client.DeleteAsync("/api/grooming/list/" + petId);

                        }
                        catch (Exception ex)
                        {
                            string error = ex.Message.ToString();
                            await DisplayAlert("Error", error, "Ok");
                        }
                    }

                }
                catch (Exception ex)
                {
                    string error = ex.Message.ToString();
                    await DisplayAlert("Error", error, "Ok");
                }

            }

            //Delete pet
            try
            {
                if (response)
                {
                    HttpResponseMessage message = await client.DeleteAsync("/api/pets/" + petId);
                    if (message.IsSuccessStatusCode)
                    {
                        await DisplayAlert("OK", "The pet with id " + petId + " was deleted.", "Ok");
                        _ = Navigation.PopModalAsync();
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
                _ = Navigation.PushModalAsync(new AddVaccinesPage(petId));

            }else if (action == "Medication")
            {
                _ = Navigation.PushModalAsync(new AddMediacationsPage(petId)); //add pet id as parameter
            }
            else if (action == "Treatment")
            {
                _ = Navigation.PushModalAsync(new AddGrooomingPage(petId));  //add pet id as parameter
            }
            
        }
        /*Todo:
         * GetMedicationsPage
         * GetGroomingPage
         * vacname_tapped => open edit vaccination
         * medname_tapped => open edit medication
         * groomname_tapped => open edit grooming
         */
        private void vaccinesButton_Clicked(object sender, EventArgs e)
        {
            _ = Navigation.PushModalAsync(new GetVaccinesPage(petId));
        }

        private void medicationsButton_Clicked(object sender, EventArgs e)
        {
            _ = Navigation.PushModalAsync(new GetMedicationsPage(petId));
        }

        private void groomingButton_Clicked(object sender, EventArgs e)
        {
            _ = Navigation.PushModalAsync(new GetGroomingPage(petId));
        }

        private void txtCellVacName_Tapped(object sender, EventArgs e)
        {

        }

        private void txtCellMedName_Tapped(object sender, EventArgs e)
        {

        }

        private void txtCellGroomName_Tapped(object sender, EventArgs e)
        {

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
 * 14.10.2021
 * - Lääkkeiden ja hoitojen lisäys toimii.
 * 
 * 
 *
 * 
 */