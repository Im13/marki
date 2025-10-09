namespace Core.DTOs.Recommendations
{
    public class RecommendationDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSlug { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public string PriceDisplay { get; set; }
        public string ImageUrl { get; set; }
        public int TotalStock { get; set; }
        public List<string> AvailableColors { get; set; }
        public List<string> AvailableSizes { get; set; }
        public double Score { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonText { get; set; }
    }
}