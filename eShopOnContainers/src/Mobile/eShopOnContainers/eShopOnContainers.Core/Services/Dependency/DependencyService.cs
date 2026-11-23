// Copyright (c) 2017 Microsoft Corp.
namespace eShopOnContainers.Core.Services.Dependency
{
    public class DependencyService : IDependencyService
    {
        public T Get<T>() where T : class
        {
            return Xamarin.Forms.DependencyService.Get<T>();
        }
    }
}
