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
    public partial class AddMediacationsPage : ContentPage
    {
        private int id = 0;
        private string debugSection;

        public AddMediacationsPage(int petId)
        {
            id = petId;
            InitializeComponent();
        }

        private async void addButton_Clicked(object sender, EventArgs e)
        {
            if (nameEntry.Text != null)
            {
                Medications meds = new Medications()
                {
                    PetId = id,
                    Medname = nameEntry.Text,
                    MedDate = medDatePicker.Date,
                    MedExpDate = medExpDatePicker.Date
                };
                try
                {

                    //Datan serialisointi ja vienti API:lle
                    debugSection = " Serialize";
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("https://pethouse.azurewebsites.net/");
                    string input = JsonConvert.SerializeObject(meds);
                    StringContent content = new StringContent(input, Encoding.UTF8, "application/json");

                    // Lähetetään serialisoitu objekti back-endiin Post pyyntönä
                    debugSection = " Send Serialized object";
                    HttpResponseMessage message = await client.PostAsync("/api/medications/", content);

                    // Otetaan vastaan palvelimen vastaus
                    debugSection = " wait and read reply" + content;
                    string reply = await message.Content.ReadAsStringAsync();

                    //Asetetaan vastaus serialisoituna success muuttujaan
                    debugSection = " take reply and set to bool " + reply;
                    bool success = JsonConvert.DeserializeObject<bool>(reply);


                    if (success)  // Näytetään ehdollisesti alert viesti
                    {

                        await DisplayAlert("New medication added", "Success", "Done"); // (otsikko, teksti, kuittausnapin teksti)
                        _ = Navigation.PopModalAsync();

                    }
                    else
                    {
                        await DisplayAlert("Error", "Vaccine could not be added", "Close");
                    }
                }
                catch (Exception ex) // Otetaan poikkeus ex muuttujaan ja sijoitetaan errorMessageen
                {
                    nameEntry.Text = debugSection;
                    string errorMessage1 = ex.GetType().Name; // Poikkeuksen customoitu selvittäminen ja...
                    string errorMessage2 = ex.Message;
                    await DisplayAlert(errorMessage1, errorMessage2, "Error");
                }

            }
            else
            {
                await DisplayAlert("Oopsie!", "You left some of the fields empty. Please fill in all the fields and try again.", "Ok");
            }
            
        }
    }
}