using Newtonsoft.Json;
using Pethouse.Models;
using System;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace Pethouse
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            if (LoginInfo.LoggedIn)
            {
                Navigation.PushModalAsync(new MainPage());
            }
        }

        private async void LoginBtn_OnClicked(Object sender, EventArgs e)
        {
            try
            {
                Login login = new Login()
                {
                    UserName = userName.Text,
                    PassWord = pwdEntryOne.Text
                };
                //Initialize http client
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://pethouse.azurewebsites.net/"); //https://84.230.166.79/

                string input = JsonConvert.SerializeObject(login);
                StringContent content = new StringContent(input, Encoding.UTF8, "application/json");
                //Send serialised data as json
                HttpResponseMessage message = await client.PostAsync("/api/users", content);

                //Response handling
                string reply = await message.Content.ReadAsStringAsync();

                Login obj = JsonConvert.DeserializeObject<Login>(reply);
               
                Console.WriteLine(obj.ToString());

                //bool success = JsonConvert.DeserializeObject<bool>(reply);

                if (obj != null)
                {
                    //obj.UserId = LoginInfo.UserId;
                    LoginInfo.UserId = obj.UserId.Value;
                    LoginInfo.LoggedIn = true;
                    await Navigation.PushModalAsync(new MainPage());
                }
                //if (success)
                //{
                //    LoginInfo.LoggedIn = true;
                //    //await DisplayAlert("Login status", "Success!", "OK");
                //    await Navigation.PushModalAsync(new MainPage());
                //}
                else
                {
                    await DisplayAlert("Login status", "Not Successfull!", "OK");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}