// Copyright (c) 2017 Microsoft Corp.
namespace eShopOnContainers.Core.Services.Dependency
{
    public interface IDependencyService
    {
        T Get<T>() where T : class;
    }
}
