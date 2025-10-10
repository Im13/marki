namespace API.DTOs.Recommendation
{
    public class TrackViewRequest
    {
        public int ProductId { get; set; }
        public int? DurationSeconds { get; set; }
    }
}