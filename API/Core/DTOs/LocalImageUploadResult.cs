namespace Core.DTOs
{
    public class LocalImageUploadResult
    {
        public string SecureUrl { get; set; }
        public string PublicId { get; set; }
        public string Result { get; set; } = "ok";
    }
}
