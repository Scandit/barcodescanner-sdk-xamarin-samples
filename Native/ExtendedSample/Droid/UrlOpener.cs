using Xamarin.Forms;
using ExtendedSample.Droid;

[assembly: Dependency(typeof(UrlOpener))]
namespace ExtendedSample.Droid
{
    public class UrlOpener : IUrlOpener
    {
        public void OpenUrl(string urlString)
        {
            var uri = Android.Net.Uri.Parse(urlString);
            var intent = new Android.Content.Intent(Android.Content.Intent.ActionView, uri);
            Android.App.Application.Context.StartActivity(intent);
        }
    }
}
