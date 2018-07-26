using Xamarin.Forms;
using ExtendedSample.Droid;

[assembly: Dependency(typeof(UrlOpener))]
namespace ExtendedSample.Droid
{
    public class UrlOpener : IUrlOpener
    {
        public void OpenUrl(string urlString)
        {
            // TODO: 
        }
    }
}
