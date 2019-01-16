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
        private static string appKey = "AejrGtaBQvQqNMrEAkQLzw02rYwJABlEwhAFM5tEi7HjXXqr1VLWogBGjaj0OGhsZw77bxs9VY3iYa+CeUWVRUQBtbCuZYmSzx4AbygoXfUfXxVFwgNYz3MdcGz3/FPfa9b2z9iqAw02ZaAto+lUaDf/UK/QSIEgajjMtL8voTHUw13MzmkQx9Xa4gt2614/8spveyOYVdOXH+fxZKv5Z98Y7IRKobw4duJmL8Hj+BBfV/Z8xQJNcSzQN/W0Zj3kteAvj6dx+DRp9BUOApRf6XH3TOHket72S4TrY/aChxXd0K/NVbDBAr4jXWUmBbpu80p5Nx9c17i8F9bwu5hDAWX3PIE7W0+FC0fDZH+Ud1KoFip2n0QuEG/Bq8CqCbrPV37uhyEQTYC9AgLPp8Hxaq6ru3lcRA6LadEyeVpc5rKpvHln7RoIiEEBzyy8Yng92YT3kRuK0zZQG7ySau4PxH7YViyEItKemx48CpWifKanf5pouM8k+YEAsG6qlG6Hkehi0WJ9iCFF9TK8KR/Uq1vqaxD+mDDCsqkL8vqy8ObyU6AzFngwxbPyG52P/Yr6wHQdbT3r+kbJOOensGROufGt6cpaC0RQHluhZnU9UVnRs2qSlbQLPTtWvPaXQvZGzvzgnPR1do50rNv2lYzpNWXz6RjAodmc0Zxpq8/31KueqQfvsdPhjFNZXCEM/narojxPipa36xsLRfCIQ0uRFRlfurjLTUTyfApHEkCa4cDwHVt1wRR8Q5AMLUB6SfJHiSGWFLOPaTDUPWzUY9gLSYvXb8T8YJpwryc=";

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

