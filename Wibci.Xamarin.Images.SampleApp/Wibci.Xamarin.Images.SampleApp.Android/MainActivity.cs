
using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.Media;
using Wibci.Xamarin.Images.Droid;
using Xamarin.Forms;

namespace Wibci.Xamarin.Images.SampleApp.Droid
{
	[Activity(Label = "Wibci.Xamarin.Images.SampleApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(savedInstanceState);
			global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

			DependencyService.Register<IAnalyseImageCommand, AndroidAnalyseImageCommand>();
			DependencyService.Register<IResizeImageCommand, AndroidResizeImageCommand>();
			DependencyService.Register<IConvertImageCommand, AndroidConvertImageCommand>();
			DependencyService.Register<IImageTools, AndroidImageTools>();
			CrossMedia.Current.Initialize();

			LoadApplication(new Wibci.Xamarin.Images.SampleApp.App());
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
		{
			Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}
	}
}