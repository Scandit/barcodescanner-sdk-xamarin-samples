using System;
using System.Collections.Generic;

namespace AndroidViewBasedMatrixScanSample.Scan.Bubbles
{
    public class BubbleData
    {
        public string Code
        {
            get; private set;
        }

        public int Stock
        {
            get; private set;
        }

        public int Online
        {
            get; private set;
        }

        public string DeliveryDate
        {
            get; private set;
        }

        public BubbleData(IList<string> values)
        {
            Code = values[0];
            Stock = Int32.Parse(values[1]);
            Online = Int32.Parse(values[2]);
            DeliveryDate = values[3];
        }
    }
}
