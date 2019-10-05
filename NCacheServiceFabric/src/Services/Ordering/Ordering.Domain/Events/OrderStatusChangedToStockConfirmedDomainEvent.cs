namespace Ordering.Domain.Events
{
    using MediatR;
    using System;

    /// <summary>
    /// Event used when the order stock items are confirmed
    /// </summary>
    /// 
    [Serializable]
    public class OrderStatusChangedToStockConfirmedDomainEvent
        : INotification
    {
        public int OrderId { get; }

        public OrderStatusChangedToStockConfirmedDomainEvent(int orderId)
            => OrderId = orderId;
    }
}