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
    public partial class AddUserPage : ContentPage
    {
        public AddUserPage()
        {
            InitializeComponent();
        }

        private async void submitButton_Clicked(object sender, EventArgs e)
        {
            if (passwordEntry.Text == pwcheckEntry.Text)
            {

                Users user = new Users()
                {
                    Username = nameEntry.Text,
                    Password = pwcheckEntry.Text,
                    Email = "placeholder@placeholder.com"

                };
                try
                {

                    //Datan serialisointi ja vienti API:lle

                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("https://pethouse.azurewebsites.net/");
                    string input = JsonConvert.SerializeObject(user);
                    StringContent content = new StringContent(input, Encoding.UTF8, "application/json");

                    // Lähetetään serialisoitu objekti back-endiin Post pyyntönä

                    HttpResponseMessage message = await client.PostAsync("/api/users/new", content);

                    // Otetaan vastaan palvelimen vastaus

                    string reply = await message.Content.ReadAsStringAsync();

                    //Asetetaan vastaus serialisoituna success muuttujaan

                    bool success = JsonConvert.DeserializeObject<bool>(reply);


                    if (success)  // Näytetään ehdollisesti alert viesti
                    {

                        await DisplayAlert("New user created", "Success", "Done"); // (otsikko, teksti, kuittausnapin teksti)
                        _ = Navigation.PopModalAsync();


                    }
                    else
                    {
                        await DisplayAlert("Error", "User could not be added, try different username.", "Close"); // Muutettu 4.5.
                    }
                }
                catch (Exception ex) // Otetaan poikkeus ex muuttujaan ja sijoitetaan errorMessageen
                {

                    string errorMessage1 = ex.GetType().Name; // Poikkeuksen customoitu selvittäminen ja...
                    string errorMessage2 = ex.Message;
                    await DisplayAlert(errorMessage1, errorMessage2, "Error");
                    //debugEntry.Text = errorMessage1; // ..näyttäminen list viewissä
                    //debugEntry2.Text = errorMessage2;
                }
            }
            else
            {
                await DisplayAlert("Error", "Passwords don't match. Check your password.", "Ok");
            }
        }
    }
}
