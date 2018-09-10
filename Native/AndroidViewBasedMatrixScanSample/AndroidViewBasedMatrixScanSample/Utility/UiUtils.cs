namespace AndroidViewBasedMatrixScanSample.Utility
{
    public static class UiUtils
    {
        public static int PxFromDp(Android.Content.Context context, int dp)
        {
            return (int)(dp * context.Resources.DisplayMetrics.Density + 0.5f);
        }
    }
}
