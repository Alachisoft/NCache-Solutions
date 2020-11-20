using Alachisoft.NCache.Runtime.Dependencies;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Alachisoft.NCache.Samples.PostGreSQLNotificationDependency
{
    [Serializable]
    public class PostGreSQLDependency : NotifyExtensibleDependency
    {
        private readonly string _connectionString;
        private readonly string _dependencyKey;
        private readonly string _schema;
        private readonly string _table;
        private readonly string _channel;


        private IDbConnection connection;
        private object task;
        private bool done = false;
        public PostGreSQLDependency(string connectionString, string dependencyKey, string schema, string table, string channel)
        {
            _connectionString = connectionString.Trim();
            _dependencyKey = dependencyKey.Trim();
            _schema = schema.Trim();
            _table = table.Trim();
            _channel = channel.Trim();
        }
        public override bool Initialize()
        {

            connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            ((NpgsqlConnection)connection).Notification += (o, e) =>
            {
                var entity = JsonConvert.DeserializeObject<Entity>(e.AdditionalInformation);

                if (entity.DependencyKey == _dependencyKey && entity.Table == _table && entity.Schema == _schema)
                {
                    done = true;
                }
            };

            using (var cmd = new NpgsqlCommand($"LISTEN {_channel};", (NpgsqlConnection)connection))
            {
                cmd.ExecuteNonQuery();

            }

            task = Task.Run(() =>
            {
                while (true)
                {
                    ((NpgsqlConnection)connection).Wait();

                    if (done)
                    {
                        break;
                    }
                }

                this.DependencyChanged.Invoke(this);

            });

            return true;
        }

    }
}
