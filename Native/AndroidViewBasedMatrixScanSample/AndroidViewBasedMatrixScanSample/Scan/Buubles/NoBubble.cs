using Android.Content;
using Android.Views;

namespace AndroidViewBasedMatrixScanSample.Scan.Buubles
{
    public class NoBubble : BaseBubble
    {
        public NoBubble(Context context, BubbleData bubbleData) : base(context, bubbleData)
        {
        }

        public override int GetHeight()
        {
            return 0;
        }

        public override int GetWidth()
        {
            return 0;
        }

        public override View GetView(Context context, LayoutInflater inflater)
        {
            return null;
        }
    }
}
