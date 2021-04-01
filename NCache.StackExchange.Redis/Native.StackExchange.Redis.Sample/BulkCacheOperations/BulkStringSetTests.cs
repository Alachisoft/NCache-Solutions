using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Native.StackExchange.Redis.Sample.BulkCacheOperations
{
    class BulkStringSetTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void AddNonExistingItemsBulk()
        {
            try
            {
                Logger.PrintTestStartInformation("Adding non-existing bulk of items in cache");

                var items = new KeyValuePair<RedisKey, RedisValue>[10];

                for (int i = 0; i < 10; i++)
                {
                    items[i] = new KeyValuePair<RedisKey, RedisValue>(Guid.NewGuid().ToString(), Program.myObjectForCaching);
                }

                var result = Program.db.StringSet(items);

                if (result)
                {
                    Logger.PrintSuccessfulOutcome("Successfully added non-existing bulk of items");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to add non-existing bulk of items");
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

        public static void AddExistingItemsBulk()
        {
            try
            {
                Logger.PrintTestStartInformation("Adding already existing bulk of items in cache");

                var items = new KeyValuePair<RedisKey, RedisValue>[10];

                for (int i = 0; i < 10; i++)
                {
                    items[i] = new KeyValuePair<RedisKey, RedisValue>(Guid.NewGuid().ToString(), Program.myObjectForCaching);
                }

                Program.db.StringSet(items);
                
                var result = Program.db.StringSet(items, When.NotExists);

                if (result)
                {
                    Logger.PrintFailureOutcome("Successfully added existing bulk of items");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to add existing bulk of items");
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

        public static void AddNonExistingItemsBulkAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously adding non-existing bulk of items in cache");

                var items = new KeyValuePair<RedisKey, RedisValue>[10];

                for (int i = 0; i < 10; i++)
                {
                    items[i] = new KeyValuePair<RedisKey, RedisValue>(Guid.NewGuid().ToString(), Program.myObjectForCaching);
                }

                var result = Program.db.StringSetAsync(items);

                result.Wait();

                if (result.Result)
                {
                    Logger.PrintSuccessfulOutcome("Successfully added non-existing bulk of items");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to add non-existing bulk of items");
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

        public static void AddExistingItemsBulkAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously adding already existing bulk of items in cache");

                var items = new KeyValuePair<RedisKey, RedisValue>[10];

                for (int i = 0; i < 10; i++)
                {
                    items[i] = new KeyValuePair<RedisKey, RedisValue>(Guid.NewGuid().ToString(), Program.myObjectForCaching);
                }

                Program.db.StringSet(items);

                var result = Program.db.StringSetAsync(items, When.NotExists);

                result.Wait();

                if (result.Result)
                {
                    Logger.PrintFailureOutcome("Successfully added existing bulk of items");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to add existing bulk of items");
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
