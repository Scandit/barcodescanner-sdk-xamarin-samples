using Xamarin.Forms;
using UIKit;
using Foundation;
using ExtendedSample.iOS;

[assembly: Dependency(typeof(UrlOpener))]
namespace ExtendedSample.iOS
{
    public class UrlOpener : IUrlOpener
    {
        public void OpenUrl(string urlString)
        {
            var url = new NSUrl(urlString);
            UIApplication.SharedApplication.OpenUrl(url);
        }
    }
}
