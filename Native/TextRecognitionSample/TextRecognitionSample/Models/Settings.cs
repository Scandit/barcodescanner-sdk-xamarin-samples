using System;
using System.Text.RegularExpressions;

namespace TextRecognitionSample
{
    public enum Mode 
    {
        IBAN, GS1AI, ScanPrice    
    }

    public enum ScanPosition
    {
        Top, Center, Bottom
    }

    public class Settings
    {
        public Mode Mode { get; set; }
        public ScanPosition ScanPosition { get; set; }

        public Settings()
        {
            ResetSettings();
        }

        public void ResetSettings()
        {
            Mode = Mode.IBAN;
            ScanPosition = ScanPosition.Center;
        }

        public string RegularExpression()
        {
            string pattern = "";
            switch (Mode)
            {
                case Mode.IBAN:
                    pattern = "([A-Z]{2}[A-Z0-9]{2}\\s([A-Z0-9]{4}\\s){4,}([A-Z0-9]{1,4}))";
                    break;
                case Mode.GS1AI:
                    pattern = "((\\(01\\)[0-9]{13,14})(\\s*(\\(10\\)[0-9a-zA-Z]{1,20})|(\\(11\\)[0-9]{6})|(\\(17\\)[0-9]{6})|(\\(21\\)[0-9a-zA-Z]{1,20}))+)";
                    break;
                case Mode.ScanPrice:
                    pattern = "((^|\\s+)[0-9]{1,}\\.[0-9]{1,}(\\s+|$))";
                    break;
            }
            return pattern;
        }

        public double ScanAreaHeight()
        {
            return 0.1;
        }

        public double ScanAreaY()
        {
            double y = 0;
            switch (ScanPosition)
            {
                case ScanPosition.Top:
                    y = 0.25 - ScanAreaHeight() / 2;
                    break;
                case ScanPosition.Center:
                    y = 0.5 - ScanAreaHeight() / 2;
                    break;
                case ScanPosition.Bottom:
                    y = 0.75 - ScanAreaHeight() / 2;
                    break;
            }
            return y;
        }
    }
}
