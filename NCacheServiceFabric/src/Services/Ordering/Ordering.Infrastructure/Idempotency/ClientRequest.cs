using System;

namespace Microsoft.eShopOnContainers.Services.Ordering.Infrastructure.Idempotency
{
    [Serializable]
    public class ClientRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
    }
}
