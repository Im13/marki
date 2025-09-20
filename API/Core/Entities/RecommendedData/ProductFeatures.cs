namespace Core.Entities.RecommendedData
{
    public class ProductFeatures
    {
        public int ProductId { get; set; }
        public double PopularityScore { get; set; }
        public double AverageRating { get; set; }
        public int TotalRatings { get; set; }
        public Dictionary<string, double> CategorySimilarity { get; set; } = new();
    }
}