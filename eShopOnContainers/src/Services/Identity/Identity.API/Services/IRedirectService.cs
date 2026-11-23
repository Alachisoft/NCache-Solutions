// Copyright (c) .NET Foundation and Contributors.
namespace Microsoft.eShopOnContainers.Services.Identity.API.Services
{
    public interface IRedirectService
    {
        string ExtractRedirectUriFromReturnUrl(string url);
    }
}
