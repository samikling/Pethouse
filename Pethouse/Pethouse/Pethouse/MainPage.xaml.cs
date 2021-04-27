using Newtonsoft.Json;
using Pethouse.Models;
using System;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace Pethouse
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void LoginBtn_OnClicked(object sender, EventArgs e)
        {
            string userName = this.userName.Text; //Get username from Username entry field
            string passWord = this.pwdEntryOne.Text; //Get password from Password entry field
            HttpClient client = new HttpClient(); //Initialize HttpClient
            Login login = new Login() //New instance of Login class with user credentials
            {
                UserName = userName,
                PassWord = passWord
            };
            //JsonConvert
            string input = JsonConvert.SerializeObject(login);
            StringContent content = new StringContent(input, Encoding.UTF8, "application/json");
            //Send post
            HttpResponseMessage message = await client.PostAsync("/api/users", content);
            //Otetaan vastaan palvelimen vastaus
            string reply = await message.Content.ReadAsStringAsync();

            //Asetetaan vastauus serialisoituna success muuttujaan
            bool success = JsonConvert.DeserializeObject<bool>(reply);

            //Näytetään ehdollisesti alert viesti
            if (success)
            {
                //Tähän siirtyminen seuraavaan näkymään
                await DisplayAlert("Työn aloitus", "Työ aloitettu.", "Sulje");
            }
            else
            {
                await DisplayAlert("Login not successfull", "Incorrect credentials or user not found", "close");
            }

            /*TODO:
            client.BaseAddress = new Uri(""); //API address
            Send get request with parameters: userName & passWord
            If username & password are in the database respond with True
            else
            respond with false
            show error message "Incorrect password or user not found".
            */

            throw new NotImplementedException();
        }
    }
}