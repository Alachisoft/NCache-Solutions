// Copyright (c) .NET Foundation and Contributors.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebhookClient.Models
{
    public class WebhookResponse
    {
        public DateTime Date { get; set; }
        public string DestUrl { get; set; }
        public string Token { get; set; }
    }
}
