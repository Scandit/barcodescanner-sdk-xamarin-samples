using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TextRecognitionSample
{
    public interface IScannerDelegate
    {
        void DidScan(string value);
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

        public void ShowResult(string recognizedText)
        {
            ResultView.IsVisible = true;
            RecognizedText.Text = recognizedText;
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

        public void DidScan(string value)
        {
            scannerPage.ShowResult(value);
        }
    }
}
