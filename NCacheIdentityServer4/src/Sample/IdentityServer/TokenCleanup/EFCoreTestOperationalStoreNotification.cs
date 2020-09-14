using IdentityServer4.EntityFramework;
using IdentityServer4.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class EFCoreTestOperationalStoreNotification : IOperationalStoreNotification
    {
        public EFCoreTestOperationalStoreNotification()
        {
            Console.WriteLine("Sql Server Notification ctor");
        }

        public Task PersistedGrantsRemovedAsync(
            IEnumerable<PersistedGrant> persistedGrants)
        {
            foreach (var grant in persistedGrants)
            {
                Console.WriteLine("cleaned from SQL Server: " + grant.Type);
            }
            return Task.CompletedTask;
        }
    }
}
