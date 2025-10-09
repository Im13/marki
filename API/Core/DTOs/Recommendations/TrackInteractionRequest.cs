namespace Core.DTOs.Recommendations
{
    public class TrackInteractionRequest
    {
        public int ProductId { get; set; }
        public int? SkuId { get; set; }
        public int? DurationSeconds { get; set; }
    }
}