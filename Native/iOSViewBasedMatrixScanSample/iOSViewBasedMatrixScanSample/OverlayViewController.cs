using CoreGraphics;
using UIKit;

namespace iOSViewBasedMatrixScanSample
{
    public class OverlayViewController : UIViewController
    {
        private readonly UILabel codeDataLabel = new UILabel(CGRect.Empty);

        private Model _model;
        public Model Model
        {
            get => _model;
            set
            {
                _model = value;
                codeDataLabel.Text = _model.Code;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.AddSubview(codeDataLabel);
            codeDataLabel.TextColor = UIColor.White;
            codeDataLabel.TextAlignment = UITextAlignment.Center;
            codeDataLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            View.AddConstraints(new []
            {
                codeDataLabel.CenterYAnchor.ConstraintEqualTo(View.CenterYAnchor),
                codeDataLabel.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor),
                codeDataLabel.LeadingAnchor.ConstraintLessThanOrEqualTo(View.LeadingAnchor, 83),
                codeDataLabel.TopAnchor.ConstraintLessThanOrEqualTo(View.TopAnchor, 278)
            });
            View.BackgroundColor = UIColor.Black;
            View.Alpha = 0.8f;
            View.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                DismissViewController(false, null);
            }));
        }
    }
}