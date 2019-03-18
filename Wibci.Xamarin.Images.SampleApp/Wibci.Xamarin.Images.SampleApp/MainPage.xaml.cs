using Xamarin.Forms;

namespace Wibci.Xamarin.Images.SampleApp
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
			BindingContext = new MainViewModel();
		}
	}
}
