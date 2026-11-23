// Copyright (c) .NET Foundation and Contributors.
namespace Ordering.API.Application.IntegrationEvents.Events
{
    using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

    public class OrderPaymentFailedIntegrationEvent : IntegrationEvent
    {
        public int OrderId { get; }

        public OrderPaymentFailedIntegrationEvent(int orderId) => OrderId = orderId;
    }
}