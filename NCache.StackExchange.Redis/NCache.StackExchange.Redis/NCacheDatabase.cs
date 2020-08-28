using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Caching;

namespace NCache.StackExchange.Redis
{
    public class NCacheDatabase : INCacheDatabase
    {
        ICache _cache;

        public void Initialize(ICache cache)
        {
            _cache = cache;
        }

        public NCacheDatabase()
        {

        }

        public NCacheDatabase(ICache cache)
        {
            Initialize(cache);
        }
        public int Database => throw new NotSupportedException();

        public IConnectionMultiplexer Multiplexer => throw new NotSupportedException();

        public IBatch CreateBatch(object asyncState = null)
        {
            throw new NotSupportedException();
        }

        public ITransaction CreateTransaction(object asyncState = null)
        {
            throw new NotSupportedException();
        }

        public RedisValue DebugObject(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<RedisValue> DebugObjectAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public RedisResult Execute(string command, params object[] args)
        {
            throw new NotSupportedException();
        }

        public RedisResult Execute(string command, ICollection<object> args, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<RedisResult> ExecuteAsync(string command, params object[] args)
        {
            throw new NotSupportedException();
        }

        public Task<RedisResult> ExecuteAsync(string command, ICollection<object> args, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public bool GeoAdd(RedisKey key, double longitude, double latitude, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public bool GeoAdd(RedisKey key, GeoEntry value, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public long GeoAdd(RedisKey key, GeoEntry[] values, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<bool> GeoAddAsync(RedisKey key, double longitude, double latitude, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<bool> GeoAddAsync(RedisKey key, GeoEntry value, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<long> GeoAddAsync(RedisKey key, GeoEntry[] values, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public double? GeoDistance(RedisKey key, RedisValue member1, RedisValue member2, GeoUnit unit = GeoUnit.Meters, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<double?> GeoDistanceAsync(RedisKey key, RedisValue member1, RedisValue member2, GeoUnit unit = GeoUnit.Meters, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public string[] GeoHash(RedisKey key, RedisValue[] members, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public string GeoHash(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<string[]> GeoHashAsync(RedisKey key, RedisValue[] members, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<string> GeoHashAsync(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public GeoPosition?[] GeoPosition(RedisKey key, RedisValue[] members, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public GeoPosition? GeoPosition(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<GeoPosition?[]> GeoPositionAsync(RedisKey key, RedisValue[] members, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<GeoPosition?> GeoPositionAsync(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public GeoRadiusResult[] GeoRadius(RedisKey key, RedisValue member, double radius, GeoUnit unit = GeoUnit.Meters, int count = -1, Order? order = null, GeoRadiusOptions options = GeoRadiusOptions.Default, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public GeoRadiusResult[] GeoRadius(RedisKey key, double longitude, double latitude, double radius, GeoUnit unit = GeoUnit.Meters, int count = -1, Order? order = null, GeoRadiusOptions options = GeoRadiusOptions.Default, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<GeoRadiusResult[]> GeoRadiusAsync(RedisKey key, RedisValue member, double radius, GeoUnit unit = GeoUnit.Meters, int count = -1, Order? order = null, GeoRadiusOptions options = GeoRadiusOptions.Default, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<GeoRadiusResult[]> GeoRadiusAsync(RedisKey key, double longitude, double latitude, double radius, GeoUnit unit = GeoUnit.Meters, int count = -1, Order? order = null, GeoRadiusOptions options = GeoRadiusOptions.Default, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public bool GeoRemove(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<bool> GeoRemoveAsync(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public long HashDecrement(RedisKey key, RedisValue hashField, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public double HashDecrement(RedisKey key, RedisValue hashField, double value, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<long> HashDecrementAsync(RedisKey key, RedisValue hashField, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<double> HashDecrementAsync(RedisKey key, RedisValue hashField, double value, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public bool HashDelete(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public long HashDelete(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<bool> HashDeleteAsync(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<long> HashDeleteAsync(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public bool HashExists(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<bool> HashExistsAsync(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public RedisValue HashGet(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public RedisValue[] HashGet(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public HashEntry[] HashGetAll(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<HashEntry[]> HashGetAllAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<RedisValue> HashGetAsync(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<RedisValue[]> HashGetAsync(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Lease<byte> HashGetLease(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<Lease<byte>> HashGetLeaseAsync(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public long HashIncrement(RedisKey key, RedisValue hashField, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public double HashIncrement(RedisKey key, RedisValue hashField, double value, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<long> HashIncrementAsync(RedisKey key, RedisValue hashField, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<double> HashIncrementAsync(RedisKey key, RedisValue hashField, double value, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public RedisValue[] HashKeys(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<RedisValue[]> HashKeysAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public long HashLength(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<long> HashLengthAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public IEnumerable<HashEntry> HashScan(RedisKey key, RedisValue pattern, int pageSize, CommandFlags flags)
        {
            throw new NotSupportedException();
        }

        public IEnumerable<HashEntry> HashScan(RedisKey key, RedisValue pattern = default, int pageSize = 250, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public IAsyncEnumerable<HashEntry> HashScanAsync(RedisKey key, RedisValue pattern = default, int pageSize = 250, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public void HashSet(RedisKey key, HashEntry[] hashFields, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public bool HashSet(RedisKey key, RedisValue hashField, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task HashSetAsync(RedisKey key, HashEntry[] hashFields, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<bool> HashSetAsync(RedisKey key, RedisValue hashField, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public long HashStringLength(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<long> HashStringLengthAsync(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public RedisValue[] HashValues(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<RedisValue[]> HashValuesAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public bool HyperLogLogAdd(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public bool HyperLogLogAdd(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<bool> HyperLogLogAddAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<bool> HyperLogLogAddAsync(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public long HyperLogLogLength(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public long HyperLogLogLength(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<long> HyperLogLogLengthAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<long> HyperLogLogLengthAsync(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public void HyperLogLogMerge(RedisKey destination, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public void HyperLogLogMerge(RedisKey destination, RedisKey[] sourceKeys, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task HyperLogLogMergeAsync(RedisKey destination, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task HyperLogLogMergeAsync(RedisKey destination, RedisKey[] sourceKeys, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public EndPoint IdentifyEndpoint(RedisKey key = default, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<EndPoint> IdentifyEndpointAsync(RedisKey key = default, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public bool IsConnected(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return (_cache != null);

        }

        public bool KeyDelete(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                _cache.Remove(key);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

        public long KeyDelete(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                List<string> nKeys = RedisKeysToStringList(keys);
                _cache.RemoveBulk(nKeys);
                return keys.Count();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private static List<string> RedisKeysToStringList(RedisKey[] keys)
        {
            List<string> nKeys = new List<string>();
            foreach (var key in keys)
            {
                nKeys.Add(key);
            }

            return nKeys;
        }

        public Task<bool> KeyDeleteAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return KeyDelete(key, flags);
            });
        }

        public Task<long> KeyDeleteAsync(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return KeyDelete(keys, flags);
            });
        }

        private static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        private static Object ByteArrayToObject(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }

        public byte[] KeyDump(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                var value = _cache.Get<object>(key);
                return ObjectToByteArray(value);

            }
            catch (Exception)
            {

                return null;
            }
        }

        public Task<byte[]> KeyDumpAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return KeyDump(key, flags);
            });
        }

        public bool KeyExists(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return _cache.Contains(key);
        }

        public long KeyExists(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            var nkeys = _cache.ContainsBulk(RedisKeysToStringList(keys));
            return nkeys.Where(s => s.Value == true).Count();
        }

        public Task<bool> KeyExistsAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return KeyExists(key, flags);
            });
        }

        public Task<long> KeyExistsAsync(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return KeyExists(keys, flags);
            });
        }

        public bool KeyExpire(RedisKey key, TimeSpan? expiry, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                var cacheItem = _cache.GetCacheItem(key);
                cacheItem.Expiration = expiry != null ? new Alachisoft.NCache.Runtime.Caching.Expiration(Alachisoft.NCache.Runtime.Caching.ExpirationType.Sliding, expiry.Value) : null;
                _cache.Insert(key, cacheItem);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool KeyExpire(RedisKey key, DateTime? expiry, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                var cacheItem = _cache.GetCacheItem(key);
                cacheItem.Expiration = expiry != null ? new Alachisoft.NCache.Runtime.Caching.Expiration(Alachisoft.NCache.Runtime.Caching.ExpirationType.Absolute, new TimeSpan(expiry.Value.Ticks - DateTime.Now.Ticks)) : null;
                _cache.Insert(key, cacheItem);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public Task<bool> KeyExpireAsync(RedisKey key, TimeSpan? expiry, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return KeyExpire(key, expiry, flags);
            });
        }

        public Task<bool> KeyExpireAsync(RedisKey key, DateTime? expiry, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return KeyExpire(key, expiry, flags);
            });
        }

        public TimeSpan? KeyIdleTime(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<TimeSpan?> KeyIdleTimeAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public void KeyMigrate(RedisKey key, EndPoint toServer, int toDatabase = 0, int timeoutMilliseconds = 0, MigrateOptions migrateOptions = MigrateOptions.None, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task KeyMigrateAsync(RedisKey key, EndPoint toServer, int toDatabase = 0, int timeoutMilliseconds = 0, MigrateOptions migrateOptions = MigrateOptions.None, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public bool KeyMove(RedisKey key, int database, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<bool> KeyMoveAsync(RedisKey key, int database, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public bool KeyPersist(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                var cacheItem = _cache.GetCacheItem(key);
                cacheItem.Expiration = null;
                _cache.Insert(key, cacheItem);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public Task<bool> KeyPersistAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return KeyPersist(key, flags);
            });
        }

        public RedisKey KeyRandom(CommandFlags flags = CommandFlags.None)
        {
            IDictionaryEnumerator cacheKeys = (IDictionaryEnumerator)_cache.GetEnumerator();
            var cacheCount = _cache.Count;
            Random r = new Random();
            int randomNumber = r.Next(0, (int)(cacheCount - 1));
            if (cacheKeys != null)
            {
                long index = 0;
                while (cacheKeys.MoveNext())
                {
                    if (randomNumber == index)
                        return new RedisKey(cacheKeys.Key.ToString());
                    index++;
                }//end while 
            }
            return new RedisKey();
        }

        public Task<RedisKey> KeyRandomAsync(CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return KeyRandom(flags);
            });
        }

        public bool KeyRename(RedisKey key, RedisKey newKey, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                var cacheItem = _cache.GetCacheItem(key);
                _cache.Remove(key);
                _cache.Insert(newKey, cacheItem);
            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }

        public Task<bool> KeyRenameAsync(RedisKey key, RedisKey newKey, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return KeyRename(key, newKey, when, flags);
            });
        }

        public void KeyRestore(RedisKey key, byte[] value, TimeSpan? expiry = null, CommandFlags flags = CommandFlags.None)
        {
            var obj = ByteArrayToObject(value);
            var cacheItem = new CacheItem(obj);
            cacheItem.Expiration = expiry != null ? new Alachisoft.NCache.Runtime.Caching.Expiration(Alachisoft.NCache.Runtime.Caching.ExpirationType.Sliding, expiry.Value) : null;
            _cache.Insert(key, cacheItem);
        }

        public Task KeyRestoreAsync(RedisKey key, byte[] value, TimeSpan? expiry = null, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                KeyRestore(key, value, expiry, flags);
            });
        }

        public TimeSpan? KeyTimeToLive(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<TimeSpan?> KeyTimeToLiveAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public bool KeyTouch(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                _cache.GetCacheItem(key);
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        public long KeyTouch(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return _cache.GetBulk<Object>(RedisKeysToStringList(keys)).Where(s => s.Value != null).Count();
            }
            catch (Exception)
            {

                return 0;
            }
        }

        public Task<bool> KeyTouchAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return KeyTouch(key, flags);
            });
        }

        public Task<long> KeyTouchAsync(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return KeyTouch(keys, flags);
            });
        }

        public RedisType KeyType(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<RedisType> KeyTypeAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public RedisValue ListGetByIndex(RedisKey key, long index, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                var dList = _cache.DataTypeManager.GetList<object>(key);
                var val = dList.ElementAt((int)index);
                return RedisValue.Unbox(val);
            }
            catch (Exception)
            {

                return new RedisValue();
            }
        }

        public Task<RedisValue> ListGetByIndexAsync(RedisKey key, long index, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return ListGetByIndex(key, index, flags);
            });
        }

        public long ListInsertAfter(RedisKey key, RedisValue pivot, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                var dList = _cache.DataTypeManager.GetList<object>(key);
                dList.InsertAfter(pivot, value);
                return dList.Count();
            }
            catch (Exception)
            {

                return -1;
            }
        }

        public Task<long> ListInsertAfterAsync(RedisKey key, RedisValue pivot, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return ListInsertAfter(key, pivot, value, flags);
            });
        }

        public long ListInsertBefore(RedisKey key, RedisValue pivot, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                var dList = _cache.DataTypeManager.GetList<object>(key);
                dList.InsertBefore(pivot, value);
                return dList.Count();
            }
            catch (Exception)
            {

                return -1;
            }
        }

        public Task<long> ListInsertBeforeAsync(RedisKey key, RedisValue pivot, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return ListInsertBefore(key, pivot, value, flags);
            });
        }

        public RedisValue ListLeftPop(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                var dList = _cache.DataTypeManager.GetList<object>(key);
                if (dList == null)
                    return new RedisValue();
                var val = dList.ElementAt((int)0);
                dList.RemoveAt(0);
                return RedisValue.Unbox(val);
            }
            catch (Exception)
            {

                return new RedisValue();
            }
        }

        public Task<RedisValue> ListLeftPopAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return ListLeftPop(key, flags);
            });
        }

        public long ListLeftPush(RedisKey key, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                var dList = _cache.DataTypeManager.GetList<object>(key);
                if (dList == null)
                    dList = _cache.DataTypeManager.CreateList<object>(key);
                dList.InsertAtHead(value.Box());
                return dList.Count();
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public long ListLeftPush(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                var dList = _cache.DataTypeManager.GetList<object>(key);
                if (dList == null)
                    dList = _cache.DataTypeManager.CreateList<object>(key);
                foreach (var value in values)
                {
                    dList.InsertAtHead(value.Box());
                }

                return dList.Count();
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public Task<long> ListLeftPushAsync(RedisKey key, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return ListLeftPush(key, value, when, flags);
            });
        }

        public Task<long> ListLeftPushAsync(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return ListLeftPush(key, values, flags);
            });
        }

        public long ListLength(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                return _cache.DataTypeManager.GetList<object>(key).Count();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public Task<long> ListLengthAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return ListLength(key, flags);
            });
        }

        public RedisValue[] ListRange(RedisKey key, long start = 0, long stop = -1, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                var dList = _cache.DataTypeManager.GetList<object>(key);
                if (dList == null)
                    dList = _cache.DataTypeManager.CreateList<object>(key);
                
                var listValues = dList.GetRange((int)start, (int)stop < 0 ? dList.Count - 1 - (int)start : ((int)stop - (int)start));
                RedisValue[] values = new RedisValue[listValues.Count];
                for (int i = 0; i < listValues.Count; i++)
                {
                    values[i] = RedisValue.Unbox(listValues[i]);
                }
                return values;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public Task<RedisValue[]> ListRangeAsync(RedisKey key, long start = 0, long stop = -1, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return ListRange(key, start, stop, flags);
            });
        }

        public long ListRemove(RedisKey key, RedisValue value, long count = 0, CommandFlags flags = CommandFlags.None)
        {
            var dList = _cache.DataTypeManager.GetList<object>(key);
            if (dList == null)
                return 0;
            try
            {
                dList.Remove(value.Box());
            }
            catch (Exception)
            {
            }


            return dList.Count;
        }

        public Task<long> ListRemoveAsync(RedisKey key, RedisValue value, long count = 0, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return ListRemove(key, value, count, flags);
            });
        }

        public RedisValue ListRightPop(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            var dList = _cache.DataTypeManager.GetList<object>(key);
            if (dList == null)
                return new RedisValue();
            try
            {
                return RedisValue.Unbox(dList.Last());
            }
            catch (Exception)
            {
            }
            return new RedisValue();
        }

        public Task<RedisValue> ListRightPopAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return ListRightPop(key, flags);
            });
        }

        public RedisValue ListRightPopLeftPush(RedisKey source, RedisKey destination, CommandFlags flags = CommandFlags.None)
        {
            var dSourceList = _cache.DataTypeManager.GetList<object>(source);
            var dDestinationList = _cache.DataTypeManager.GetList<object>(destination);
            if (dSourceList == null || dDestinationList == null)
                return new RedisValue();
            try
            {
                var poped = dSourceList.Last();
                dSourceList.RemoveAt(dSourceList.Count - 1);
                dDestinationList.InsertAtHead(poped);
                return RedisValue.Unbox(poped);
            }
            catch (Exception)
            {
            }
            return new RedisValue();
        }

        public Task<RedisValue> ListRightPopLeftPushAsync(RedisKey source, RedisKey destination, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return ListRightPopLeftPush(source, destination, flags);
            });
        }

        public long ListRightPush(RedisKey key, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            var dList = _cache.DataTypeManager.GetList<object>(key);
            if (dList == null)
                dList = _cache.DataTypeManager.CreateList<object>(key);
            try
            {
                dList.InsertAtTail(value.Box());
            }
            catch (Exception)
            {
            }
            return dList.Count();
        }

        public long ListRightPush(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            var dList = _cache.DataTypeManager.GetList<object>(key);
            if (dList == null)
                dList = _cache.DataTypeManager.CreateList<object>(key);
            try
            {
                foreach (var value in values)
                {
                    dList.InsertAtTail(value.Box());
                }
            }
            catch (Exception)
            {
            }
            return dList.Count();
        }

        public Task<long> ListRightPushAsync(RedisKey key, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return ListRightPush(key, value, when, flags);
            });
        }

        public Task<long> ListRightPushAsync(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return ListRightPush(key, values, flags);
            });
        }

        public void ListSetByIndex(RedisKey key, long index, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            var dList = _cache.DataTypeManager.GetList<object>(key);
            if (dList == null)
                return;
            try
            {
                dList[(int)index] = (value.Box());
            }
            catch (Exception)
            {
            }
        }

        public Task ListSetByIndexAsync(RedisKey key, long index, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                ListSetByIndex(key, index, value, flags);
            });
        }

        public void ListTrim(RedisKey key, long start, long stop, CommandFlags flags = CommandFlags.None)
        {
            var dList = _cache.DataTypeManager.GetList<object>(key);
            if (dList == null)
                return;
            try
            {
                dList.Trim((int)start, (int)stop);
            }
            catch (Exception)
            {
            }
        }

        public Task ListTrimAsync(RedisKey key, long start, long stop, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                ListTrim(key, start, stop, flags);
            });
        }

        public bool LockExtend(RedisKey key, RedisValue value, TimeSpan expiry, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<bool> LockExtendAsync(RedisKey key, RedisValue value, TimeSpan expiry, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public RedisValue LockQuery(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<RedisValue> LockQueryAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public bool LockRelease(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<bool> LockReleaseAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public bool LockTake(RedisKey key, RedisValue value, TimeSpan expiry, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<bool> LockTakeAsync(RedisKey key, RedisValue value, TimeSpan expiry, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public TimeSpan Ping(CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<TimeSpan> PingAsync(CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public long Publish(RedisChannel channel, RedisValue message, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                var topic = _cache.MessagingService.GetTopic(channel);
                topic = topic == null ? _cache.MessagingService.CreateTopic(channel) : topic;
                topic.Publish(new Alachisoft.NCache.Runtime.Caching.Message(message.Box()), Alachisoft.NCache.Runtime.Caching.DeliveryOption.All);
                return 1;
            }
            catch (Exception ex)
            {

                return 0;
            }
        }

        public Task<long> PublishAsync(RedisChannel channel, RedisValue message, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return Publish(channel, message, flags);
            });
        }

        public RedisResult ScriptEvaluate(string script, RedisKey[] keys = null, RedisValue[] values = null, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public RedisResult ScriptEvaluate(byte[] hash, RedisKey[] keys = null, RedisValue[] values = null, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public RedisResult ScriptEvaluate(LuaScript script, object parameters = null, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public RedisResult ScriptEvaluate(LoadedLuaScript script, object parameters = null, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<RedisResult> ScriptEvaluateAsync(string script, RedisKey[] keys = null, RedisValue[] values = null, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<RedisResult> ScriptEvaluateAsync(byte[] hash, RedisKey[] keys = null, RedisValue[] values = null, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<RedisResult> ScriptEvaluateAsync(LuaScript script, object parameters = null, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<RedisResult> ScriptEvaluateAsync(LoadedLuaScript script, object parameters = null, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public bool SetAdd(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            var dSet = _cache.DataTypeManager.GetHashSet<object>(key);
            if (dSet == null)
                dSet = _cache.DataTypeManager.CreateHashSet<object>(key);
            try
            {

                dSet.Add(value.Box());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public long SetAdd(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            var dSet = _cache.DataTypeManager.GetHashSet<object>(key);
            if (dSet == null)
                dSet = _cache.DataTypeManager.CreateHashSet<object>(key);

            var itemsAddedCount = 0;
            foreach (var value in values)
            {
                try
                {
                    dSet.Add(value.Box());
                    itemsAddedCount++;
                }
                catch
                {

                }
            }
            return itemsAddedCount;

        }

        public Task<bool> SetAddAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return SetAdd(key, value, flags);
            });
        }

        public Task<long> SetAddAsync(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return SetAdd(key, values, flags);
            });
        }

        public RedisValue[] SetCombine(SetOperation operation, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
        {
            var firstSet = _cache.DataTypeManager.CreateHashSet<object>(first);
            try
            {
                IEnumerable<object> result = new List<object>();
                switch (operation)
                {
                    case SetOperation.Union:
                        result = firstSet.Union(second);
                        break;
                    case SetOperation.Intersect:
                        result = firstSet.Intersect(second);
                        break;
                    case SetOperation.Difference:
                        result = firstSet.Difference(second);
                        break;
                }

                RedisValue[] values = new RedisValue[
                result.Count()];
                int index = 0;
                foreach (var item in result)
                {
                    values[index++] = RedisValue.Unbox(item);
                }
                return values;
            }
            catch (Exception)
            {
            }
            return null;
        }

        public RedisValue[] SetCombine(SetOperation operation, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            try
            {

                string tempKey = Guid.NewGuid().ToString();
                var tempSet = _cache.DataTypeManager.CreateHashSet<object>(tempKey);
                foreach (var key in keys)
                {
                    switch (operation)
                    {
                        case SetOperation.Union:
                            tempSet.StoreUnion(tempKey, key);
                            break;
                        case SetOperation.Intersect:
                            tempSet.StoreIntersection(tempKey, key);
                            break;
                        case SetOperation.Difference:
                            tempSet.StoreDifference(tempKey, key);
                            break;
                    }
                }

                RedisValue[] values = new RedisValue[
                tempSet.Count];
                int index = 0;
                foreach (var item in tempSet)
                {
                    values[index++] = RedisValue.Unbox(item);
                }
                _cache.DataTypeManager.Remove(tempKey);
                return values;
            }
            catch (Exception)
            {
            }
            return null;
        }

        public long SetCombineAndStore(SetOperation operation, RedisKey destination, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
        {
            string tempKey = Guid.NewGuid().ToString();
            var destinationSet = _cache.DataTypeManager.GetHashSet<object>(destination);

            destinationSet = destinationSet == null ? _cache.DataTypeManager.CreateHashSet<object>(destination) : destinationSet;
            try
            {
                destinationSet.Clear();
                switch (operation)
                {
                    case SetOperation.Union:
                        destinationSet.StoreUnion(first, second);
                        break;
                    case SetOperation.Intersect:
                        destinationSet.StoreIntersection(first, second);
                        break;
                    case SetOperation.Difference:
                        destinationSet.StoreDifference(first, second);
                        break;
                }

            }
            catch (Exception)
            {
            }
            return destinationSet.Count;
        }

        public long SetCombineAndStore(SetOperation operation, RedisKey destination, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            string tempKey = Guid.NewGuid().ToString();
            var destinationSet = _cache.DataTypeManager.GetHashSet<object>(destination);

            destinationSet = destinationSet == null ? _cache.DataTypeManager.CreateHashSet<object>(destination) : destinationSet;
            try
            {
                destinationSet.Clear();
                foreach (var key in keys)
                {
                    switch (operation)
                    {
                        case SetOperation.Union:
                            destinationSet.StoreUnion(destination, key);
                            break;
                        case SetOperation.Intersect:
                            destinationSet.StoreIntersection(destination, key);
                            break;
                        case SetOperation.Difference:
                            destinationSet.StoreDifference(destination, key);
                            break;
                    }
                }

            }
            catch (Exception)
            {
            }
            return destinationSet.Count;
        }

        public Task<long> SetCombineAndStoreAsync(SetOperation operation, RedisKey destination, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return SetCombineAndStore(operation, destination, first, second, flags);
            });
        }

        public Task<long> SetCombineAndStoreAsync(SetOperation operation, RedisKey destination, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return SetCombineAndStore(operation, destination, keys, flags);
            });
        }

        public Task<RedisValue[]> SetCombineAsync(SetOperation operation, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return SetCombine(operation, first, second, flags);
            });
        }

        public Task<RedisValue[]> SetCombineAsync(SetOperation operation, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return SetCombine(operation, keys, flags);
            });
        }

        public bool SetContains(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                var set = _cache.DataTypeManager.GetHashSet<object>(key);
                if (set == null)
                    return false;
                return set.Contains(value.Box());
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Task<bool> SetContainsAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return SetContains(key, value, flags);
            });
        }

        public long SetLength(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                var set = _cache.DataTypeManager.GetHashSet<object>(key);
                if (set == null)
                    return 0;
                return set.Count();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public Task<long> SetLengthAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return SetLength(key, flags);
            });
        }

        public RedisValue[] SetMembers(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                var set = _cache.DataTypeManager.GetHashSet<object>(key);
                if (set == null)
                    return null;
                List<RedisValue> values = new List<RedisValue>();
                foreach (var member in set)
                {
                    values.Add(RedisValue.Unbox(member));
                }
                return values.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Task<RedisValue[]> SetMembersAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return SetMembers(key, flags);
            });
        }

        public bool SetMove(RedisKey source, RedisKey destination, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                var sourceSet = _cache.DataTypeManager.GetHashSet<object>(source);
                var destinationSet = _cache.DataTypeManager.GetHashSet<object>(destination);
                if (sourceSet == null || destinationSet == null)
                    return false;
                sourceSet.Remove(value.Box());
                destinationSet.Add(value.Box());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Task<bool> SetMoveAsync(RedisKey source, RedisKey destination, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return SetMove(source, destination, value, flags);
            });
        }

        public RedisValue SetPop(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                var set = _cache.DataTypeManager.GetHashSet<object>(key);
                if (set == null)
                    return new RedisValue();

                return RedisValue.Unbox(set.RemoveRandom());
            }
            catch (Exception)
            {
                return new RedisValue();
            }
        }

        public RedisValue[] SetPop(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                var set = _cache.DataTypeManager.GetHashSet<object>(key);
                if (set == null)
                    return null;
                var values = set.GetRandom((int)count);
                List<RedisValue> redisValues = new List<RedisValue>();
                foreach (var value in values)
                {
                    set.Remove(value);
                    redisValues.Add(RedisValue.Unbox(value));
                }
                return redisValues.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Task<RedisValue> SetPopAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return SetPop(key, flags);
            });
        }

        public Task<RedisValue[]> SetPopAsync(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return SetPop(key, count, flags);
            });
        }

        public RedisValue SetRandomMember(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                var set = _cache.DataTypeManager.GetHashSet<object>(key);
                if (set == null)
                    return new RedisValue();
                ;
                return RedisValue.Unbox(set.GetRandom());
            }
            catch (Exception)
            {
                return new RedisValue();
            }
        }

        public Task<RedisValue> SetRandomMemberAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return SetRandomMember(key, flags);
            });
        }

        public RedisValue[] SetRandomMembers(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                var set = _cache.DataTypeManager.GetHashSet<object>(key);
                if (set == null)
                    return null;
                var values = set.GetRandom((int)count);
                List<RedisValue> redisValues = new List<RedisValue>();
                foreach (var value in values)
                {
                    redisValues.Add(RedisValue.Unbox(value));
                }
                return redisValues.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Task<RedisValue[]> SetRandomMembersAsync(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return SetRandomMembers(key, count, flags);
            });
        }

        public bool SetRemove(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                var set = _cache.DataTypeManager.GetHashSet<object>(key);
                if (set == null)
                    return false;
                return set.Remove(value.Box());
            }
            catch (Exception)
            {
                return false;
            }
        }

        public long SetRemove(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                var set = _cache.DataTypeManager.GetHashSet<object>(key);
                if (set == null)
                    return 0;
                List<Object> itemsToRemove = new List<object>();
                foreach (var value in values)
                {
                    itemsToRemove.Add(value.Box());
                }
                return set.Remove(itemsToRemove);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public Task<bool> SetRemoveAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return SetRemove(key, value, flags);
            });
        }

        public Task<long> SetRemoveAsync(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return SetRemove(key, values, flags);
            });
        }

        public IEnumerable<RedisValue> SetScan(RedisKey key, RedisValue pattern, int pageSize, CommandFlags flags)
        {
            throw new NotSupportedException();
        }

        public IEnumerable<RedisValue> SetScan(RedisKey key, RedisValue pattern = default, int pageSize = 250, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public IAsyncEnumerable<RedisValue> SetScanAsync(RedisKey key, RedisValue pattern = default, int pageSize = 250, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public RedisValue[] Sort(RedisKey key, long skip = 0, long take = -1, Order order = Order.Ascending, SortType sortType = SortType.Numeric, RedisValue by = default, RedisValue[] get = null, CommandFlags flags = CommandFlags.None)
        {
            var list = _cache.DataTypeManager.GetList<object>(key)?.ToList<object>();
            if (list == null)
            {
                list = _cache.DataTypeManager.GetHashSet<object>(key)?.ToList<object>();
                if (list == null)
                    return null;
            }
            list.Sort();
            var index = 0;
            RedisValue[] redisValues = new RedisValue[list.Count];
            foreach (var item in list)
            {
                redisValues[index++] = RedisValue.Unbox(item);
            }
            return redisValues;
        }

        public long SortAndStore(RedisKey destination, RedisKey key, long skip = 0, long take = -1, Order order = Order.Ascending, SortType sortType = SortType.Numeric, RedisValue by = default, RedisValue[] get = null, CommandFlags flags = CommandFlags.None)
        {
            bool isList = true;
            var list = _cache.DataTypeManager.GetList<object>(key)?.ToList<object>();
            if (list == null)
            {
                isList = false;
                list = _cache.DataTypeManager.GetHashSet<object>(key)?.ToList<object>();
                if (list == null)
                    return 0;
            }
            list.Sort();
            if (isList)
            {
                var destinationList = _cache.DataTypeManager.GetList<object>(destination);
                if (destinationList == null)
                    destinationList = _cache.DataTypeManager.CreateList<object>(destination);
                destinationList.AddRange(list);
                return destinationList.Count();
            }
            else
            {
                var destinationSet = _cache.DataTypeManager.GetHashSet<object>(destination);
                if (destinationSet == null)
                    destinationSet = _cache.DataTypeManager.CreateHashSet<object>(destination);
                destinationSet.AddRange(list);
                return destinationSet.Count();
            }
        }

        public Task<long> SortAndStoreAsync(RedisKey destination, RedisKey key, long skip = 0, long take = -1, Order order = Order.Ascending, SortType sortType = SortType.Numeric, RedisValue by = default, RedisValue[] get = null, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return SortAndStore(destination, key);
            });
        }

        public Task<RedisValue[]> SortAsync(RedisKey key, long skip = 0, long take = -1, Order order = Order.Ascending, SortType sortType = SortType.Numeric, RedisValue by = default, RedisValue[] get = null, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return Sort(key);
            });
        }

        public bool SortedSetAdd(RedisKey key, RedisValue member, double score, CommandFlags flags)
        {
            throw new NotSupportedException();
        }

        public bool SortedSetAdd(RedisKey key, RedisValue member, double score, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public long SortedSetAdd(RedisKey key, SortedSetEntry[] values, CommandFlags flags)
        {
            throw new NotSupportedException();
        }

        public long SortedSetAdd(RedisKey key, SortedSetEntry[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<bool> SortedSetAddAsync(RedisKey key, RedisValue member, double score, CommandFlags flags)
        {
            throw new NotSupportedException();
        }

        public Task<bool> SortedSetAddAsync(RedisKey key, RedisValue member, double score, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<long> SortedSetAddAsync(RedisKey key, SortedSetEntry[] values, CommandFlags flags)
        {
            throw new NotSupportedException();
        }

        public Task<long> SortedSetAddAsync(RedisKey key, SortedSetEntry[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public long SortedSetCombineAndStore(SetOperation operation, RedisKey destination, RedisKey first, RedisKey second, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public long SortedSetCombineAndStore(SetOperation operation, RedisKey destination, RedisKey[] keys, double[] weights = null, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<long> SortedSetCombineAndStoreAsync(SetOperation operation, RedisKey destination, RedisKey first, RedisKey second, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<long> SortedSetCombineAndStoreAsync(SetOperation operation, RedisKey destination, RedisKey[] keys, double[] weights = null, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public double SortedSetDecrement(RedisKey key, RedisValue member, double value, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<double> SortedSetDecrementAsync(RedisKey key, RedisValue member, double value, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public double SortedSetIncrement(RedisKey key, RedisValue member, double value, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<double> SortedSetIncrementAsync(RedisKey key, RedisValue member, double value, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public long SortedSetLength(RedisKey key, double min = double.NegativeInfinity, double max = double.PositiveInfinity, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<long> SortedSetLengthAsync(RedisKey key, double min = double.NegativeInfinity, double max = double.PositiveInfinity, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public long SortedSetLengthByValue(RedisKey key, RedisValue min, RedisValue max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<long> SortedSetLengthByValueAsync(RedisKey key, RedisValue min, RedisValue max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public SortedSetEntry? SortedSetPop(RedisKey key, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public SortedSetEntry[] SortedSetPop(RedisKey key, long count, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<SortedSetEntry?> SortedSetPopAsync(RedisKey key, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<SortedSetEntry[]> SortedSetPopAsync(RedisKey key, long count, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public RedisValue[] SortedSetRangeByRank(RedisKey key, long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<RedisValue[]> SortedSetRangeByRankAsync(RedisKey key, long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public SortedSetEntry[] SortedSetRangeByRankWithScores(RedisKey key, long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<SortedSetEntry[]> SortedSetRangeByRankWithScoresAsync(RedisKey key, long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public RedisValue[] SortedSetRangeByScore(RedisKey key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<RedisValue[]> SortedSetRangeByScoreAsync(RedisKey key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public SortedSetEntry[] SortedSetRangeByScoreWithScores(RedisKey key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<SortedSetEntry[]> SortedSetRangeByScoreWithScoresAsync(RedisKey key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public RedisValue[] SortedSetRangeByValue(RedisKey key, RedisValue min, RedisValue max, Exclude exclude, long skip, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public RedisValue[] SortedSetRangeByValue(RedisKey key, RedisValue min = default, RedisValue max = default, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<RedisValue[]> SortedSetRangeByValueAsync(RedisKey key, RedisValue min, RedisValue max, Exclude exclude, long skip, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<RedisValue[]> SortedSetRangeByValueAsync(RedisKey key, RedisValue min = default, RedisValue max = default, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public long? SortedSetRank(RedisKey key, RedisValue member, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<long?> SortedSetRankAsync(RedisKey key, RedisValue member, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public bool SortedSetRemove(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public long SortedSetRemove(RedisKey key, RedisValue[] members, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<bool> SortedSetRemoveAsync(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<long> SortedSetRemoveAsync(RedisKey key, RedisValue[] members, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public long SortedSetRemoveRangeByRank(RedisKey key, long start, long stop, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<long> SortedSetRemoveRangeByRankAsync(RedisKey key, long start, long stop, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public long SortedSetRemoveRangeByScore(RedisKey key, double start, double stop, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<long> SortedSetRemoveRangeByScoreAsync(RedisKey key, double start, double stop, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public long SortedSetRemoveRangeByValue(RedisKey key, RedisValue min, RedisValue max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<long> SortedSetRemoveRangeByValueAsync(RedisKey key, RedisValue min, RedisValue max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public IEnumerable<SortedSetEntry> SortedSetScan(RedisKey key, RedisValue pattern, int pageSize, CommandFlags flags)
        {
            throw new NotSupportedException();
        }

        public IEnumerable<SortedSetEntry> SortedSetScan(RedisKey key, RedisValue pattern = default, int pageSize = 250, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public IAsyncEnumerable<SortedSetEntry> SortedSetScanAsync(RedisKey key, RedisValue pattern = default, int pageSize = 250, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public double? SortedSetScore(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<double?> SortedSetScoreAsync(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public long StreamAcknowledge(RedisKey key, RedisValue groupName, RedisValue messageId, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public long StreamAcknowledge(RedisKey key, RedisValue groupName, RedisValue[] messageIds, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<long> StreamAcknowledgeAsync(RedisKey key, RedisValue groupName, RedisValue messageId, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<long> StreamAcknowledgeAsync(RedisKey key, RedisValue groupName, RedisValue[] messageIds, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public RedisValue StreamAdd(RedisKey key, RedisValue streamField, RedisValue streamValue, RedisValue? messageId = null, int? maxLength = null, bool useApproximateMaxLength = false, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public RedisValue StreamAdd(RedisKey key, NameValueEntry[] streamPairs, RedisValue? messageId = null, int? maxLength = null, bool useApproximateMaxLength = false, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<RedisValue> StreamAddAsync(RedisKey key, RedisValue streamField, RedisValue streamValue, RedisValue? messageId = null, int? maxLength = null, bool useApproximateMaxLength = false, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<RedisValue> StreamAddAsync(RedisKey key, NameValueEntry[] streamPairs, RedisValue? messageId = null, int? maxLength = null, bool useApproximateMaxLength = false, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public StreamEntry[] StreamClaim(RedisKey key, RedisValue consumerGroup, RedisValue claimingConsumer, long minIdleTimeInMs, RedisValue[] messageIds, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<StreamEntry[]> StreamClaimAsync(RedisKey key, RedisValue consumerGroup, RedisValue claimingConsumer, long minIdleTimeInMs, RedisValue[] messageIds, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public RedisValue[] StreamClaimIdsOnly(RedisKey key, RedisValue consumerGroup, RedisValue claimingConsumer, long minIdleTimeInMs, RedisValue[] messageIds, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<RedisValue[]> StreamClaimIdsOnlyAsync(RedisKey key, RedisValue consumerGroup, RedisValue claimingConsumer, long minIdleTimeInMs, RedisValue[] messageIds, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public bool StreamConsumerGroupSetPosition(RedisKey key, RedisValue groupName, RedisValue position, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<bool> StreamConsumerGroupSetPositionAsync(RedisKey key, RedisValue groupName, RedisValue position, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public StreamConsumerInfo[] StreamConsumerInfo(RedisKey key, RedisValue groupName, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<StreamConsumerInfo[]> StreamConsumerInfoAsync(RedisKey key, RedisValue groupName, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public bool StreamCreateConsumerGroup(RedisKey key, RedisValue groupName, RedisValue? position, CommandFlags flags)
        {
            throw new NotSupportedException();
        }

        public bool StreamCreateConsumerGroup(RedisKey key, RedisValue groupName, RedisValue? position = null, bool createStream = true, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<bool> StreamCreateConsumerGroupAsync(RedisKey key, RedisValue groupName, RedisValue? position, CommandFlags flags)
        {
            throw new NotSupportedException();
        }

        public Task<bool> StreamCreateConsumerGroupAsync(RedisKey key, RedisValue groupName, RedisValue? position = null, bool createStream = true, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public long StreamDelete(RedisKey key, RedisValue[] messageIds, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<long> StreamDeleteAsync(RedisKey key, RedisValue[] messageIds, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public long StreamDeleteConsumer(RedisKey key, RedisValue groupName, RedisValue consumerName, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<long> StreamDeleteConsumerAsync(RedisKey key, RedisValue groupName, RedisValue consumerName, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public bool StreamDeleteConsumerGroup(RedisKey key, RedisValue groupName, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<bool> StreamDeleteConsumerGroupAsync(RedisKey key, RedisValue groupName, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public StreamGroupInfo[] StreamGroupInfo(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<StreamGroupInfo[]> StreamGroupInfoAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public StreamInfo StreamInfo(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<StreamInfo> StreamInfoAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public long StreamLength(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<long> StreamLengthAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public StreamPendingInfo StreamPending(RedisKey key, RedisValue groupName, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<StreamPendingInfo> StreamPendingAsync(RedisKey key, RedisValue groupName, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public StreamPendingMessageInfo[] StreamPendingMessages(RedisKey key, RedisValue groupName, int count, RedisValue consumerName, RedisValue? minId = null, RedisValue? maxId = null, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<StreamPendingMessageInfo[]> StreamPendingMessagesAsync(RedisKey key, RedisValue groupName, int count, RedisValue consumerName, RedisValue? minId = null, RedisValue? maxId = null, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public StreamEntry[] StreamRange(RedisKey key, RedisValue? minId = null, RedisValue? maxId = null, int? count = null, Order messageOrder = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<StreamEntry[]> StreamRangeAsync(RedisKey key, RedisValue? minId = null, RedisValue? maxId = null, int? count = null, Order messageOrder = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public StreamEntry[] StreamRead(RedisKey key, RedisValue position, int? count = null, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public RedisStream[] StreamRead(StreamPosition[] streamPositions, int? countPerStream = null, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<StreamEntry[]> StreamReadAsync(RedisKey key, RedisValue position, int? count = null, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<RedisStream[]> StreamReadAsync(StreamPosition[] streamPositions, int? countPerStream = null, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public StreamEntry[] StreamReadGroup(RedisKey key, RedisValue groupName, RedisValue consumerName, RedisValue? position, int? count, CommandFlags flags)
        {
            throw new NotSupportedException();
        }

        public StreamEntry[] StreamReadGroup(RedisKey key, RedisValue groupName, RedisValue consumerName, RedisValue? position = null, int? count = null, bool noAck = false, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public RedisStream[] StreamReadGroup(StreamPosition[] streamPositions, RedisValue groupName, RedisValue consumerName, int? countPerStream, CommandFlags flags)
        {
            throw new NotSupportedException();
        }

        public RedisStream[] StreamReadGroup(StreamPosition[] streamPositions, RedisValue groupName, RedisValue consumerName, int? countPerStream = null, bool noAck = false, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<StreamEntry[]> StreamReadGroupAsync(RedisKey key, RedisValue groupName, RedisValue consumerName, RedisValue? position, int? count, CommandFlags flags)
        {
            throw new NotSupportedException();
        }

        public Task<StreamEntry[]> StreamReadGroupAsync(RedisKey key, RedisValue groupName, RedisValue consumerName, RedisValue? position = null, int? count = null, bool noAck = false, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<RedisStream[]> StreamReadGroupAsync(StreamPosition[] streamPositions, RedisValue groupName, RedisValue consumerName, int? countPerStream, CommandFlags flags)
        {
            throw new NotSupportedException();
        }

        public Task<RedisStream[]> StreamReadGroupAsync(StreamPosition[] streamPositions, RedisValue groupName, RedisValue consumerName, int? countPerStream = null, bool noAck = false, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public long StreamTrim(RedisKey key, int maxLength, bool useApproximateMaxLength = false, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<long> StreamTrimAsync(RedisKey key, int maxLength, bool useApproximateMaxLength = false, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public long StringAppend(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            string str = _cache.Contains(key) ? _cache.Get<string>(key) : null;

            try
            {
                if (str == null)
                {
                    str = value;
                    _cache.Add(key, str);
                }
                else
                {
                    str += value;
                    _cache.Insert(key, str);
                }
            }
            catch (Exception)
            {
            }
            return str.Length;
        }

        public Task<long> StringAppendAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return StringAppend(key, value, flags);
            });
        }
        static int countSetBits(char n)
        {
            int count = 0;
            while (n > 0)
            {
                count += n & 1;
                n >>= 1;
            }
            return count;
        }
        public long StringBitCount(RedisKey key, long start = 0, long end = -1, CommandFlags flags = CommandFlags.None)
        {
            string str = RedisValue.Unbox(_cache.Get<object>(key));
            try
            {
                if(str != null)
                {
                    int bitSetCount = 0;
                    for (long i = start; i <( end < 0 ? str.Length : end); i++)
                    {
                        bitSetCount += countSetBits(str[(int)i]);
                    }
                    return bitSetCount;
                }
            }
            catch (Exception)
            {
            }
            return 0;
        }

        public Task<long> StringBitCountAsync(RedisKey key, long start = 0, long end = -1, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return StringBitCount(key, start, end, flags);
            });
        }

        public long StringBitOperation(Bitwise operation, RedisKey destination, RedisKey first, RedisKey second = default, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public long StringBitOperation(Bitwise operation, RedisKey destination, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<long> StringBitOperationAsync(Bitwise operation, RedisKey destination, RedisKey first, RedisKey second = default, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<long> StringBitOperationAsync(Bitwise operation, RedisKey destination, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public long StringBitPosition(RedisKey key, bool bit, long start = 0, long end = -1, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<long> StringBitPositionAsync(RedisKey key, bool bit, long start = 0, long end = -1, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public long StringDecrement(RedisKey key, long value = 1, CommandFlags flags = CommandFlags.None)
        {

            try
            {
                long n = value;
                if (_cache.Contains(key))
                {
                    n = int.Parse(RedisValue.Unbox(_cache.Get<object>(key)));
                    n += value;
                }

                _cache.Insert(key, n.ToString());
                return n;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public double StringDecrement(RedisKey key, double value, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                double n = value;
                if (_cache.Contains(key))
                {
                    n = double.Parse(RedisValue.Unbox(_cache.Get<object>(key)));
                    n += value;
                }

                _cache.Insert(key, n.ToString());
                return n;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<long> StringDecrementAsync(RedisKey key, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return StringDecrement(key, value, flags);
            });
        }

        public Task<double> StringDecrementAsync(RedisKey key, double value, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return StringDecrement(key, value, flags);
            });
        }

        public RedisValue StringGet(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            var obj = _cache.Get<Object>(key.ToString());
            return RedisValue.Unbox(obj);
        }

        public RedisValue[] StringGet(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                var keysValues = _cache.GetBulk<object>(RedisKeysToStringList(keys));
                RedisValue[] redisValues = new RedisValue[keysValues.Count];
                for (int i = 0; i < keysValues.Count; i++)
                {
                    redisValues[i] = RedisValue.Unbox(keysValues[keys[i].ToString()]);
                }
                return redisValues;
            }
            catch (Exception)
            {

                return null;
            };
        }

        public Task<RedisValue> StringGetAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return StringGet(key, flags);
            });
        }

        public Task<RedisValue[]> StringGetAsync(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return StringGet(keys, flags);
            });
        }

        public bool StringGetBit(RedisKey key, long offset, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<bool> StringGetBitAsync(RedisKey key, long offset, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Lease<byte> StringGetLease(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<Lease<byte>> StringGetLeaseAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public RedisValue StringGetRange(RedisKey key, long start, long end, CommandFlags flags = CommandFlags.None)
        {
            RedisValue ret = new RedisValue();
            try
            {
                if (_cache.Contains(key))
                {
                    var val = (string)RedisValue.Unbox(_cache.Get<object>(key));
                    ret = val.Substring((int)start, (int)(end < 0 ? val.Length - 1 - end : val.Length - 1));
                }
            }
            catch (Exception)
            {
            }

            return ret;
        }

        public Task<RedisValue> StringGetRangeAsync(RedisKey key, long start, long end, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return StringGetRange(key, start, end, flags);
            });
        }

        public RedisValue StringGetSet(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            var val = new RedisValue();
            try
            {
                val = RedisValue.Unbox(_cache.Get<object>(key));
                _cache.Insert(key, value.Box());
            }
            catch (Exception)
            {
            }

            return val;
        }

        public Task<RedisValue> StringGetSetAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return StringGetSet(key, value, flags);
            });
        }

        public RedisValueWithExpiry StringGetWithExpiry(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<RedisValueWithExpiry> StringGetWithExpiryAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public long StringIncrement(RedisKey key, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            return StringDecrement(key, -value, flags);
        }

        public double StringIncrement(RedisKey key, double value, CommandFlags flags = CommandFlags.None)
        {
            return StringDecrement(key, -value, flags);
        }

        public Task<long> StringIncrementAsync(RedisKey key, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return StringIncrement(key, value, flags);
            });
        }

        public Task<double> StringIncrementAsync(RedisKey key, double value, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return StringIncrement(key, value, flags);
            });
        }

        public long StringLength(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<long> StringLengthAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public bool StringSet(RedisKey key, RedisValue value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                var cacheItem = new CacheItem(value);
                if (expiry != null)
                    cacheItem.Expiration = new Alachisoft.NCache.Runtime.Caching.Expiration(Alachisoft.NCache.Runtime.Caching.ExpirationType.Sliding, expiry.Value);
                _cache.Insert(key, value.Box());

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool StringSet(KeyValuePair<RedisKey, RedisValue>[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            try
            {
                RedisKey[] keys = new RedisKey[values.Count()];
                IDictionary<string, CacheItem> items = new Dictionary<string, CacheItem>();
                for (int i = 0; i < values.Count(); i++)
                {
                    keys[i] = values[i].Key;
                    items.Add(values[i].ToString(), new CacheItem(values[i].Value.Box()));
                }
                var contains = _cache.ContainsBulk(RedisKeysToStringList(keys));
                if (contains.Where(s => s.Value == false).Count() > 0)
                    return false;
                _cache.InsertBulk(items);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Task<bool> StringSetAsync(RedisKey key, RedisValue value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return StringSet(key, value, expiry, when, flags);
            });
        }

        public Task<bool> StringSetAsync(KeyValuePair<RedisKey, RedisValue>[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                return StringSet(values, when, flags);
            });
        }

        public bool StringSetBit(RedisKey key, long offset, bool bit, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<bool> StringSetBitAsync(RedisKey key, long offset, bool bit, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public RedisValue StringSetRange(RedisKey key, long offset, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public Task<RedisValue> StringSetRangeAsync(RedisKey key, long offset, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            throw new NotSupportedException();
        }

        public bool TryWait(Task task)
        {
            throw new NotSupportedException();
        }

        public void Wait(Task task)
        {
            throw new NotSupportedException();
        }

        public T Wait<T>(Task<T> task)
        {
            throw new NotSupportedException();
        }

        public void WaitAll(params Task[] tasks)
        {
            throw new NotSupportedException();
        }

        public ChannelMessageQueue Subscribe(RedisChannel topicName)
        {
            try
            {
                return new ChannelMessageQueue(_cache, topicName);
            }
            catch (Exception)
            {
            }

            return null;
        }

        public void UnSubscribe(ChannelMessageQueue channel)
        {
            try
            {
                channel.Unsubscribe();
            }
            catch (Exception)
            {
            }
        }
    }
}
