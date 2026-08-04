using System;
using System.Collections.Generic;
using System.Text;

using Native.StackExchange.Redis.Sample;

namespace Native.StackExchange.Redis.Sample.BasicCacheOperations
{
    class StringGetTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void GetExistingKey()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting existing key from cache");
 
                var key = Guid.NewGuid().ToString();
                Program.db.StringSet(key, Program.myObjectForCaching);
                var result = Program.db.StringGet(key);

                if (!result.IsNull)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got item from cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get item from cache");
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

        public static void GetNonExistingKey()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting non-existing key from cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.StringGet(key);

                if (!result.IsNull)
                {
                    Logger.PrintFailureOutcome("Successfully got Item from cache");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to get item from cache");
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

        public static void GetExistingKeyAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting existing key Asynchronously from cache");

                var key = Guid.NewGuid().ToString();
                Program.db.StringSet(key, Program.myObjectForCaching);
                var result = Program.db.StringGetAsync(key);

                result.Wait();

                if (!result.Result.IsNull)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got item from cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get item from cache");
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

        public static void GetNonExistingKeyAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting non-existing key Asynchronously from cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.StringGetAsync(key);

                result.Wait();

                if (!result.Result.IsNull)
                {
                    Logger.PrintFailureOutcome("Successfully got item from cache");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to get item from cache");
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
