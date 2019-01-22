// Copyright 2019 Scandit AG
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Scandit.BarcodePicker.Unified;
using SimpleSample.ViewModels.Abstract;
using Xamarin.Forms;

namespace SimpleSample.ViewModels
{
    public class SimpleSampleViewModel : BaseViewModel
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
