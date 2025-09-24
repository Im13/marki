namespace Core.Settings
{
    public class TimeDecaySettings
    {
        public double HalfLifeDays { get; set; } = 30;
        public int MaxAgeDays { get; set; } = 365;
    }
}