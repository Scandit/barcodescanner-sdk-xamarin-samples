using Android.Content;
using Android.Support.V4.Content;
using Android.Views;

namespace AndroidViewBasedMatrixScanSample.Scan.Buubles
{
    public abstract class BaseBubble
    {
        Context context;
        int greenThreshold;
        int yellowThreshold;

        public int highlightColor;

        public BaseBubble(Context context, BubbleData bubbleData) : this(context, bubbleData, 15, 2)
        {}

        public BaseBubble(Context context, BubbleData bubbleData, int greenThreshold, int yellowThreshold)
        {
            this.context = context;
            this.greenThreshold = greenThreshold;
            this.yellowThreshold = yellowThreshold;

            highlightColor = GetHighlightColor(bubbleData.Stock);
        }

        int GetHighlightColor(int inStock)
        {
            if (inStock > greenThreshold)
            {
                return ContextCompat.GetColor(context, Resource.Color.transparentGreen);
            }

            if (inStock > yellowThreshold)
            {
                return ContextCompat.GetColor(context, Resource.Color.transparentYellow);
            }

            return ContextCompat.GetColor(context, Resource.Color.transparentRed);
        }

        public abstract int GetWidth();

        public abstract int GetHeight();

        public abstract View GetView(Context context, LayoutInflater inflater);
    }
}
