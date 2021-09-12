using Newtonsoft.Json;
using Pethouse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pethouse.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VaccinesPage : ContentPage
    {   int id = 0;
        public VaccinesPage(int petId)
        {
            id = petId;
            InitializeComponent();
        }

        private async void addButton_Clicked(object sender, EventArgs e)
        {
           
            Vaccines vacs = new Vaccines()
            {
                PetId = id,
                Vacname = nameEntry.Text,
                VacDate = vacDatePicker.Date,
                VacExpDate = vacExpDatePicker.Date                               
            };
            try
            {
                //Datan serialisointi ja vienti API:lle
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://pethouse.azurewebsites.net/");
                string input = JsonConvert.SerializeObject(vacs);
                StringContent content = new StringContent(input, Encoding.UTF8, "application/json");

                // Lähetetään serialisoitu objekti back-endiin Post pyyntönä
                HttpResponseMessage message = await client.PostAsync("/api/vaccines/", content);

                // Otetaan vastaan palvelimen vastaus
                string reply = await message.Content.ReadAsStringAsync();

                //Asetetaan vastaus serialisoituna success muuttujaan
                bool success = JsonConvert.DeserializeObject<bool>(reply);


                if (success)  // Näytetään ehdollisesti alert viesti
                {

                    await DisplayAlert("New vaccine added", "Success", "Done"); // (otsikko, teksti, kuittausnapin teksti)


                }
                else
                {
                    await DisplayAlert("Error", "Vaccine could not be added", "Close"); // Muutettu 4.5.
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
    }
}