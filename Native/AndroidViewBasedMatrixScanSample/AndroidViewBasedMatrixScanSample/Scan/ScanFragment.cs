using ScanditBarcodePicker.Android;
using ScanditBarcodePicker.Android.Recognition;

namespace AndroidViewBasedMatrixScanSample.Scan
{
    /*
     * A Fragment responsible for barcode scanning, preparing and setting the ScanSettings,
     * controlling scanning flow (stopping and resuming scanning when necessary), etc.
     */
    public abstract class ScanFragment : CameraPermissionFragment
    {
        protected enum ScanState
        {
            STOPPED, SCANNING
        }

        ScanState scanState = ScanState.STOPPED;

        protected BarcodePicker picker;

        protected void InitializePicker()
        {
            if (picker == null)
            {
                picker = new BarcodePicker(Context, GetScanSettings());
            }
        }

        public override void OnStart()
        {
            base.OnStart();
            InitializePicker();

            picker.ApplyScanSettings(GetScanSettings());
            var view = View;
            if (view != null)
            {
                view.Post(() => {
                    UpdateScanUi(picker);
                });
            }

            SetScanState(ScanState.SCANNING);
        }

        public override void OnStop()
        {
            base.OnStop();
            SetScanState(ScanState.STOPPED);
        }

        public override void OnResume()
        {
            base.OnResume();
            SetScanState(ScanState.SCANNING);
        }

        public override void OnPause()
        {
            base.OnPause();
            SetScanState(ScanState.STOPPED);
        }

        protected ScanSettings GetScanSettings()
        {
            ScanSettings settings = ScanSettings.Create();
            settings.SetSymbologyEnabled(Barcode.SymbologyEan13, true);
            settings.SetSymbologyEnabled(Barcode.SymbologyUpce, true);
            settings.SetSymbologyEnabled(Barcode.SymbologyUpca, true);
            settings.SetSymbologyEnabled(Barcode.SymbologyEan8, true);
            settings.MatrixScanEnabled = true;
            settings.MaxNumberOfCodesPerFrame = 12;
            settings.HighDensityModeEnabled = true;
            settings.CodeCachingDuration = 0;
            settings.CodeDuplicateFilter = 0;

            return settings;
        }

        protected void UpdateScanUi(BarcodePicker picker)
        {
            InitializePicker();

            picker.OverlayView.SetGuiStyle(ScanOverlay.GuiStyleNone);
            picker.OverlayView.SetTorchEnabled(false);
            picker.OverlayView.SetViewfinderDimension(0.9f, 0.75f, 0.95f, 0.9f);
        }

        protected void SetScanState(ScanState state)
        {
            SetScanState(state, null);
        }

        protected void SetScanState(ScanState state, IScanSession scanSession)
        {
            scanState = state;

            switch (state)
            {
                case ScanState.STOPPED:
                    if (scanSession != null)
                    {
                        scanSession.StopScanning();
                    }
                    else
                    {
                        picker.StopScanning();
                    }
                    break;
                case ScanState.SCANNING:
                    if (HasCameraPermission())
                    {
                        picker.StartScanning();
                    }
                    else
                    {
                        RequestCameraPermission();
                    }
                    break;
            }
        }

        public override void OnCameraPermissionGranted()
        {
            SetScanState(scanState);
        }
    }
}
