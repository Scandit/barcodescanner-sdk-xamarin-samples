using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using AndroidViewBasedMatrixScanSample.Scan.Bubbles;
using AndroidViewBasedMatrixScanSample.Utility;
using ScanditBarcodePicker.Android;
using ScanditBarcodePicker.Android.Matrixscan;
using ScanditBarcodePicker.Android.Recognition;

namespace AndroidViewBasedMatrixScanSample.Scan
{
    /*
     * A class implementing 3 interfaces related to matrix scan:
     * - MatrixScanListener - an interface used by the MatrixScan to control the processing of
     * tracked barcodes,
     * - SimpleMatrixScanOverlay.SimpleMatrixScanOverlayListener - an interface required by the
     * SimpleMatrixScanOverlay providing user basic control over the overlay,
     * - ViewBasedMatrixScanOverlay.ViewBasedMatrixScanOverlayListener - an interface required by the
     * ViewBasedMatrixScanOverlay providing user basic control over the overlay.
     */
    public class MatrixScanOverlayListener
        : Java.Lang.Object, SimpleMatrixScanOverlay.ISimpleMatrixScanOverlayListener, ViewBasedMatrixScanOverlay.IViewBasedMatrixScanOverlayListener, IMatrixScanListener
    {
        const float MINIMISED_RATIO = 0.1f;
        const float MAXIMISED_RATIO = 0.25f;
        const int MAX_INDICATOR_NUMBER = 10;

        readonly Context context;
        readonly Action<TrackedBarcode> bubbleClickAction;
        readonly BarcodePicker picker;

        ViewBasedMatrixScanOverlay viewBasedMatrixScanOverlay;

        IndicatorState lastIndicatorState = IndicatorState.MINIMISED;
        IndicatorViewModelFactory bubbleFactory;
        ConcurrentDictionary<long, BaseBubble> bubbles = new ConcurrentDictionary<long, BaseBubble>();
        float screenWidth;

        public MatrixScanOverlayListener(Context context, Action<TrackedBarcode> bubbleClickAction, BarcodePicker picker)
        {
            this.context = context;
            this.bubbleClickAction = bubbleClickAction;
            this.picker = picker;

            var size = new Point();
            (context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>())
                .DefaultDisplay.GetSize(size);
            screenWidth = size.X;
            bubbleFactory = new IndicatorViewModelFactory(context);
        }

        public void SetViewBasedMatrixScanOverlay(ViewBasedMatrixScanOverlay viewBasedMatrixScanOverlay)
        {
            this.viewBasedMatrixScanOverlay = viewBasedMatrixScanOverlay;
        }

        IndicatorState CalculateIndicatorState(IDictionary<Java.Lang.Long, TrackedBarcode> recognizedCodes)
        {
            List<int> list = new List<int>();
            foreach (var code in recognizedCodes.Values)
            {
                Quadrilateral convertedLocation = ConvertQuadrilateral(code.Location);
                list.Add(convertedLocation.TopRight.X - convertedLocation.TopLeft.X);
            }
            list.Sort();

            float indicatorSizeRatio = list[recognizedCodes.Count / 2] / screenWidth;

            if (recognizedCodes.Count > MAX_INDICATOR_NUMBER)
            {
                return IndicatorState.HIGHLIGHT_ONLY;
            }

            if (indicatorSizeRatio > MAXIMISED_RATIO)
            {
                return IndicatorState.MAXIMISED;
            }

            if (indicatorSizeRatio > MINIMISED_RATIO)
            {
                return IndicatorState.MINIMISED;
            }

            return IndicatorState.HIGHLIGHT_ONLY;
        }

        Quadrilateral ConvertQuadrilateral(Quadrilateral quadrilateral)
        {
            return new Quadrilateral(picker.ConvertPointToPickerCoordinates(quadrilateral.TopLeft),
                                     picker.ConvertPointToPickerCoordinates(quadrilateral.TopRight),
                                     picker.ConvertPointToPickerCoordinates(quadrilateral.BottomLeft),
                                     picker.ConvertPointToPickerCoordinates(quadrilateral.BottomRight));
        }

        public int GetColorForCode(TrackedBarcode barcode, long trackingId)
        {
            return bubbles[trackingId].highlightColor;
        }

        public Point GetOffsetForCode(TrackedBarcode barcode, long trackingId)
        {
            BaseBubble indicator = bubbles[trackingId];

            return new Point(-UiUtils.PxFromDp(context, indicator.GetWidth()) / 2,
                    -UiUtils.PxFromDp(context, indicator.GetHeight()));
        }

        public View GetViewForCode(TrackedBarcode barcode, long trackingId)
        {
            LayoutInflater inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            return bubbles[trackingId].GetView(context, inflater);
        }

        public void MatrixScan(MatrixScan matrixScan, Frame didUpdate)
        {
            if (didUpdate.TrackedCodes.Count == 0)
            {
                return;
            }

            IndicatorState indicatorState = CalculateIndicatorState(didUpdate.TrackedCodes);
            if (lastIndicatorState != indicatorState)
            {
                bubbles.Clear();
                if (indicatorState != IndicatorState.HIGHLIGHT_ONLY)
                {
                    foreach (var it in didUpdate.TrackedCodes)
                    {
                        var bubble = bubbleFactory.CreateBubble(indicatorState, it.Value);
                        bubbles.AddOrUpdate(
                            it.Key.LongValue(),
                            bubble,
                            (arg1, arg2) => bubble);

                        viewBasedMatrixScanOverlay.SetOffsetForCode(GetOffsetForCode(it.Value, it.Key.LongValue()), it.Key.LongValue());
                        viewBasedMatrixScanOverlay.Post(
                            () => viewBasedMatrixScanOverlay.SetViewForCode(GetViewForCode(it.Value, it.Key.LongValue()), it.Key.LongValue()));
                    }
                }
            }
            lastIndicatorState = indicatorState;

            foreach (var id in didUpdate.AddedIdentifiers)
            {
                if (didUpdate.TrackedCodes.ContainsKey(id))
                {
                    var bubble = bubbleFactory.CreateBubble(indicatorState, didUpdate.TrackedCodes[id]);
                    bubbles.AddOrUpdate(
                        id.LongValue(),
                        bubble,
                        (arg1, arg2) => bubble);
                }
            }

            foreach (var id in didUpdate.RemovedIdentifiers)
            {
                ((IDictionary<long, BaseBubble>)bubbles).Remove(id.LongValue());
            }
        }

        public void OnCodeTouched(TrackedBarcode barcode, long trackingId)
        {
            bubbleClickAction(barcode);
        }

        public bool ShouldRejectCode(MatrixScan matrixScan, TrackedBarcode barcode)
        {
            return false;
        }
    }
}
