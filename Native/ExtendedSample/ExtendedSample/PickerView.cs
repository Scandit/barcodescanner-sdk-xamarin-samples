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

        public static string GetAppKey()
        {
            return "AUSrZQgfFegTMaMl3UPk11YKZtj3KdetWX+nLv5jFRMEdXd3SkM0xi5uMs5JTR8xJl/FR2Vs7yCObka2yCEWaUVv/sZkc9OKzBgjasdRwmH7XkE0Sn5D/CNpJnXEd35fp2yQoQxzPfeCaG9HWUXwLvVW8aAzfib6G1ETKlFD0vGyVtEiPUrebqt8iem+cFxa10MwWfZg6Tp7bGq3XXRoaz50a1L5dtXdE3gRfrhOL6irauTmkXRiYkRWcON6aQCjsU7zNDpUijiBbCnKy2LPBtdccFtfQy2QYnS+FL0o9zF/d8WMOXDdI2Rcqe4pQU/1c0i4Ce4GpzNgC6XazhQjH6mFunp59E2nbOhbhZaTUZQm00SodYoX9iryvcYqjnepqT8WEGORPZzb2bry8t0X0HPNvlrnjSMuxnCjLl9n4CseLqisJVNlgJsWoSD67UbvFjMkNKXrZDLFHBhUh8tG9LIkVu/5f1gBJqtvOUQ4cKNHjH7yrTAMkk+P0XwupLHXVp/yDnafCDv8Tn1jAlC/+OpQsjeF7zkI1j7jOrV09IxWCT+eiGOo/9eGFYtAEo7KsQSHTvUECaSwwKd/gOZt4bh7SfDmNuIP10d5myF1rFYVbcOTeRVrmmr0PhrO3s4LyBRp8QK3GzSCs/Jp6TLfQCIBz3nHbW/wcVBmCyGTA8zOPzYuJFAXV/drQQUwpQavKiA1h8g71foX20Dn2bqYnAJK4goXu2bdK/9637MoLjsRTAZvT3pVrG/1aM2kkKPVgFQtDVcaQLdsHJuTyXX2M0ni40sTkCQN0YfiFjgAasnozw/5uUuDCzJVcNFq6JGdfM/zSNiK+EQaP/w2kLlf1gw1bg7iHw5CtQHqvKa2mOP0g3zntApB88ClErDwiYO4VmhQM20BalxtTbKkUhPE7lVz+e2/9z8yOPLKpIiQgqWo7xIauOjbBY3iUjgRYndtfeSM3hWXBW/1KKc8+a9ptrZPSWV+QsMNngID6rspuxsD7ScM+OH8GSperqPXGA==";
        }

		public void StartScanning()
		{
            StartScanningRequested?.Invoke(this, EventArgs.Empty);
        }

        public void PauseScanning()
        {
            PauseScanningRequested?.Invoke(this, EventArgs.Empty);
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
