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

using Scandit.BarcodePicker.Unified;
using Scandit.BarcodePicker.Unified.Abstractions;
using Xamarin.Forms;

namespace SimpleSample
{
	public partial class App : Application
    {
        private static string appKey = "-- ENTER YOUR SCANDIT LICENSE KEY HERE --";

        public App()
        {
            // must be set before you use the picker for the first time.
            ScanditService.ScanditLicense.AppKey = appKey;
            initSettings();

            InitializeComponent();

			MainPage = new SimpleSamplePage();
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}

        async void initSettings()
        {
            IBarcodePicker picker = ScanditService.BarcodePicker;

            // The scanning behavior of the barcode picker is configured through scan
            // settings. We start with empty scan settings and enable a very generous
            // set of symbologies. In your own apps, only enable the symbologies you
            // actually need.
            var settings = picker.GetDefaultScanSettings();
            var symbologiesToEnable = new Symbology[] {
                Symbology.Qr,
                Symbology.Ean13,
                Symbology.Upce,
                Symbology.Ean8,
                Symbology.Upca,
                Symbology.Code128,
                Symbology.DataMatrix
            };
            foreach (var sym in symbologiesToEnable)
                settings.EnableSymbology(sym, true);
            await picker.ApplySettingsAsync(settings);
            // This will open the scanner in full-screen mode. 
        }
    }
}

