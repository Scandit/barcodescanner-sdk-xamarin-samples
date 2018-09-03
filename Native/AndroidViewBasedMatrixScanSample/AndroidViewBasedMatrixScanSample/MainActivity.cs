using Android.App;
using Android.Widget;
using Android.OS;

namespace AndroidViewBasedMatrixScanSample
{
    [Activity(Label = "AndroidViewBasedMatrixScanSample", MainLauncher = true, Icon = "@mipmap/ic_launcher")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
        }
    }
}

