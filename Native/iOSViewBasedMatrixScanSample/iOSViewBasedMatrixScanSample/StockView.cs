using System;
using CoreGraphics;
using UIKit;

namespace iOSViewBasedMatrixScanSample
{
    public class StockViewClickedEventArgs : EventArgs
    {
        public readonly Model Model;

        internal StockViewClickedEventArgs(Model model)
        {
            Model = model;
        }
    }

    public class StockView : UIView
    {
        internal static float StandardWidth = 140;
        internal static float StandardHeight = 80;

        private UIView contentView = new UIView(CGRect.Empty);
        private UIView topStripeView = new UIView(CGRect.Empty);
        private UIView bottomStripeView =new UIView(CGRect.Empty);
        private UIView darkView = new UIView(CGRect.Empty);
        private UILabel stockLabel = new UILabel(CGRect.Empty);
        private UILabel stockTextLabel = new UILabel(CGRect.Empty);
        private UILabel deliveryLabel = new UILabel(CGRect.Empty);
        private UILabel deliveryTextLabel = new UILabel(CGRect.Empty);
        private StockViewTip stockViewTip = new StockViewTip(CGRect.Empty);

        private Model _model;

        public Model Model
        {
            get => _model;
            set
            {
                _model = value;
                Setup();
            }
        }

        public StockView(CGRect frame) : base(frame)
        {
            bottomStripeView.AddSubviews(deliveryLabel, deliveryTextLabel);
            
            darkView.AddSubviews(topStripeView, stockLabel, stockTextLabel, bottomStripeView);
            
            contentView.AddSubviews(darkView, stockViewTip);
            
            AddSubview(contentView);
        }

        public override void LayoutSubviews()
        {
            contentView.TranslatesAutoresizingMaskIntoConstraints = false;
            topStripeView.TranslatesAutoresizingMaskIntoConstraints = false;
            stockTextLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            bottomStripeView.TranslatesAutoresizingMaskIntoConstraints = false;
            stockLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            stockTextLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            darkView.TranslatesAutoresizingMaskIntoConstraints = false;
            deliveryLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            deliveryTextLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            stockViewTip.TranslatesAutoresizingMaskIntoConstraints = false;
            AddConstraints(new []
            {
                contentView.LeadingAnchor.ConstraintEqualTo(LeadingAnchor),
                contentView.TopAnchor.ConstraintEqualTo(TopAnchor),
                contentView.TrailingAnchor.ConstraintEqualTo(TrailingAnchor),
                contentView.BottomAnchor.ConstraintEqualTo(BottomAnchor),
                
                contentView.TrailingAnchor.ConstraintEqualTo(darkView.TrailingAnchor),
                contentView.LeadingAnchor.ConstraintEqualTo(darkView.LeadingAnchor),
                contentView.TopAnchor.ConstraintEqualTo(darkView.TopAnchor),
                stockViewTip.TopAnchor.ConstraintEqualTo(darkView.BottomAnchor),
                stockViewTip.CenterXAnchor.ConstraintEqualTo(contentView.CenterXAnchor),
                stockViewTip.BottomAnchor.ConstraintEqualTo(contentView.BottomAnchor),
                
                topStripeView.LeadingAnchor.ConstraintEqualTo(darkView.LeadingAnchor),
                topStripeView.TopAnchor.ConstraintEqualTo(darkView.TopAnchor),
                darkView.TrailingAnchor.ConstraintEqualTo(topStripeView.TrailingAnchor),
                stockTextLabel.LeadingAnchor.ConstraintEqualTo(darkView.LeadingAnchor, 8),
                stockTextLabel.TopAnchor.ConstraintEqualTo(topStripeView.BottomAnchor, 2),
                bottomStripeView.LeadingAnchor.ConstraintEqualTo(darkView.LeadingAnchor),
                bottomStripeView.TrailingAnchor.ConstraintEqualTo(darkView.TrailingAnchor),
                bottomStripeView.BottomAnchor.ConstraintEqualTo(darkView.BottomAnchor),
                stockLabel.TopAnchor.ConstraintEqualTo(darkView.TopAnchor, 8),
                stockLabel.LeadingAnchor.ConstraintEqualTo(stockTextLabel.TrailingAnchor, 4),
                stockLabel.HeightAnchor.ConstraintEqualTo(stockTextLabel.HeightAnchor),
                
                topStripeView.HeightAnchor.ConstraintEqualTo(5),
                
                stockTextLabel.WidthAnchor.ConstraintGreaterThanOrEqualTo(19),
                
                bottomStripeView.HeightAnchor.ConstraintEqualTo(25),
                deliveryLabel.BottomAnchor.ConstraintEqualTo(bottomStripeView.BottomAnchor, -2).Priority(750),
                deliveryLabel.LeadingAnchor.ConstraintEqualTo(bottomStripeView.LeadingAnchor, 8),
                deliveryLabel.TopAnchor.ConstraintEqualTo(bottomStripeView.TopAnchor, 2).Priority(700),
                deliveryTextLabel.BottomAnchor.ConstraintEqualTo(bottomStripeView.BottomAnchor, -2).Priority(750),
                deliveryTextLabel.TopAnchor.ConstraintEqualTo(bottomStripeView.TopAnchor, 2).Priority(700),
                deliveryTextLabel.TrailingAnchor.ConstraintEqualTo(bottomStripeView.TrailingAnchor, -8),
                
                stockViewTip.HeightAnchor.ConstraintEqualTo(8),
                stockViewTip.HeightAnchor.ConstraintEqualTo(stockViewTip.WidthAnchor, stockViewTip.Frame.Size.Height / stockViewTip.Frame.Size.Width, 0)
            });
        }

        private void Setup()
        {
            darkView.Layer.CornerRadius = 5;
            darkView.Layer.MasksToBounds = true;

            contentView.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                OnViewClicked?.Invoke(this, new StockViewClickedEventArgs(Model));
            }));
            UpdateUI();
        }

        private void UpdateUI()
        {
            topStripeView.BackgroundColor = Model.Color;
            bottomStripeView.BackgroundColor = Model.Color;
            stockTextLabel.TextColor = Model.Color;
            stockViewTip.FillColor = UIColor.Black;

            stockTextLabel.Text = $"{Model.StockCount}";
            deliveryTextLabel.Text = Model.DeliveryDate;
        }

        public event EventHandler<StockViewClickedEventArgs> OnViewClicked;
    }
}
