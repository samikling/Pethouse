﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using Pethouse.Models;
namespace Pethouse.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GetGroomingPage : ContentPage
    {
        int id = 0;
        public GetGroomingPage(int PetId)
        {
            InitializeComponent();
            id = PetId;
            GetGrooming();
        }
        private async void GetGrooming()
        {
            //Initialize and setup httpclient and base address
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://pethouse.azurewebsites.net/");
            //Load vaccines
            string json = await client.GetStringAsync("/api/grooming/list/" + id);
            IEnumerable<Grooming> groom = JsonConvert.DeserializeObject<Grooming[]>(json);
            ObservableCollection<Grooming> dataa = new ObservableCollection<Grooming>(groom);
            groomList.ItemsSource = dataa;
        }

        private void groomList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Grooming groom = (Grooming)groomList.SelectedItem;
            //int id = pet.PetId;
            _ = Navigation.PushModalAsync(new EditGroomingPage(groom.GroomId, groom.Groomname, groom.GroomDate, groom.GroomExpDate, groom.Comments));
        }
    }
}