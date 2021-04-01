
using System;
using System.Collections.Generic;
using System.Text;

namespace Native.StackExchange.Redis.Sample.CacheContainsOperations
{
    class KeyExistsTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        internal static void ContainsExistingKey()
        {
            try
            {
                Logger.PrintTestStartInformation("Checking already existing key presence in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.StringSet(key, Program.myObjectForCaching);

                if (Program.db.KeyExists(key))
                {
                    Logger.PrintSuccessfulOutcome("Key found in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to find key in cache");
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

        internal static void ContainsNonExistingKey()
        {
            try
            {
                Logger.PrintTestStartInformation("Checking already existing key presence in cache");

                var key = Guid.NewGuid().ToString();

                if (Program.db.KeyExists(key))
                {
                    Logger.PrintFailureOutcome("Key found in cache");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to find key in cache");
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

        internal static void ContainsExistingKeyAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Checking already existing key presence in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.StringSet(key, Program.myObjectForCaching);

                if (Program.db.KeyExistsAsync(key).Result)
                {
                    Logger.PrintSuccessfulOutcome("Key found in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to find key in cache");
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

        internal static void ContainsNonExistingKeyAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Checking already existing key presence in cache");

                var key = Guid.NewGuid().ToString();

                if (Program.db.KeyExistsAsync(key).Result)
                {
                    Logger.PrintFailureOutcome("Key found in cache");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to find key in cache");
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
