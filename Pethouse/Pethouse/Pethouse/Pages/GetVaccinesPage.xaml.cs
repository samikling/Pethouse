using Newtonsoft.Json;
using Pethouse.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pethouse.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GetVaccinesPage : ContentPage
    {
        int id = 0;
        public GetVaccinesPage(int PetId)
        {
            InitializeComponent();
            id = PetId;
            GetVaccines();
            OnAppearing();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            GetVaccines();
        }

        private async void GetVaccines()
        {
            //Initialize and setup httpclient and base address
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://pethouse.azurewebsites.net/");
            //Load vaccines
            string json = await client.GetStringAsync("/api/vaccines/list/" + id);
            IEnumerable<Vaccines> vacs = JsonConvert.DeserializeObject<Vaccines[]>(json);
            ObservableCollection<Vaccines> dataa = new ObservableCollection<Vaccines>(vacs);
            vacList.ItemsSource = dataa;
        }
        private void vacList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Vaccines vac = (Vaccines)vacList.SelectedItem;
            //int id = pet.PetId;
            _ = Navigation.PushModalAsync(new EditVaccinePage(vac.VacId, vac.Vacname, vac.VacDate, vac.VacExpDate));
        }
    }
}