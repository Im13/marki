namespace Core.Entities.OrderAggregate
{
    public enum OrderStatus
    {
        New = 1,
        WaitingForStock = 2,
        PriorityShipping = 3,
        Confirmed = 4,
        Shipped = 5,
        Cancelled = 6,
        Deleted = 7,
        CustomerRefused = 8,
        Returning = 9,
        FullyReturned = 10,
        Completed = 11,
        Duplicate = 12
    }
}