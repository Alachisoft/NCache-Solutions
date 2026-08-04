using BasicUsageStackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCache.StackExchange.Redis.Sample.BasicCacheOperations
{
    class KeyDeleteTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void DeleteExistingKey()
        {
            try
            {
                Logger.PrintTestStartInformation("Deleting already existing key from cache");

                var key = Guid.NewGuid().ToString();
                Program.db.StringSet(key, Program.myObjectForCaching);
                var result = Program.db.KeyDelete(key);

                if (result)
                {
                    Logger.PrintSuccessfulOutcome("Item successfully deleted from cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to delete item from cache");
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

        public static void DeleteNonExistingKey()
        {
            try
            {
                Logger.PrintTestStartInformation("Deleting non-existing key from cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.KeyDelete(key);

                if (result)
                {
                    Logger.PrintFailureOutcome("Item successfully deleted from cache");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to delete item from cache");
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

        public static void DeleteExistingKeyAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously deleting already existing key from cache");

                var key = Guid.NewGuid().ToString();
                Program.db.StringSet(key, Program.myObjectForCaching);
                var result = Program.db.KeyDeleteAsync(key);

                result.Wait();

                if (result.Result)
                {
                    Logger.PrintSuccessfulOutcome("Item successfully deleted from cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to delete item from cache");
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

        public static void DeleteNonExistingKeyAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously deleting non-existing key from cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.KeyDeleteAsync(key);

                result.Wait();

                if (result.Result)
                {
                    Logger.PrintFailureOutcome("Item successfully deleted from cache");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to delete item from cache");
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
