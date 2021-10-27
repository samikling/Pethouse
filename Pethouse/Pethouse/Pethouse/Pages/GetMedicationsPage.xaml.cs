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
    public partial class GetMedicationsPage : ContentPage
    {
        int id = 0;
        public GetMedicationsPage(int PetId)
        {
            InitializeComponent();
            id = PetId;
            GetMedications();
            OnAppearing();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            GetMedications();
        }
        private async void GetMedications()
        {
            //Initialize and setup httpclient and base address
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://pethouse.azurewebsites.net/");
            //Load vaccines
            string json = await client.GetStringAsync("/api/medications/list/" + id);
            IEnumerable<Medications> meds = JsonConvert.DeserializeObject<Medications[]>(json);
            ObservableCollection<Medications> dataa = new ObservableCollection<Medications>(meds);
            medList.ItemsSource = dataa;
        }

        private void medList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Medications med = (Medications)medList.SelectedItem;
            //int id = pet.PetId;
            _ = Navigation.PushModalAsync(new EditMedicationsPage(med.MedId, med.Medname, med.MedDate, med.MedExpDate));
        }
    }
}