using Xamarin.Forms;

namespace ExtendedSample
{
    public partial class SettingsPage : ContentPage
    {
        private Settings _settings;
        public SettingsPage(Settings settings)
        {
            this._settings = settings;

            InitializeComponent();

            Ean13Upc12Cell.On = settings.Ean13Upc12;
            Ean8Cell.On = settings.Ean8;
            UpceCell.On = settings.Upce;
            TwoDigitAddOnCell.On = settings.TwoDigitAddOn;
            FiveDigitAddOnCell.On = settings.FiveDigitAddOn;
            Code11Cell.On = settings.Code11;
            Code25Cell.On = settings.Code25;
            Code39Cell.On = settings.Code39;
            Code93Cell.On = settings.Code93;
            Code128Cell.On = settings.Code128;
            Interleaved2Of5Cell.On = settings.Interleaved2Of5;
            MsiPlesseyCell.On = settings.MsiPlessey;
            Gs1DatabarCell.On = settings.Gs1Databar;
            Gs1DatabarExpandedCell.On = settings.Gs1DatabarExpanded;
            Gs1DatabarLimitedCell.On = settings.Gs1DatabarLimited;
            CodebarCell.On = settings.Codebar;
            QrCell.On = settings.Qr;
            QrInvertedCell.On = settings.QrInverted;
            DataMatrixCell.On = settings.DataMatrix;
            DataMatrixInvertedCell.On = settings.DataMatrixInverted;
            Pdf417Cell.On = settings.Pdf417;
            MicroPdf417Cell.On = settings.MicroPdf417;
            AztecCell.On = settings.Aztec;
            MaxiCodeCell.On = settings.MaxiCode;
            Rm4sccCell.On = settings.Rm4scc;
            KixCell.On = settings.Kix;
            RestrictScanningAreaCell.On = settings.RestrictScanningArea;
            HotSpotHeightSlider.Value = settings.HotSpotHeight;
            HotSpotWidthSlider.Value = settings.HotSpotWidth;
            HotSpotYSlider.Value = settings.HotSpotY;
            GuiStylePicker.SelectedItem = settings.GuiStyle;
            ViewFinderPortraitWidth.Value = settings.ViewFinderPortraitWidth;
            ViewFinderPortraitHeight.Value = settings.ViewFinderPortraitHeight;
            ViewFinderLandscapeWidth.Value = settings.ViewFinderLandscapeWidth;
            ViewFinderLandscapeHeight.Value = settings.ViewFinderLandscapeHeight;
            BeepCell.On = settings.Beep;
            VibrateCell.On = settings.Vibrate;
            TorchButtonCell.On = settings.TorchButtonVisible;
            TorchLeftMargin.Value = settings.TorchLeftMargin;
            TorchTopMargin.Value = settings.TorchTopMargin;
            CameraButtonPicker.SelectedItem = settings.CameraButton;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

			_settings.Ean13Upc12 = Ean13Upc12Cell.On;
			_settings.Ean8 = Ean8Cell.On;
			_settings.Upce = UpceCell.On;
			_settings.TwoDigitAddOn = TwoDigitAddOnCell.On;
			_settings.FiveDigitAddOn = FiveDigitAddOnCell.On;
			_settings.Code11 = Code11Cell.On;
			_settings.Code25 = Code25Cell.On;
			_settings.Code39 = Code39Cell.On;
			_settings.Code93 = Code93Cell.On;
			_settings.Code128 = Code128Cell.On;
			_settings.Interleaved2Of5 = Interleaved2Of5Cell.On;
			_settings.MsiPlessey = MsiPlesseyCell.On;
			_settings.Gs1Databar = Gs1DatabarCell.On;
			_settings.Gs1DatabarExpanded = Gs1DatabarExpandedCell.On;
			_settings.Gs1DatabarLimited = Gs1DatabarLimitedCell.On;
			_settings.Codebar = CodebarCell.On;
			_settings.Qr = QrCell.On;
			_settings.QrInverted = QrInvertedCell.On;
			_settings.DataMatrix = DataMatrixCell.On;
			_settings.DataMatrixInverted = DataMatrixInvertedCell.On;
			_settings.Pdf417 = Pdf417Cell.On;
			_settings.MicroPdf417 = MicroPdf417Cell.On;
			_settings.Aztec = AztecCell.On;
			_settings.MaxiCode = MaxiCodeCell.On;
			_settings.Rm4scc = Rm4sccCell.On;
			_settings.Kix = KixCell.On;
			_settings.RestrictScanningArea = RestrictScanningAreaCell.On;
			_settings.HotSpotHeight = HotSpotHeightSlider.Value;
			_settings.HotSpotWidth = HotSpotWidthSlider.Value;
			_settings.HotSpotY = HotSpotYSlider.Value;
            _settings.GuiStyle = GuiStylePicker.SelectedItem.ToString();
			_settings.ViewFinderPortraitWidth = ViewFinderPortraitWidth.Value;
			_settings.ViewFinderPortraitHeight = ViewFinderPortraitHeight.Value;
			_settings.ViewFinderLandscapeWidth = ViewFinderLandscapeWidth.Value;
			_settings.ViewFinderLandscapeHeight = ViewFinderLandscapeHeight.Value;
			_settings.Beep = BeepCell.On;
			_settings.Vibrate = VibrateCell.On;
			_settings.TorchButtonVisible = TorchButtonCell.On;
			_settings.TorchLeftMargin = TorchLeftMargin.Value;
			_settings.TorchTopMargin = TorchTopMargin.Value;
			_settings.CameraButton = CameraButtonPicker.SelectedItem.ToString();

			SettingsArchiver.ArchiveSettings(_settings);
		}
    }
}
