using System;
using Xamarin.Forms;
using ExtendedSample.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(UrlOpener))]
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
