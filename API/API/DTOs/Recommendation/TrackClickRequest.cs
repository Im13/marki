namespace API.DTOs.Recommendation
{
    public class TrackClickRequest
    {
        public int ProductId { get; set; }
        public string ReasonCode { get; set; }
    }
}