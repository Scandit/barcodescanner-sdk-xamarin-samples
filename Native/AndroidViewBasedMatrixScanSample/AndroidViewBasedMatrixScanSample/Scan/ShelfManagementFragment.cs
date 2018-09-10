using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using ScanditBarcodePicker.Android.Matrixscan;
using ScanditBarcodePicker.Android.Recognition;

namespace AndroidViewBasedMatrixScanSample.Scan
{
    /*
     * Main Fragment of the AndroidViewBasedMatrixScanSample. It prepares and manages the view of the app,
     * as well as controls the matrix scan events/callbacks via the MatrixScanOverlayListener.
     */
    public class ShelfManagementFragment : ScanFragment
    {
        MatrixScanOverlayListener matrixScanListener;

        bool frozen = false;
        FrameLayout pickerContainer;
        Button freezeButton;
        TextView detail;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            InitializePicker();

            matrixScanListener = new MatrixScanOverlayListener(Context, BubbleClick, picker);
            var matrixScan = new MatrixScan(picker, matrixScanListener);

            var viewBasedOverlay =
                new ViewBasedMatrixScanOverlay(Context, matrixScan, matrixScanListener);
            matrixScan.AddOverlay(new SimpleMatrixScanOverlay(Context, matrixScan, matrixScanListener));
            matrixScan.AddOverlay(viewBasedOverlay);


            // You can enable beeping via:
            matrixScan.BeepOnNewCode = true;

            matrixScanListener.SetViewBasedMatrixScanOverlay(viewBasedOverlay);

            var view = inflater.Inflate(Resource.Layout.ShelfManagementFragment, null);
            pickerContainer = view.FindViewById<FrameLayout>(Resource.Id.picker);
            pickerContainer.AddView(picker);
            freezeButton = view.FindViewById<Button>(Resource.Id.freeze_button);
            freezeButton.Click += (sender, e) => Freeze();
            detail = view.FindViewById<TextView>(Resource.Id.detail);
            detail.Click += (sender, e) => CloseDetail();

            return view;
        }

        public override void OnResume()
        {
            base.OnResume();

            var bar = ((AppCompatActivity)Context).SupportActionBar;
            if (bar != null)
            {
                bar.Hide();
            }
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            pickerContainer.RemoveAllViews();
        }

        void BubbleClick(TrackedBarcode barcode)
        {
            detail.Text = barcode.Data;
            detail.Visibility = ViewStates.Visible;
        }

        void CloseDetail()
        {
            detail.Visibility = ViewStates.Invisible;
        }

        void Freeze()
        {
            if (frozen)
            {
                frozen = false;
                SetScanState(ScanState.SCANNING);
                freezeButton.Text = GetString(Resource.String.sm_freeze);
            }
            else
            {
                frozen = true;
                SetScanState(ScanState.STOPPED);
                freezeButton.Text = GetString(Resource.String.sm_done);
            }
        }
    }
}
