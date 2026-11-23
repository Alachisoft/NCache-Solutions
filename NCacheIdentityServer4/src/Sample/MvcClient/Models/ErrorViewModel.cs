// Copyright notices for this sample are in LICENSE-IdentityServer4 and LICENSE-Alachisoft at the sample root.
using System;

namespace MvcClient.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
