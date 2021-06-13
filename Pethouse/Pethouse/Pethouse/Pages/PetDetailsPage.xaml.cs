﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public partial class PetDetailsPage : ContentPage
    {
        int petId;
        public PetDetailsPage(int idParam)
        {
            InitializeComponent();
            petId = idParam;
            LoadDetails(null,null);
        }
        private async void LoadDetails(object sender, EventArgs e)
        {
            Pets pet = new Pets();
            //Get pet details from database
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("https://pethouse.azurewebsites.net/")
            };
            string json = await client.GetStringAsync("/api/pets/" + petId);
            Pets pets = JsonConvert.DeserializeObject<Pets>(json);

            //IEnumerable<Pets> pets = JsonConvert.DeserializeObject<Pets[]>(json);
            //ObservableCollection<Pets> dataa = new ObservableCollection<Pets>(pets);
            try
            {
                petTable.BindingContext = pets;
                nameLabel.Text = pets.Petname;
                
                if (pets.RaceId == 1)
                {
                    petRace.Detail = "Dog";
                }
                else
                {
                    petRace.Detail = "Cat";
                }
                //Get breed function
                //!!!TODO!!!
                petBreed.Detail = pets.Breed.ToString();
                

            }
            catch (Exception ex)
            {

                Debug.WriteLine("Virhe " + ex);
            }
        }
    }
}