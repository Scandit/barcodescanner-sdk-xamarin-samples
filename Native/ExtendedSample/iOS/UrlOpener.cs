using System;
using Xamarin.Forms;
using SafariServices;
using UIKit;
using Foundation;
using ExtendedSample.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(UrlOpener))]
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
