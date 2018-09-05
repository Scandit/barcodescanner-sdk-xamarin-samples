using System;
using UIKit;

namespace iOSViewBasedMatrixScanSample
{
    public static class NSLayoutConstraintExtensions
    {
        public static NSLayoutConstraint Priority(this NSLayoutConstraint constraint, nint priority)
        {
            constraint.Priority = priority;
            return constraint;
        }
    }
}