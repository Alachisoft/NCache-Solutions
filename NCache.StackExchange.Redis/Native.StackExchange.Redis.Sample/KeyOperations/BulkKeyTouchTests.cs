using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Native.StackExchange.Redis.Sample.KeyOperations
{
    class BulkKeyTouchTests
    {
        //------------------------------------------------Sync Methods------------------------------------------------\\

        internal static void TouchExistingKeysBulk()
        {
            try
            {
                Logger.PrintTestStartInformation("Touching already existing bulk of keys in cache");

                var keyValuePair = new KeyValuePair<RedisKey, RedisValue>[10];
                var keys = new RedisKey[10];

                for (int i = 0; i < 10; i++)
                {
                    keys[i] = Guid.NewGuid().ToString();
                    keyValuePair[i] = new KeyValuePair<RedisKey, RedisValue>(keys[i], Program.myObjectForCaching);
                }

                Program.db.StringSet(keyValuePair);

                var result = Program.db.KeyTouch(keys);

                if (result == 10)
                {
                    Logger.PrintSuccessfulOutcome("Successfully touched all keys in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome($"Unable to touch all keys in cache. Number of keys touched: {result}");
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

        internal static void TouchNonExistingKeysBulk()
        {
            try
            {
                Logger.PrintTestStartInformation("Touching non-existing bulk of keys in cache");

                var keys = new RedisKey[10];

                for (int i = 0; i < 10; i++)
                {
                    keys[i] = Guid.NewGuid().ToString();
                }

                var result = Program.db.KeyTouch(keys);

                if (result == 10)
                {
                    Logger.PrintFailureOutcome("All keys touched in cache");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome($"Unable to touch all keys in cache. Number of keys touch: {result}");
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

        internal static void TouchExistingKeysBulkAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously touching already existing bulk of keys in cache");

                var keyValuePair = new KeyValuePair<RedisKey, RedisValue>[10];
                var keys = new RedisKey[10];

                for (int i = 0; i < 10; i++)
                {
                    keys[i] = Guid.NewGuid().ToString();
                    keyValuePair[i] = new KeyValuePair<RedisKey, RedisValue>(keys[i], Program.myObjectForCaching);
                }

                Program.db.StringSet(keyValuePair);

                var result = Program.db.KeyTouchAsync(keys);

                result.Wait();

                if (result.Result == 10)
                {
                    Logger.PrintSuccessfulOutcome("All keys touched in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome($"Unable to touch all keys in cache. Number of keys touched: {result.Result}");
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

        internal static void TouchNonExistingKeysBulkAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Touching presence of non-existing bulk of keys in cache");

                var keys = new RedisKey[10];

                for (int i = 0; i < 10; i++)
                {
                    keys[i] = Guid.NewGuid().ToString();
                }

                var result = Program.db.KeyTouchAsync(keys);

                result.Wait();

                if (result.Result == 10)
                {
                    Logger.PrintFailureOutcome("All keys touched in cache");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome($"Unable to touch all keys in cache. Number of keys touched: {result.Result}");
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
