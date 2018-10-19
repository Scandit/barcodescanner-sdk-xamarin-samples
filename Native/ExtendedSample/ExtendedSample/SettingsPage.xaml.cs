using Xamarin.Forms;

namespace ExtendedSample
{
    public partial class SettingsPage : ContentPage
    {
        private Settings settings;

        public SettingsPage()
        {
            InitializeComponent();
        }

        public SettingsPage(Settings settings)
        {
            this.settings = settings;

            InitializeComponent();

            setupTableView();
        }

        private void setupTableView()
        {
            ContinuousAfterScan.On = settings.ContinuousAfterScan;
            RotationWithDevice.On = settings.RotationWithDevice;
            Ean13Upc12Cell.On = settings.Ean13Upc12;
            Ean8Cell.On = settings.Ean8;
            UpceCell.On = settings.Upce;
            TwoDigitAddOnCell.On = settings.TwoDigitAddOn;
            FiveDigitAddOnCell.On = settings.FiveDigitAddOn;
            Code11Cell.On = settings.Code11;
            Code25Cell.On = settings.Code25;
            Code32Cell.On = settings.Code32;
            Code39Cell.On = settings.Code39;
            Code93Cell.On = settings.Code93;
            Code128Cell.On = settings.Code128;
            Interleaved2Of5Cell.On = settings.Interleaved2Of5;
            MsiPlesseyCell.On = settings.MsiPlessey;
            Gs1DatabarCell.On = settings.Gs1Databar;
            Gs1DatabarExpandedCell.On = settings.Gs1DatabarExpanded;
            Gs1DatabarLimitedCell.On = settings.Gs1DatabarLimited;
            CodebarCell.On = settings.Codabar;
            QrCell.On = settings.Qr;
            QrInvertedCell.On = settings.QrInverted;

            DataMatrixCell.On = settings.DataMatrix;
            DataMatrixCell.OnChanged += (object sender, ToggledEventArgs e) => {
                DataMatrixInvertedCell.IsEnabled = DataMatrixCell.On;
                DpmModeCell.IsEnabled = DataMatrixCell.On;
            };
            DataMatrixInvertedCell.IsEnabled = settings.DataMatrix;
            DataMatrixInvertedCell.On = settings.DataMatrixInverted;
            DpmModeCell.IsEnabled = settings.DataMatrix;
            DpmModeCell.On = settings.DpmMode;

            Pdf417Cell.On = settings.Pdf417;
            MicroPdf417Cell.On = settings.MicroPdf417;
            AztecCell.On = settings.Aztec;
            MaxiCodeCell.On = settings.MaxiCode;
            Rm4sccCell.On = settings.Rm4scc;
            KixCell.On = settings.Kix;
            MicroQRCell.On = settings.MicroQR;
            Lapa4scCell.On = settings.Lapa4sc;
            RestrictScanningAreaCell.On = settings.RestrictScanningArea;
            HotSpotHeightSlider.Value = settings.HotSpotHeight;
            HotSpotWidthSlider.Value = settings.HotSpotWidth;
            HotSpotYSlider.Value = settings.HotSpotY;
            GuiStylePicker.SelectedIndex = (int)settings.GuiStyle;
            ViewFinderPortraitWidth.Value = settings.ViewFinderPortraitWidth;
            ViewFinderPortraitHeight.Value = settings.ViewFinderPortraitHeight;
            ViewFinderLandscapeWidth.Value = settings.ViewFinderLandscapeWidth;
            ViewFinderLandscapeHeight.Value = settings.ViewFinderLandscapeHeight;
            BeepCell.On = settings.Beep;
            VibrateCell.On = settings.Vibrate;
            TorchButtonCell.On = settings.TorchButtonVisible;
            TorchLeftMargin.Value = settings.TorchLeftMargin;
            TorchTopMargin.Value = settings.TorchTopMargin;
            CameraButtonPicker.SelectedIndex = (int)settings.CameraButton;
            ResolutionPicker.SelectedIndex = (int)settings.Resolution;
        }

        public void UpdateSettings()
        {
            settings.RotationWithDevice = RotationWithDevice.On;
            settings.ContinuousAfterScan = ContinuousAfterScan.On;
            settings.Ean13Upc12 = Ean13Upc12Cell.On;
            settings.Ean8 = Ean8Cell.On;
            settings.Upce = UpceCell.On;
            settings.TwoDigitAddOn = TwoDigitAddOnCell.On;
            settings.FiveDigitAddOn = FiveDigitAddOnCell.On;
            settings.Code11 = Code11Cell.On;
            settings.Code25 = Code25Cell.On;
            settings.Code32 = Code32Cell.On;
            settings.Code39 = Code39Cell.On;
            settings.Code93 = Code93Cell.On;
            settings.Code128 = Code128Cell.On;
            settings.Interleaved2Of5 = Interleaved2Of5Cell.On;
            settings.MsiPlessey = MsiPlesseyCell.On;
            settings.Gs1Databar = Gs1DatabarCell.On;
            settings.Gs1DatabarExpanded = Gs1DatabarExpandedCell.On;
            settings.Gs1DatabarLimited = Gs1DatabarLimitedCell.On;
            settings.Codabar = CodebarCell.On;
            settings.Qr = QrCell.On;
            settings.QrInverted = QrInvertedCell.On;
            settings.DataMatrix = DataMatrixCell.On;
            settings.DataMatrixInverted = DataMatrixInvertedCell.On;
            settings.DpmMode = DpmModeCell.On;
            settings.Pdf417 = Pdf417Cell.On;
            settings.MicroPdf417 = MicroPdf417Cell.On;
            settings.Aztec = AztecCell.On;
            settings.MaxiCode = MaxiCodeCell.On;
            settings.Rm4scc = Rm4sccCell.On;
            settings.Kix = KixCell.On;
            settings.MicroQR = MicroQRCell.On;
            settings.Lapa4sc = Lapa4scCell.On;
            settings.RestrictScanningArea = RestrictScanningAreaCell.On;
            settings.HotSpotHeight = HotSpotHeightSlider.Value;
            settings.HotSpotWidth = HotSpotWidthSlider.Value;
            settings.HotSpotY = HotSpotYSlider.Value;
            settings.GuiStyle = (GuiStyle)GuiStylePicker.SelectedIndex;
            settings.ViewFinderPortraitWidth = ViewFinderPortraitWidth.Value;
            settings.ViewFinderPortraitHeight = ViewFinderPortraitHeight.Value;
            settings.ViewFinderLandscapeWidth = ViewFinderLandscapeWidth.Value;
            settings.ViewFinderLandscapeHeight = ViewFinderLandscapeHeight.Value;
            settings.Beep = BeepCell.On;
            settings.Vibrate = VibrateCell.On;
            settings.TorchButtonVisible = TorchButtonCell.On;
            settings.TorchLeftMargin = TorchLeftMargin.Value;
            settings.TorchTopMargin = TorchTopMargin.Value;
            settings.CameraButton = (CameraButton)CameraButtonPicker.SelectedIndex;
            settings.Resolution = (Resolution)ResolutionPicker.SelectedIndex;
        }

        protected void OnResetSettings(object sender, System.EventArgs e)
        {
            settings.ResetSettings();
            setupTableView();
        }

        protected void OnOpenOnlineDocumentation(object sender, System.EventArgs e)
        {
            DependencyService.Get<IUrlOpener>().OpenUrl("http://docs.scandit.com");
        }
    }
}
