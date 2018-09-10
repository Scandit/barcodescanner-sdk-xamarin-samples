using Android;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;

namespace AndroidViewBasedMatrixScanSample.Scan
{
    /*
     * Abstract class inheriting from Fragment, that provides the camera permission handling.
     */
    public abstract class CameraPermissionFragment : Fragment
    {
        const int CAMERA_PERMISSION_REQUEST = 0;

        bool deniedCameraAccess = false;
        bool paused = true;

        public override void OnPause()
        {
            base.OnPause();
            paused = false;
        }

        protected bool HasCameraPermission()
        {
            return Build.VERSION.SdkInt < BuildVersionCodes.M
                        || Context.CheckSelfPermission(Manifest.Permission.Camera) == Permission.Granted;
        }

        protected void RequestCameraPermission()
        {
            // Handle permissions for Marshmallow and onwards...
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M
                && Context.CheckSelfPermission(Manifest.Permission.Camera) != Permission.Granted)
            {
                if (!deniedCameraAccess)
                {
                    // It's pretty clear for why the camera is required. We don't need to give a
                    // detailed reason.
                    RequestPermissions(new string[] { Manifest.Permission.Camera },
                            CAMERA_PERMISSION_REQUEST);
                }
            }
            else
            {
                // We already have the permission or don't need it.
                OnCameraPermissionGranted();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
                                                        [GeneratedEnum] Permission[] grantResults)
        {
            if (requestCode == CAMERA_PERMISSION_REQUEST)
            {
                if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
                {
                    deniedCameraAccess = false;
                    if (!paused)
                    {
                        OnCameraPermissionGranted();
                    }
                }
                else
                {
                    deniedCameraAccess = true;
                }
                return;
            }
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public abstract void OnCameraPermissionGranted();
    }
}
