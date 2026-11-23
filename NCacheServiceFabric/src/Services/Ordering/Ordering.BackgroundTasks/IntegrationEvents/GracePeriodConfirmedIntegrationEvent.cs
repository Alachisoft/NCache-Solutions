// Copyright (c) .NET Foundation and Contributors.
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Ordering.BackgroundTasks.IntegrationEvents
{
    public class GracePeriodConfirmedIntegrationEvent : IntegrationEvent
    {
        public int OrderId { get; }

        public GracePeriodConfirmedIntegrationEvent(int orderId) =>
            OrderId = orderId;
    }
}
