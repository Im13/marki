namespace API.Core.DTOs
{
    public class RecommendationDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public double Score { get; set; }
        public string RecommendationType { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new();
        public double AverageRating { get; set; }
        public int TotalRatings { get; set; }

    }
}