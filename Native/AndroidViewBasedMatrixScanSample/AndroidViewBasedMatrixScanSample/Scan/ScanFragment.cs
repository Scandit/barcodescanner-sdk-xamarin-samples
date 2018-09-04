using ScanditBarcodePicker.Android;

namespace AndroidViewBasedMatrixScanSample.Scan
{
    /*
     * A Fragment responsible for barcode scanning, preparing and setting the ScanSettings,
     * controlling scanning flow (stopping and resuming scanning when necessary), etc.
     */
    public abstract class ScanFragment : CameraPermissionFragment
    {
        enum ScanState
        {
            STOPPED, SCANNING
        }

        ScanState scanState = ScanState.STOPPED;

        protected BarcodePicker picker;

    }
}
