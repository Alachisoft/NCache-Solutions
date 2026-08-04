using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using StackExchange.Redis;

namespace Native.StackExchange.Redis.Sample.KeyOperations
{
    class KeyRandomTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void GetRandomKeyFromNonEmptyCache()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting random key from non-empty cache");

                var items = new KeyValuePair<RedisKey, RedisValue>[10];

                for (int i = 0; i < 10; i++)
                {
                    items[i] = new KeyValuePair<RedisKey, RedisValue>(Guid.NewGuid().ToString(), Program.myObjectForCaching);
                }

                Program.db.StringSet(items);

                var result = Program.db.KeyRandom();

                if (!string.IsNullOrEmpty(result.ToString()))
                {
                    Logger.PrintSuccessfulOutcome("Successfully got a random key from cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get a random key from cache");
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

        public static void GetRandomKeyFromEmptyCache()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting random key from empty cache");

                var server = Program.redis.GetServer(ConfigurationManager.AppSettings["ServerIP"]);
                server.FlushDatabase();

                var result = Program.db.KeyRandom();

                if (string.IsNullOrEmpty(result.ToString()))
                {
                    Logger.PrintFailureOutcome("Successfully got a random key from cache");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to get a random key from cache");
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

        public static void GetRandomKeyFromNonEmptyCacheAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting random key from non-empty cache");

                var items = new KeyValuePair<RedisKey, RedisValue>[10];

                for (int i = 0; i < 10; i++)
                {
                    items[i] = new KeyValuePair<RedisKey, RedisValue>(Guid.NewGuid().ToString(), Program.myObjectForCaching);
                }

                Program.db.StringSet(items);

                var result = Program.db.KeyRandomAsync();

                result.Wait();

                if (!string.IsNullOrEmpty(result.Result.ToString()))
                {
                    Logger.PrintSuccessfulOutcome("Successfully got a random key from cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get a random key from cache");
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

        public static void GetRandomKeyFromEmptyCacheAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting random key from empty cache");
                
                var server = Program.redis.GetServer(ConfigurationManager.AppSettings["ServerIP"]);
                server.FlushDatabase();

                var result = Program.db.KeyRandomAsync();

                result.Wait();

                if (string.IsNullOrEmpty(result.Result.ToString()))
                {
                    Logger.PrintFailureOutcome("Successfully got a random key from cache");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to get a random key from cache");
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
