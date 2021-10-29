using Newtonsoft.Json;
using Pethouse.Models;
using System;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pethouse.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditVaccinePage : ContentPage
    {
        Vaccines vacOld = new Vaccines();


        public EditVaccinePage(int id, string vacname, DateTime? vacdate, DateTime? vacexpdate)
        {
            InitializeComponent();
            vacOld.VacId = id;
            vacOld.Vacname = vacname;
            vacOld.VacDate = vacdate;
            vacOld.VacExpDate = vacexpdate;
            LoadDetails(id, vacname, vacdate, vacexpdate);
        }

        private void LoadDetails(int id, string vacname, DateTime? vacdate, DateTime? vacexpdate)
        {
            //Initialize and setup httpclient and base address
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://pethouse.azurewebsites.net/");
            //Load vaccines
            Vaccines edit = new Vaccines()
            {
                VacId = id,
                Vacname = vacname,
                VacDate = vacdate,
                VacExpDate = vacexpdate

            };
            editVacStack.BindingContext = edit;
            //string jsonString = JsonConvert.SerializeObject(edit);
            //string json = await client.GetStringAsync("/api/vaccines/edit" + id);
            //IEnumerable<Vaccines> vacs = JsonConvert.DeserializeObject<Vaccines[]>(json);
            //ObservableCollection<Vaccines> dataa = new ObservableCollection<Vaccines>(vacs);

        }

        private async void saveButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                int id = vacOld.VacId;
                Vaccines vacNew = new Vaccines
                {
                    Vacname = nameEntry.Text,
                    VacDate = vacdatePicker.Date,
                    VacExpDate = vacexpdatePicker.Date
                };
                //Datan serialisointi ja vienti API:lle
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://pethouse.azurewebsites.net/");
                string newObject = JsonConvert.SerializeObject(vacNew);
                StringContent newContent = new StringContent(newObject, Encoding.UTF8, "application/json");


                // Lähetetään serialisoitu objekti back-endiin Put pyyntönä
                HttpResponseMessage message = await client.PutAsync("/api/vaccines/" + id, newContent);

                // Otetaan vastaan palvelimen vastaus
                string reply = await message.Content.ReadAsStringAsync();

                //Asetetaan vastaus serialisoituna success muuttujaan
                bool success = JsonConvert.DeserializeObject<bool>(reply);

                if (success)  // Näytetään ehdollisesti alert viesti
                {
                    await DisplayAlert("Pet with ID:" + vacNew.Vacname.ToString() + " - Edit", "Success", "Done"); // (otsikko, teksti, kuittausnapin teksti)
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

        private async void deleteButton_Clicked(object sender, EventArgs e)
        {
            int id = vacOld.VacId;
            bool response = await DisplayAlert("Delete " + vacOld.Vacname + "?", "Really Delete?", "Yes", "No");

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://pethouse.azurewebsites.net/");
            try
            {
                if (response)
                {
                    HttpResponseMessage message = await client.DeleteAsync("/api/vaccines/" + id);
                    if (message.IsSuccessStatusCode)
                    {
                        await DisplayAlert("OK", "The vaccine " + vacOld.Vacname + " was deleted.", "Ok");
                        _ = Navigation.PopModalAsync();
                    }
                }
                else
                {
                    await DisplayAlert("Cancelled", "The vaccine " + vacOld.Vacname + " was not deleted.", "Ok");
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
