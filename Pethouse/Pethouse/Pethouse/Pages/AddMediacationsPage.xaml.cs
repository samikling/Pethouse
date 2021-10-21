﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pethouse.Models;
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
                //debugEntry.Text = errorMessage1; // ..näyttäminen list viewissä
                //debugEntry2.Text = errorMessage2;
            }
        }
    }
}