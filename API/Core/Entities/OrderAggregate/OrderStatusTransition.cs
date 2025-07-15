namespace Core.Entities.OrderAggregate
{
    public class OrderStatusTransition
    {
        private static readonly Dictionary<OrderStatus, List<OrderStatus>> AllowedTransitions = new()
        {
            { OrderStatus.New, new List<OrderStatus>
                {
                    OrderStatus.WaitingForStock,
                    OrderStatus.PriorityShipping,
                    OrderStatus.Confirmed,
                    OrderStatus.Shipped,
                    OrderStatus.Cancelled,
                    OrderStatus.Deleted
                }
            },
            { OrderStatus.WaitingForStock, new List<OrderStatus>
                {
                    OrderStatus.PriorityShipping,
                    OrderStatus.Confirmed,
                    OrderStatus.Shipped,
                    OrderStatus.Cancelled,
                    OrderStatus.Deleted
                }
            },
            { OrderStatus.PriorityShipping, new List<OrderStatus>
                {
                    OrderStatus.WaitingForStock,
                    OrderStatus.Confirmed,
                    OrderStatus.Shipped,
                    OrderStatus.Cancelled,
                    OrderStatus.Deleted
                }
            },
            { OrderStatus.Confirmed, new List<OrderStatus>
                {
                    OrderStatus.WaitingForStock,
                    OrderStatus.PriorityShipping,
                    OrderStatus.Shipped,
                    OrderStatus.Cancelled,
                    OrderStatus.Deleted
                }
            },
            { OrderStatus.Shipped, new List<OrderStatus>
                {
                    OrderStatus.CustomerRefused,
                    OrderStatus.Completed,
                    OrderStatus.Cancelled,
                    OrderStatus.Deleted
                }
            },
            { OrderStatus.Cancelled, new List<OrderStatus>
                {
                    OrderStatus.Deleted,
                    OrderStatus.Duplicate
                }
            },
            { OrderStatus.Deleted, new List<OrderStatus>
                {
                    OrderStatus.Duplicate
                }
            },
            { OrderStatus.Completed, new List<OrderStatus>
                {
                    OrderStatus.Duplicate
                }
            }
        };
    }
}