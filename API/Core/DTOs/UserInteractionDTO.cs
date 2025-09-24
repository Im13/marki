using Core.Enums;

namespace API.Core.DTO
{
    public class UserInteractionDTO
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public InteractionType Type { get; set; }
        public double? Rating { get; set; }
        public int? Duration { get; set; }
        public DateTime? Timestamp { get; set; }

    }
}