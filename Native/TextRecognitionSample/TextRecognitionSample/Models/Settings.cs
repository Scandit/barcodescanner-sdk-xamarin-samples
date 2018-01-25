using System;

namespace TextRecognitionSample
{
    public enum Mode 
    {
        IBAN, GS1AI, ScanPrice    
    }

    public enum Position
    {
        Top, Center, Bottom
    }

    public class Settings
    {
        public Mode Mode { get; set; }
        public Position Position { get; set; }
    }
}
