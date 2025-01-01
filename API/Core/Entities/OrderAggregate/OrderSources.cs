using System.Runtime.Serialization;

namespace Core.Entities.OrderAggregate
{
    public enum OrderSources
    {
        [EnumMember(Value = "Website")]
        Website,
        [EnumMember(Value = "Offline")]
        Offline,
        [EnumMember(Value = "Instagram")]
        Instagram,
        [EnumMember(Value = "Facebook")]
        Facebook,
        [EnumMember(Value = "Tiktok")]
        Tiktok
    }
}