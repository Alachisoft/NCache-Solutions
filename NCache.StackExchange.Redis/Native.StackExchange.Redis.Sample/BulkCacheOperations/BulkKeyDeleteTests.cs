using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Native.StackExchange.Redis.Sample.BulkCacheOperations
{
    class BulkKeyDeleteTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void DeleteExistingKeysBulk()
        {
            try
            {
                Logger.PrintTestStartInformation("Deleting bulk of existing keys from cache");

                var keys = new RedisKey[10];

                for (int i = 0; i < 10; i++)
                {
                    keys[i] = Guid.NewGuid().ToString();
                    Program.db.StringSet(keys[i], Program.myObjectForCaching);
                }

                var result = Program.db.KeyDelete(keys);

                if (result == 10)
                {
                    Logger.PrintSuccessfulOutcome("All keys were removed successfully");
                }
                else
                {
                    Logger.PrintFailureOutcome($"Unable to remove all keys. Number of keys removed: {result}");
                }
            }
            catch(Exception e)
            {
                Logger.PrintDataCacheException(e);
            }
            finally
            {
                Logger.PrintBreakLine();
            }
        }

        public static void DeleteNonExistingKeysBulk()
        {
            try
            {
                Logger.PrintTestStartInformation("Deleting bulk of non-existing keys from cache");

                var keys = new RedisKey[10];

                for (int i = 0; i < 10; i++)
                {
                    keys[i] = Guid.NewGuid().ToString();
                }

                var result = Program.db.KeyDelete(keys);

                if (result == 10)
                {
                    Logger.PrintFailureOutcome("All keys were removed successfully");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome($"Unable to remove all keys. Number of keys removed: {result}");
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

        public static void DeleteExistingKeysBulkAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously deleting bulk of existing keys from cache");

                var keys = new RedisKey[10];

                for (int i = 0; i < 10; i++)
                {
                    keys[i] = Guid.NewGuid().ToString();
                    Program.db.StringSet(keys[i], Program.myObjectForCaching);
                }

                var result = Program.db.KeyDeleteAsync(keys);
                result.Wait();

                if (result.Result == 10)
                {
                    Logger.PrintSuccessfulOutcome("All keys were removed successfully");
                }
                else
                {
                    Logger.PrintFailureOutcome($"Unable to remove all keys. Number of keys removed: {result.Result}");
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

        public static void DeleteNonExistingKeysBulkAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously deleting bulk of non-existing keys from cache");

                var keys = new RedisKey[10];

                for (int i = 0; i < 10; i++)
                {
                    keys[i] = Guid.NewGuid().ToString();
                }

                var result = Program.db.KeyDeleteAsync(keys);
                result.Wait();

                if (result.Result == 10)
                {
                    Logger.PrintFailureOutcome("All keys were removed successfully");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome($"Unable to remove all keys. Number of keys removed: {result.Result}");
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
