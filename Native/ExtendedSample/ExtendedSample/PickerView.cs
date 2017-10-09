using System;
using Xamarin.Forms;

namespace ExtendedSample
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

        public void DidScan(string symbology, string code)
        {
            if (Delegate != null)
            {
                Delegate.DidScan(symbology, code);
            }
        }
    }
}
