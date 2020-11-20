using Newtonsoft.Json;
using System;

namespace Alachisoft.NCache.Samples.PostGreSQLNotificationDependency
{
    [Serializable]
    public class Entity
    {
        [JsonProperty("table")]
        public string Table { get; set; }

        [JsonProperty("schema")]
        public string Schema { get; set; }

        [JsonProperty("dep_key")]
        public string DependencyKey { get; set; }
    }
}
