using Xamarin.Forms;

namespace Wibci.Xamarin.Images.SampleApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.Register<IChoosePictureCommand, ChoosePictureCommand>();

            MainPage = new Wibci.Xamarin.Images.SampleApp.MainPage();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }
    }
}