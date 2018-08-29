using System;
using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;

namespace ExtendedSample.Droid
{
    [Activity(Label = "ExtendedSample.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private const int CAMERA_REQUEST = 1;

        private bool paused = true;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Forms.Forms.Init(this, savedInstanceState);
        }

        protected override void OnResume()
        {
            base.OnResume();
            paused = false;

            if ((int)Build.VERSION.SdkInt >= 23)
            {
                GrantCameraPermissionsThenStartScanning();
            }
            else
            {
                LoadApplication(new App());
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
            paused = true;

            // Call GC.Collect() before stopping the scanner as the garbage collector for some reason does not
            // collect objects without references asap but waits for a long time until finally collecting them.
            GC.Collect();
        }

        private void GrantCameraPermissionsThenStartScanning()
        {
            if (CheckSelfPermission(Manifest.Permission.Camera) != (int)Permission.Granted)
            {
                RequestPermissions(new String[] { Manifest.Permission.Camera }, CAMERA_REQUEST);
            }
            else
            {
                LoadApplication(new App());
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            if (requestCode == CAMERA_REQUEST)
            {
                if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
                {
                    if (!paused)
                    {
                        LoadApplication(new App());
                    }
                }
                return;
            }
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
