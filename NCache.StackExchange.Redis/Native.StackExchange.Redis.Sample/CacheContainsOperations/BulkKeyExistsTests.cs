using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Native.StackExchange.Redis.Sample.CacheContainsOperations
{
    class BulkKeyExistsTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        internal static void ContainsExistingKeysBulk()
        {
            try
            {
                Logger.PrintTestStartInformation("Checking presence of already existing bulk of keys in cache");
                
                var keyValuePair = new KeyValuePair<RedisKey, RedisValue>[10];
                var keys = new RedisKey[10];

                for (int i = 0; i < 10; i++)
                {
                    keys[i] = Guid.NewGuid().ToString();
                    keyValuePair[i] = new KeyValuePair<RedisKey, RedisValue>(keys[i], Program.myObjectForCaching);
                }

                Program.db.StringSet(keyValuePair);

                var result = Program.db.KeyExists(keys);

                if (result == 10)
                {
                    Logger.PrintSuccessfulOutcome("All keys found in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome($"Unable to find all keys in cache. Number of keys found: {result}");
                }
            }
            catch (Exception e)
            {
                Logger.PrintDataCacheException(e);
            }
            finally
            {
                Logger.PrintBreakLine();
            }
        }

        internal static void ContainsNonExistingKeysBulk()
        {
            try
            {
                Logger.PrintTestStartInformation("Checking presence of non-existing bulk of keys in cache");

                var keys = new RedisKey[10];

                for (int i = 0; i < 10; i++)
                {
                    keys[i] = Guid.NewGuid().ToString();
                }

                var result = Program.db.KeyExists(keys);

                if (result == 10)
                {
                    Logger.PrintFailureOutcome("All keys found in cache");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome($"Unable to find all keys in cache. Number of keys found: {result}");
                }
            }
            catch (Exception e)
            {
                Logger.PrintDataCacheException(e);
            }
            finally
            {
                Logger.PrintBreakLine();
            }
        }

        //------------------------------------------------Async Methods------------------------------------------------\\

        internal static void ContainsExistingKeysBulkAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously checking presence of already existing bulk of keys in cache");

                var keyValuePair = new KeyValuePair<RedisKey, RedisValue>[10];
                var keys = new RedisKey[10];

                for (int i = 0; i < 10; i++)
                {
                    keys[i] = Guid.NewGuid().ToString();
                    keyValuePair[i] = new KeyValuePair<RedisKey, RedisValue>(keys[i], Program.myObjectForCaching);
                }

                Program.db.StringSet(keyValuePair);

                var result = Program.db.KeyExistsAsync(keys);

                result.Wait();

                if (result.Result == 10)
                {
                    Logger.PrintSuccessfulOutcome("All keys found in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome($"Unable to find all keys in cache. Number of keys found: {result.Result}");
                }
            }
            catch (Exception e)
            {
                Logger.PrintDataCacheException(e);
            }
            finally
            {
                Logger.PrintBreakLine();
            }
        }

        internal static void ContainsNonExistingKeysBulkAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously checking presence of non-existing bulk of keys in cache");

                var keys = new RedisKey[10];

                for (int i = 0; i < 10; i++)
                {
                    keys[i] = Guid.NewGuid().ToString();
                }

                var result = Program.db.KeyExistsAsync(keys);

                result.Wait();

                if (result.Result == 10)
                {
                    Logger.PrintFailureOutcome("All keys found in cache");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome($"Unable to find all keys in cache. Number of keys found: {result.Result}");
                }
            }
            catch (Exception e)
            {
                Logger.PrintDataCacheException(e);
            }
            finally
            {
                Logger.PrintBreakLine();
            }
        }
    }
}
