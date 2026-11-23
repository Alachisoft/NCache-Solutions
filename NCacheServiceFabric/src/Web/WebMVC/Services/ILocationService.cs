// Copyright (c) .NET Foundation and Contributors.
using System.Threading.Tasks;
using WebMVC.Services.ModelDTOs;

namespace WebMVC.Services
{
    public interface ILocationService
    {
        Task CreateOrUpdateUserLocation(LocationDTO location);
    }
}
