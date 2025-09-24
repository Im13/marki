namespace API.Core.DTO
{
    public class SimilarProductDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public double SimilarityScore { get; set; }
        public string Reason { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new();
        public double AverageRating { get; set; }
    }
}