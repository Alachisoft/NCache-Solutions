// Copyright (c) .NET Foundation and Contributors.
namespace Basket.API.Infrastructure.Filters
{
    public class JsonErrorResponse
    {
        public string[] Messages { get; set; }

        public object DeveloperMessage { get; set; }
    }
}
