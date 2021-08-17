
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pethouse.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPetPage : ContentPage
    /*
    General notes for solution:
    Prequisites

    Firstly to create a get function that queries the database for all the avaiable breeds, so that they can be show on the selection menu.
    It should propably be of type List<Breeds>
    UserId = static(should allready exist, see login)
    Petname
    Birthdate
    Photo
    Race_id -  Dropdown box, either cat or a dog
    Breed_id - Based on race selection show all breeds for that race
    */
    {
        public AddPetPage()
        {
            InitializeComponent();
        }
    }
}
