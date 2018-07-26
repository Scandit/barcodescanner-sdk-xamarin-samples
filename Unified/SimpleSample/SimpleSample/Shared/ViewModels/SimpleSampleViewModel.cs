using SimpleSample.ViewModels.Abstract;
using Scandit.BarcodePicker.Unified;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Diagnostics;

namespace SimpleSample.ViewModels
{
    public class SimpleSampleViewModel:BaseViewModel
    {
        private string _recognizedCode;

        public string RecognizedCode
        {
            get
            {
                return (_recognizedCode == null) ? "" : "Code scanned: " + _recognizedCode;
            }

            set
            {
                _recognizedCode = value;
                OnPropertyChanged(nameof(RecognizedCode));
            }
        }

        public SimpleSampleViewModel()
        {
            ScanditService.BarcodePicker.DidScan += BarcodePickerOnDidScan;
        }

        private async void BarcodePickerOnDidScan(ScanSession session)
        {
            RecognizedCode = session.NewlyRecognizedCodes.LastOrDefault()?.Data;
		    await ScanditService.BarcodePicker.StopScanningAsync();
        }

        public ICommand StartScanningCommand => new Command(async () => await StartScanning());

        private async Task StartScanning()
        {
            await ScanditService.BarcodePicker.StartScanningAsync(false);
        }
    }
}
