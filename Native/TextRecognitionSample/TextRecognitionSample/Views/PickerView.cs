using System;
using Xamarin.Forms;

namespace TextRecognitionSample
{
    public class PickerView : View
    {
        public event EventHandler StartScanningRequested;
        public event EventHandler PauseScanningRequested;
        public IScannerDelegate Delegate { get; set; }
        public Settings Settings { get; set; }

        public void StartScanning()
        {
            if (StartScanningRequested != null)
            {
                StartScanningRequested(this, EventArgs.Empty);
            }
        }

        public void PauseScanning()
        {
            if (PauseScanningRequested != null)
            {
                PauseScanningRequested(this, EventArgs.Empty);
            }
        }

        public void DidScan(string value)
        {
            if (Delegate != null)
            {
                Delegate.DidScan(value);
            }
        }
    }
}
