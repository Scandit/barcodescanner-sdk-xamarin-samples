using System;

namespace ExtendedSample
{
    public enum GuiStyle
    {
        Frame, Laser, None, MatrixScan, LocationsOnly
    };

    public enum CameraButton
    {
        Never, OnTablet, Always
    };

    public enum Resolution
    {
        Standard, HD
    };

    public class Settings
    {
        // General
        public bool RotationWithDevice { get; set; }
        public bool ContinuousAfterScan { get; set; }

        // Symbologies
        public bool Ean13Upc12 { get; set; }
        public bool Ean8 { get; set; }
        public bool Upce { get; set; }
        public bool TwoDigitAddOn { get; set; }
        public bool FiveDigitAddOn { get; set; }
        public bool Code11 { get; set; }
        public bool Code25 { get; set; }
        public bool Code32 { get; set; }
        public bool Code39 { get; set; }
        public bool Code93 { get; set; }
        public bool Code128 { get; set; }
        public bool Interleaved2Of5 { get; set; }
        public bool MsiPlessey { get; set; }
        public bool Gs1Databar { get; set; }
        public bool Gs1DatabarExpanded { get; set; }
        public bool Gs1DatabarLimited { get; set; }
        public bool Codabar { get; set; }
        public bool Qr { get; set; }
        public bool QrInverted { get; set; }
        public bool DataMatrix { get; set; }
        public bool DataMatrixInverted { get; set; }
        public bool DpmMode { get; set; }
        public bool Pdf417 { get; set; }
        public bool MicroPdf417 { get; set; }
        public bool Aztec { get; set; }
        public bool MaxiCode { get; set; }
        public bool Rm4scc { get; set; }
        public bool Kix { get; set; }
        public bool DotCode { get; set; }
        public bool MicroQR { get; set; }

        // Scanning Area
        public bool RestrictScanningArea { get; set; }
        public double HotSpotHeight { get; set; }
        public double HotSpotWidth { get; set; }
        public double HotSpotY { get; set; }

        // View Finder
        public GuiStyle GuiStyle { get; set; }
        public double ViewFinderPortraitWidth { get; set; }
        public double ViewFinderPortraitHeight { get; set; }
        public double ViewFinderLandscapeWidth { get; set; }
        public double ViewFinderLandscapeHeight { get; set; }

        // Feedback
        public bool Beep { get; set; }
        public bool Vibrate { get; set; }

        // Button Visibility
        public bool TorchButtonVisible { get; set; }
        public double TorchLeftMargin { get; set; }
        public double TorchTopMargin { get; set; }
        public CameraButton CameraButton { get; set; }

        // Camera
        public Resolution Resolution { get; set; }

        public Settings()
        {
            ResetSettings();
        }

        public void ResetSettings()
        {
            RotationWithDevice = true;
            ContinuousAfterScan = true;
            Ean13Upc12 = true;
            Ean8 = true;
            Upce = true;
            TwoDigitAddOn = false;
            FiveDigitAddOn = false;
            Code11 = false;
            Code25 = false;
            Code32 = false;
            Code39 = false;
            Code93 = false;
            Code128 = false;
            Interleaved2Of5 = false;
            MsiPlessey = false;
            Gs1Databar = false;
            Gs1DatabarExpanded = false;
            Gs1DatabarLimited = false;
            Codabar = false;
            Qr = true;
            QrInverted = false;
            DataMatrix = false;
            DataMatrixInverted = false;
            DpmMode = false;
            Pdf417 = false;
            MicroPdf417 = false;
            Aztec = false;
            MaxiCode = false;
            Rm4scc = false;
            Kix = false;
            DotCode = false;
            RestrictScanningArea = false;
            HotSpotHeight = 0.25F;
            HotSpotWidth = 1.0F;
            HotSpotY = 0.45F;
            GuiStyle = GuiStyle.Frame;
            ViewFinderPortraitWidth = 0.9F;
            ViewFinderPortraitHeight = 0.5F;
            ViewFinderLandscapeWidth = 0.4F;
            ViewFinderLandscapeHeight = 0.4F;
            Beep = true;
            Vibrate = true;
            TorchButtonVisible = true;
            TorchLeftMargin = 15;
            TorchTopMargin = 15;
            CameraButton = CameraButton.Always;
            Resolution = Resolution.Standard;
        }
    }
}
