using Android.App;
using Android.Widget;
using Android.OS;
using ScanditBarcodePicker.Android;
using ScanditBarcodePicker.Android.Recognition;
using Android.Views;
using Android;
using System;
using Android.Content.PM;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using Android.Graphics;

namespace AndroidSplitViewSample
{
    [Activity(Label = "AndroidSplitViewSample", MainLauncher = true, Icon = "@mipmap/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity, IOnScanListener
    {
        private const string KEY = "-- ENTER YOUR SCANDIT LICENSE KEY HERE --";

        private const int CAMERA_REQUEST = 0;
        private const int TIMER = 13;
        private const int SCAN = 99;

        BarcodePicker barcodePicker;
        View tap;
        Handler handler;
        MyAdapter adapter = new MyAdapter();
        private bool paused = true;

        public void DidScan(IScanSession session)
        {
            StartTimer();

            if (session.NewlyRecognizedCodes.Count > 0) {
                Message message = Message.Obtain();
                message.What = SCAN;
                message.Obj = session.NewlyRecognizedCodes[0];
                handler.SendMessage(message);
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            handler = new MyHandler(this);
            RequestWindowFeature(WindowFeatures.NoTitle);
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

            SetContentView(Resource.Layout.Main);
            tap = FindViewById(Resource.Id.tap);
            tap.Click += delegate {
                StartPicker();
            };
            FindViewById(Resource.Id.clear_button).Click += delegate {
                adapter.Clear();
            };
            RecyclerView recycler = FindViewById(Resource.Id.recycler_view) as RecyclerView;
            recycler.SetLayoutManager(new LinearLayoutManager(this));
            recycler.SetAdapter(adapter);

            ScanditLicense.AppKey = KEY;
            InitializeBarcodeScanning();
        }

        protected override void OnResume()
        {
            base.OnResume();

            paused = false;
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                GrantCameraPermissionsThenStartScanning();
            }
            else
            {
                barcodePicker.StartScanning();
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
            GC.Collect();
            barcodePicker.StopScanning();
            paused = true;
        }

        void GrantCameraPermissionsThenStartScanning()
        {
            if (CheckSelfPermission(Manifest.Permission.Camera) != (int)Permission.Granted)
            {
                RequestPermissions(new String[] { Manifest.Permission.Camera }, CAMERA_REQUEST);
            }
            else
            {
                StartPicker();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode == CAMERA_REQUEST)
            {
                if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
                {
                    if (!paused)
                    {
                        StartPicker();
                    }
                }
                return;
            }
        }

        void StartTimer()
        {
            handler.RemoveMessages(TIMER);
            handler.SendEmptyMessageDelayed(TIMER, 5000);
        }

        void StartPicker()
        {
            tap.Visibility = ViewStates.Gone;
            barcodePicker.StartScanning();
            StartTimer();
        }

        void InitializeBarcodeScanning()
        {
            ScanSettings settings = ScanSettings.Create();
            int[] symbologiesToEnable = {
                Barcode.SymbologyEan13,
                Barcode.SymbologyEan8,
                Barcode.SymbologyUpca,
                Barcode.SymbologyCode39,
                Barcode.SymbologyCode128,
                Barcode.SymbologyInterleaved2Of5,
                Barcode.SymbologyUpce
            };

            ScanAreaSettings scanAreaSettings = new ScanAreaSettings();
            scanAreaSettings.SquareCodeLocationConstraint = BarcodeScannerSettings.CodeLocationRestrict;
            scanAreaSettings.WideCodeLocationConstraint = BarcodeScannerSettings.CodeLocationRestrict;
            scanAreaSettings.WideCodeLocationArea = new RectF(0f, 0.475f, 1f, 0.525f);
            scanAreaSettings.SquareCodeLocationArea = new RectF(0f, 0.3f, 1f, 0.7f);
            settings.AreaSettingsPortrait = scanAreaSettings;
            settings.AreaSettingsLandscape = scanAreaSettings;
            settings.CodeDuplicateFilter = 1000;

            foreach (int symbology in symbologiesToEnable)
            {
                settings.SetSymbologyEnabled(symbology, true);
            }

            barcodePicker = new BarcodePicker(this, settings);
            barcodePicker.OverlayView.SetGuiStyle(ScanOverlay.GuiStyleLaser);
            barcodePicker.SetOnScanListener(this);
            (FindViewById(Resource.Id.picker_container) as FrameLayout).AddView(barcodePicker, 0);
        }

        class MyHandler : Handler
        {
            private readonly MainActivity activity;

            public MyHandler(MainActivity activity) 
            {
                this.activity = activity;
            }

            public override void HandleMessage(Message msg)
            {
                switch (msg.What)
                {
                    case TIMER:
                        activity.barcodePicker.StopScanning();
                        activity.tap.Visibility = ViewStates.Visible;
                        break;
                    case SCAN:
                        activity.adapter.Add(msg.Obj as Barcode);
                        break;
                }
            }
        }

        class MyAdapter : RecyclerView.Adapter
        {
            private readonly List<Barcode> barcodes = new List<Barcode>();

            public override int ItemCount => barcodes.Count;

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                (holder as MyHolder).data.Text = barcodes[position].Data;
                (holder as MyHolder).symbology.Text = barcodes[position].SymbologyName;
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ResultItem, parent, false);
                return new MyHolder(view);
            }

            public void Add(Barcode barcode)
            {
                barcodes.Insert(0, barcode);
                NotifyDataSetChanged();
            }

            public void Clear()
            {
                barcodes.Clear();
                NotifyDataSetChanged();
            }
        }

        class MyHolder : RecyclerView.ViewHolder
        {
            public readonly TextView data;
            public readonly TextView symbology;
            
            public MyHolder(View itemView) : base(itemView)
            {
                data = itemView.FindViewById(Resource.Id.data) as TextView;
                symbology = itemView.FindViewById(Resource.Id.symbology) as TextView;
            }
        }
    }
}

