using System;

using Foundation;
using UIKit;

namespace iOSSplitViewSample
{
    public partial class SplitViewResultTableViewCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("SplitViewResultTableViewCell");
        public static readonly UINib Nib;

        static SplitViewResultTableViewCell()
        {
            Nib = UINib.FromName("SplitViewResultTableViewCell", NSBundle.MainBundle);
        }

        protected SplitViewResultTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public void setBarcode(string barcode)
        {
            barcodeLabel.Text = barcode;
        }

        public void setSymbology(string symbology)
        {
            symbologyLabel.Text = symbology;
        }

    }
}
