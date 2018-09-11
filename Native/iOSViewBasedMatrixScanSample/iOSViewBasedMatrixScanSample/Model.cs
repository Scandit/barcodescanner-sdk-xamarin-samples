using System;
using UIKit;

namespace iOSViewBasedMatrixScanSample
{
    public struct Model
    {
        public readonly UIColor Color;
        public readonly nint StockCount;
        public readonly string DeliveryDate;
        public readonly string Code;

        private static readonly Random random = new Random();
        
        private static readonly UIColor greenStock = new UIColor(red: 57.0f/255.0f, green: 204.0f/255.0f, blue: 97.0f/255.0f, alpha: 1.0f);
        private static readonly UIColor greenStockTransparent = greenStock.ColorWithAlpha(0.6f);
        private static readonly UIColor yellowStock = new UIColor(red: 250.0f/255.0f, green: 208.0f/255.0f, blue: 92.0f/255.0f, alpha: 1.0f);
        private static readonly UIColor yellowStockTransparent = yellowStock.ColorWithAlpha(0.6f);
        private static readonly UIColor redStock = new UIColor(red: 228.0f/255.0f, green: 76.0f/255.0f, blue: 76.0f/255.0f, alpha: 1.0f);

        private static readonly int SuffixLength = 2;
        private static readonly nint Modulo = 5;


        private static readonly UIColor redStockTransparent = redStock.ColorWithAlpha(0.6f);

        private Model(UIColor color, nint stockCount, string deliveryDate, string code)
        {
            Color = color;
            StockCount = stockCount;
            DeliveryDate = deliveryDate;
            Code = code;
        }

        public static Model MockedModel(string data)
        {
            return new Model(MockedColor(data), MockedCount(data), MockedDate(data), data);            
        }

        private static string MockedDate(string data)
        {
            var start = new DateTime(2018, 1, 1);
            var range = (DateTime.Today - start).Days;           
            return start.AddDays(random.Next(range)).ToString("MM-dd-yy");
        }

        private static nint MockedCount(string data)
        {
            var suffix = data.Substring((int)(data.Length - SuffixLength));
            int.TryParse(suffix, out var result);
            return result <= SuffixLength
                ? result % Modulo
                : result <= Modulo ? 11 + int.Parse(data.Substring((int)(data.Length - SuffixLength))) : 6 + result % Modulo;
        }

        public static UIColor MockedColor(string data)
        {
            var suffix = data.Substring((int)(data.Length - SuffixLength));
            int.TryParse(suffix, out var result);
            return result <= SuffixLength ? redStockTransparent : result <= Modulo ? greenStockTransparent : yellowStockTransparent;
        }
    }
}