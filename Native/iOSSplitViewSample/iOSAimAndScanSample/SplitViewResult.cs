using System;

namespace iOSSplitViewSample
{
    public struct SplitViewResult
    {
        public string barcode, symbology;

        public SplitViewResult(string barcode, string symbology) {
            this.barcode = barcode;
            this.symbology = symbology;
        }

    }
}
