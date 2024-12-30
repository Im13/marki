using System.Runtime.Serialization;

namespace Core.Entities.OrderAggregate
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Mới")]
        New,

        [EnumMember(Value = "Cần xử lý")]
        NeedAttention,

        [EnumMember(Value = "Chờ hàng")]
        Restock,

        [EnumMember(Value = "Đã xác nhận")]
        Confirmed,

        [EnumMember(Value = "Đang đóng hàng")]
        Packaging,

        [EnumMember(Value = "Chờ chuyển hàng")]
        WaitForPickup,

        [EnumMember(Value = "Đã gửi hàng")]
        Shipped,

        [EnumMember(Value = "Đã nhận")]
        Delivered,

        [EnumMember(Value = "Đã thu tiền")]
        CollectedMoney,

        [EnumMember(Value = "Đang hoàn")]
        Returning,

        [EnumMember(Value = "Đã hoàn")]
        Returned,

        [EnumMember(Value = "Đã huỷ")]
        Canceled,

        [EnumMember(Value = "Đã xoá")]
        Deleted
    }
}