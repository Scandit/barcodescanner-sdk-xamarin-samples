using System;
using Xamarin.Forms;

namespace ExtendedSample
{
    public class PickerView : View
    {
		public event EventHandler ResumeScanningRequested;
		public event EventHandler PauseScanningRequested;
        public IScannerDelegate Delegate { get; set; }

		public void ResumeScanning()
		{
			if (ResumeScanningRequested != null)
			{
				ResumeScanningRequested(this, EventArgs.Empty);
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
