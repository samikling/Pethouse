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
    public partial class EditGroomingPage : ContentPage
    {
        Grooming groomingOld = new Grooming();
        public EditGroomingPage(int id, string groomname, DateTime? groomdate, DateTime? groomexpdate,string comments)
        {
            InitializeComponent();
            groomingOld.GroomId = id;
            groomingOld.Groomname = groomname;
            groomingOld.GroomDate = groomdate;
            groomingOld.GroomExpDate = groomexpdate;
            groomingOld.Comments = comments;
            LoadDetails(id,groomname,groomdate,groomexpdate,comments);
        }

        private void LoadDetails(int id, string groomname, DateTime? groomdate, DateTime? groomexpdate,string comments)
        {
            //Initialize and setup httpclient and base address
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://pethouse.azurewebsites.net/");
            //Load vaccines
            Grooming edit = new Grooming()
            {
                GroomId = id,
                Groomname = groomname,
                GroomDate = groomdate,
                GroomExpDate = groomexpdate,
                Comments= comments

            };
            editGroomStack.BindingContext = edit;
            //string jsonString = JsonConvert.SerializeObject(edit);
            //string json = await client.GetStringAsync("/api/vaccines/edit" + id);
            //IEnumerable<Vaccines> vacs = JsonConvert.DeserializeObject<Vaccines[]>(json);
            //ObservableCollection<Vaccines> dataa = new ObservableCollection<Vaccines>(vacs);

        }

        private async void saveButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                int id = groomingOld.GroomId;
                Grooming groomNew = new Grooming
                {
                    Groomname = nameEntry.Text,
                    GroomDate = groomdatePicker.Date,
                    GroomExpDate = groomexpdatePicker.Date,
                    Comments = commentsEditor.Text
                };
                //Datan serialisointi ja vienti API:lle
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://pethouse.azurewebsites.net/");
                string newObject = JsonConvert.SerializeObject(groomNew);
                StringContent newContent = new StringContent(newObject, Encoding.UTF8, "application/json");


                // Lähetetään serialisoitu objekti back-endiin Put pyyntönä
                HttpResponseMessage message = await client.PutAsync("/api/grooming/" + id, newContent);

                // Otetaan vastaan palvelimen vastaus
                string reply = await message.Content.ReadAsStringAsync();

                //Asetetaan vastaus serialisoituna success muuttujaan
                bool success = JsonConvert.DeserializeObject<bool>(reply);

                if (success)  // Näytetään ehdollisesti alert viesti
                {
                    await DisplayAlert("Operation " + groomNew.Groomname.ToString() + " - Edit", "Success", "Done"); // (otsikko, teksti, kuittausnapin teksti)
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
            int id = groomingOld.GroomId;
            bool response = await DisplayAlert("Delete " + groomingOld.Groomname + "?", "Really Delete?", "Yes", "No");

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://pethouse.azurewebsites.net/");
            try
            {
                if (response)
                {
                    HttpResponseMessage message = await client.DeleteAsync("/api/grooming/" + id);
                    if (message.IsSuccessStatusCode)
                    {
                        await DisplayAlert("OK", "The treatment " + groomingOld.Groomname + " was deleted.", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Cancelled", "The treatment " + groomingOld.Groomname + " was not deleted.", "Ok");
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