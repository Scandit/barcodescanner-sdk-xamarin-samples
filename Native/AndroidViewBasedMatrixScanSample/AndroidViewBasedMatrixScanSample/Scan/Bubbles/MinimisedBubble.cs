using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V4.Content;
using AndroidViewBasedMatrixScanSample.Utility;

namespace AndroidViewBasedMatrixScanSample.Scan.Bubbles
{
    public class MinimisedBubble : BaseBubble
    {

        const int WIDTH = 52;
        const int HEIGHT = 45;

        readonly BubbleData bubbleData;

        public MinimisedBubble(Context context, BubbleData bubbleData) : base(context, bubbleData)
        {
            this.bubbleData = bubbleData;
        }

        public override int GetHeight()
        {
            return HEIGHT;
        }

        public override int GetWidth()
        {
            return WIDTH;
        }

        public override View GetView(Context context, LayoutInflater inflater)
        {
            var view = inflater.Inflate(Resource.Layout.BubbleMinimised, null);

            // XXX A vector drawable has to be loaded with the AppCompatResources.GetDrawable method
            // and then added programatically in order to avoid InflateException.
            view.FindViewById(Resource.Id.triangle).Background = ContextCompat.GetDrawable(context, Resource.Drawable.bg_triangle);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                view.FindViewById(Resource.Id.header).BackgroundTintList = ColorStateList.ValueOf(new Color(highlightColor));
            }

            ((TextView)view.FindViewById(Resource.Id.stock)).Text = bubbleData.Stock.ToString();
            view.LayoutParameters = new FrameLayout.LayoutParams(
                UiUtils.PxFromDp(context, WIDTH), UiUtils.PxFromDp(context, HEIGHT));

            return view;
        }
    }
}
