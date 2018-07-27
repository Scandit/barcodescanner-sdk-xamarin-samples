using Android.Content;
using Android.Net;
using ExtendedSample.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(UrlOpener))]
namespace ExtendedSample.Droid
{
    public class UrlOpener : IUrlOpener
    {
        public void OpenUrl(string urlString)
        {
            var uri = Uri.Parse(urlString);
            var intent = new Intent(Intent.ActionView, uri);
            Android.App.Application.Context.StartActivity(intent);
        }
    }
}
