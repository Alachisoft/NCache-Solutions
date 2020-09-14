using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.NCache.Entities;
using IdentityServer4.Stores.Serialization;

namespace IdentityServer4.NCache.Mappers
{
    public static class DeviceFlowCodesMappers
    {
        private static readonly IPersistentGrantSerializer _serializer = new PersistentGrantSerializer();

        public static DeviceCode ToModel(this DeviceFlowCodes deviceFlowCode)
        {
            
            if (deviceFlowCode == null || string.IsNullOrWhiteSpace(deviceFlowCode.Data)) return null;

            return _serializer.Deserialize<DeviceCode>(deviceFlowCode.Data);
        }

        public static DeviceFlowCodes ToEntity(this DeviceCode model, string deviceCode, string userCode)
        {
            if (model == null || deviceCode == null || userCode == null) return null;

            return new DeviceFlowCodes
            {
                DeviceCode = deviceCode,
                UserCode = userCode,
                ClientId = model.ClientId,
                SubjectId = model.Subject?.FindFirst(JwtClaimTypes.Subject).Value,
                CreationTime = model.CreationTime,
                Expiration = model.CreationTime.AddSeconds(model.Lifetime),
                Data = _serializer.Serialize(model)
            };
        }
    }
}
