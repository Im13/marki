namespace Core.Entities
{
    public class SlideImage : BaseEntity
    {
        public int OrderNo { get; set; }
        public string DesktopImageUrl { get; set; }
        public string MobileImageUrl { get; set; }
        public string Link { get; set; }
        public string AltText { get; set; }
        public bool Status { get; set; }
    }
}