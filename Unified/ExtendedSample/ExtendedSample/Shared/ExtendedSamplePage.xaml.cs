// Copyright 2016 Scandit AG
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

using Xamarin.Forms;
using Scandit.BarcodePicker.Unified;
using Scandit.BarcodePicker.Unified.Abstractions;
using System.Linq;

namespace ExtendedSample
{
	public partial class ExtendedSamplePage : ContentPage
	{
		void OnDidScan(ScanSession session)
        {
            var firstCode = session.NewlyRecognizedCodes.First();
            var message = "";
            if (session.NewlyRecognizedCodes.Count() == 1)
            {
                message = string.Format("Code Scanned:\n {0}\n({1})", firstCode.Data,
                                            firstCode.SymbologyString.ToUpper());
            }
            else if (session.NewlyRecognizedCodes.Count() > 1)
            {
                var secondCode = session.NewlyRecognizedCodes.ElementAt(1);
                message = string.Format("Codes Scanned:\n {0}\n({1})\n {2}\n({3})", 
                                        firstCode.Data,
                                        firstCode.SymbologyString.ToUpper(),
                                        secondCode.Data,
                                        secondCode.SymbologyString.ToUpper());
            }
			// Because this event handler is called from an scanner-internal thread, 
			// you must make sure to dispatch to the main thread for any UI work.
			Device.BeginInvokeOnMainThread(() => this.ResultLabel.Text = message);
			session.StopScanning();
		}

		async void OnSettingsClicked(object sender, System.EventArgs e)
		{
			await Navigation.PushAsync(_settingsPage);
		}

		async void OnScanButtonClicked(object sender, System.EventArgs e)
		{
			
			// The scanning behavior of the barcode picker is configured through scan
			// settings. We start with empty scan settings and enable a very generous
			// set of symbologies. In your own apps, only enable the symbologies you
			// actually need.
			await _picker.StartScanningAsync(false);
		}

		IBarcodePicker _picker;

		SettingsPage _settingsPage;

		public static string appKey = "-- ENTER YOUR SCANDIT LICENSE KEY HERE --";

		public ExtendedSamplePage()
		{
			InitializeComponent();
			// must be set before you use the picker for the first time.
			ScanditService.ScanditLicense.AppKey = appKey;
			// retrieve the actual native barcode picker implementation. The concrete implementation 
			// will be different depending on the platform the code runs on.
			_picker = ScanditService.BarcodePicker;
			// will get invoked whenever a code has been scanned.
			_picker.DidScan += OnDidScan;

			// set up the settings page
			_settingsPage = new SettingsPage();
		}
	}
}

