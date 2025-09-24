namespace Core.Settings
{
    public class WeightSettings
    {
        public double ViewWeight { get; set; } = 1.0;
        public double LikeWeight { get; set; } = 2.0;
        public double AddToCartWeight { get; set; } = 3.0;
        public double PurchaseWeight { get; set; } = 5.0;
        public double RatingWeight { get; set; } = 4.0;
    }
}