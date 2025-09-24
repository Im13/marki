namespace Core.Settings
{
    public class RecommendationSettings
    {
        public int ModelTrainingIntervalHours { get; set; } = 6;
        public int RecommendationCacheExpirationHours { get; set; } = 24;
        public int MaxRecommendationsPerUser { get; set; } = 50;
        public int SimilarProductsCount { get; set; } = 10;
        public int PopularProductsCount { get; set; } = 20;
        public int MinInteractionsForCollaborativeFiltering { get; set; } = 5;
        public double MinSimilarityThreshold { get; set; } = 0.1;

        public WeightSettings WeightSettings { get; set; } = new();
        public TimeDecaySettings TimeDecaySettings { get; set; } = new();
    }
}