using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ExtendedSample
{
	public interface IScannerDelegate
	{
		Task DidScan(string symbology, string code);
	}

    public partial class ScannerPage : ContentPage
    {
        public Settings Settings { get; set; }

		public ScannerPage()
		{
            this.Settings = new Settings();
			InitializeComponent();
		}

		public ScannerPage(Settings settings)
        {
			this.Settings = settings;
			InitializeComponent();
            PickerView.Delegate = new ScannerDelegate(this);
            PickerView.Settings = Settings;
        }

		public void Handle_Clicked(object sender, System.EventArgs e)
		{
			HideResult();
            PickerView.StartScanning();
		}

		public void HideResult()
        {
			ResultView.IsVisible = false;
		}

		public void ShowResult(String symbology, String code)
        {
			ResultView.IsVisible = true;
            SymbologyLabel.Text = symbology;
            CodeLabel.Text = code;
		}

        public void ResumeScanning() 
        {
            PickerView.StartScanning();
        }

        public void PauseScanning() 
        {
            PickerView.PauseScanning();
        }
    }

    public class ScannerDelegate : IScannerDelegate
    {
        private ScannerPage scannerPage;

        public ScannerDelegate(ScannerPage scannerPage)
        {
            this.scannerPage = scannerPage;
        }

        public async Task DidScan(string symbology, string code)
        {
            scannerPage.ShowResult(symbology, code);

            if (scannerPage.Settings.ContinuousAfterScan) 
            {
                await Task.Delay(1000);
                scannerPage.HideResult();
            }
        }
    }
}
