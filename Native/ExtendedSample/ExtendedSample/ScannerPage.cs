using System;
using Xamarin.Forms;

namespace ExtendedSample
{
	public partial class ScannerPage : ContentPage
	{
        public Settings Settings { get; set; }

		public ScannerPage(Settings settings)
		{
			Settings = settings;
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //pickerView.Settings = new Settings();

        }
	}
}
