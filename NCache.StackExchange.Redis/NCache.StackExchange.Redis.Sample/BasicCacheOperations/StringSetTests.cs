using System;
using System.Collections.Generic;
using System.Text;
using BasicUsageStackExchangeRedis;
using NCache.StackExchange.Redis.Sample;

namespace NCache.StackExchange.Redis.Sample.BasicCacheOperations
{
    class StringSetTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void AddKeyValuePair()
        {
            try
            {
                Logger.PrintTestStartInformation("Adding non-existing key-value pair into cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.StringSet(key, Program.myObjectForCaching);

                if (result)
                {
                    Logger.PrintSuccessfulOutcome("Item successfully added to cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to add item to cache");
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

        public static void AddExistingKeyValuePair()
        {
            try
            {
                Logger.PrintTestStartInformation("Adding already existing key-value pair into cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.StringSet(key, Program.myObjectForCaching);

                result = Program.db.StringSet(key, Program.myObjectForCaching);

                if (result)
                {
                    Logger.PrintSuccessfulOutcome("Item successfully added to cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to add item to cache");
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

        public static void AddKeyValuePairAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Adding non-existing key-value pair Asynchronously into cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.StringSetAsync(key, Program.myObjectForCaching);

                result.Wait();

                if (result.Result)
                {
                    Logger.PrintSuccessfulOutcome("Item successfully added to cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to add item to cache");
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

        public static void AddExistingKeyValuePairAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Adding already existing key-value pair Asynchronously into cache");

                var key = Guid.NewGuid().ToString();
                Program.db.StringSet(key, Program.myObjectForCaching);

                var result = Program.db.StringSetAsync(key, Program.myObjectForCaching);

                result.Wait();

                if (result.Result)
                {
                    Logger.PrintSuccessfulOutcome("Item successfully added to cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to add item to cache");
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
