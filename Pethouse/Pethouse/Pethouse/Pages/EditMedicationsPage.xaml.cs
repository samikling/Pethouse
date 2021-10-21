using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pethouse.Models;
using Newtonsoft.Json;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pethouse.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditMedicationsPage : ContentPage
    {
        Medications medicationOld = new Medications();
        public EditMedicationsPage(int id,string medname, DateTime? meddate, DateTime? medexpdate)
        {
            InitializeComponent();
            medicationOld.MedId = id;
            medicationOld.Medname = medname;
            medicationOld.MedDate = meddate;
            medicationOld.MedExpDate = medexpdate;
            LoadDetails(id,medname,meddate,medexpdate);
        }

        private void LoadDetails(int id, string medname, DateTime? meddate, DateTime? medexpdate)
        {
            //Initialize and setup httpclient and base address
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://pethouse.azurewebsites.net/");
            //Load vaccines
            Medications edit = new Medications()
            {
                MedId = id,
                Medname = medname,
                MedDate = meddate,
                MedExpDate = medexpdate

            };
            editMedStack.BindingContext = edit;
            //string jsonString = JsonConvert.SerializeObject(edit);
            //string json = await client.GetStringAsync("/api/vaccines/edit" + id);
            //IEnumerable<Vaccines> vacs = JsonConvert.DeserializeObject<Vaccines[]>(json);
            //ObservableCollection<Vaccines> dataa = new ObservableCollection<Vaccines>(vacs);

        }

        private async void saveButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                int id = medicationOld.MedId;
                Medications medNew = new Medications
                {
                    Medname = nameEntry.Text,
                    MedDate = meddatePicker.Date,
                    MedExpDate = medexpdatePicker.Date
                };
                //Datan serialisointi ja vienti API:lle
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://pethouse.azurewebsites.net/");
                string newObject = JsonConvert.SerializeObject(medNew);
                StringContent newContent = new StringContent(newObject, Encoding.UTF8, "application/json");


                // Lähetetään serialisoitu objekti back-endiin Put pyyntönä
                HttpResponseMessage message = await client.PutAsync("/api/medications/" + id, newContent);

                // Otetaan vastaan palvelimen vastaus
                string reply = await message.Content.ReadAsStringAsync();

                //Asetetaan vastaus serialisoituna success muuttujaan
                bool success = JsonConvert.DeserializeObject<bool>(reply);

                if (success)  // Näytetään ehdollisesti alert viesti
                {
                    await DisplayAlert("Operation " + medNew.Medname.ToString() + " - Edit", "Success", "Done"); // (otsikko, teksti, kuittausnapin teksti)
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

        private async void deleteButton_Clicked(object sender, EventArgs e)
        {
            int id = medicationOld.MedId;
            bool response = await DisplayAlert("Delete " + medicationOld.Medname + "?", "Really Delete?", "Yes", "No");

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://pethouse.azurewebsites.net/");
            try
            {
                if (response)
                {
                    HttpResponseMessage message = await client.DeleteAsync("/api/medications/" + id);
                    if (message.IsSuccessStatusCode)
                    {
                        await DisplayAlert("OK", "The medication " + medicationOld.Medname + " was deleted.", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Cancelled", "The vaccine " + medicationOld.Medname + " was not deleted.", "Ok");
                }

            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
                await DisplayAlert("Error", error, "Ok");
            }
        }
    }
}