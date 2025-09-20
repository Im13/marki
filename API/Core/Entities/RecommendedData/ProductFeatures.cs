namespace Core.Entities.RecommendedData
{
    public class ProductFeatures
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public double PopularityScore { get; set; }
        public double AverageRating { get; set; }
        public int TotalRatings { get; set; }
        public Dictionary<string, double> CategorySimilarity { get; set; } = new();
    }
}