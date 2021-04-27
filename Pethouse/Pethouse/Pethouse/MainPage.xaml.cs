using Xamarin.Forms;

namespace Pethouse
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Send Username and Password to the API and checks if such credentials are found.
        /// If such credentials exist API returns true and the Login is succesfull.
        /// If not the API returns a value of False.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        public void Login(string userName, string passWord)
        {
            //Login method

        }
        /// <summary>
        /// Redirects the user to the registration view
        /// </summary>
        public void Register()s
        {
            //Redirects the user to the register view
        }
    }
}