using System;
using Xamarin.Forms;

namespace ExtendedSample
{
	public partial class ScannerPage : ContentPage
	{
        public Settings Settings { get; }

		public ScannerPage(Settings settings)
		{
            this.Settings = settings;
		}
	}
}
