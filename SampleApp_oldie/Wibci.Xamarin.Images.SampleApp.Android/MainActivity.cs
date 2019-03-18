using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.Media;
using Plugin.Permissions;
using System;
using Wibci.Xamarin.Images.Droid;
using Xamarin.Forms;

namespace Wibci.Xamarin.Images.SampleApp.Droid
{
    [Activity(Label = "Wibci.Xamarin.Images.SampleApp", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            DependencyService.Register<IAnalyseImageCommand, AndroidAnalyseImageCommand>();
            DependencyService.Register<IResizeImageCommand, AndroidResizeImageCommand>();
            DependencyService.Register<IConvertImageCommand, AndroidConvertImageCommand>();
            DependencyService.Register<IImageTools, AndroidImageTools>();
            CrossMedia.Current.Initialize();

            LoadApplication(new App());
        }
    }
}