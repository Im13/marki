namespace API.DTOs.Recommendation
{
    public class TrackCartRequest
    {
        public int ProductId { get; set; }
        public int? SkuId { get; set; }
    }
}