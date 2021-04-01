using BasicUsageStackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCache.StackExchange.Redis.Sample.KeyOperations
{
    class KeyTouchTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        internal static void TouchExistingKey()
        {
            try
            {
                Logger.PrintTestStartInformation("Touching already existing key in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.StringSet(key, Program.myObjectForCaching);

                if (Program.db.KeyTouch(key))
                {
                    Logger.PrintSuccessfulOutcome("Successfully touched the key in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to touch the key in cache");
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

        internal static void TouchNonExistingKey()
        {
            try
            {
                Logger.PrintTestStartInformation("Touching non-existing key in cache");

                var key = Guid.NewGuid().ToString();

                if (Program.db.KeyTouch(key))
                {
                    Logger.PrintFailureOutcome("Successfully touched the key in cache");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to touch the key in cache");
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

        internal static void TouchExistingKeyAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously touching already existing key in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.StringSet(key, Program.myObjectForCaching);

                var result = Program.db.KeyTouchAsync(key);

                result.Wait();

                if (result.Result)
                {
                    Logger.PrintSuccessfulOutcome("Successfully touched the key in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to touch the key in cache");
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

        internal static void TouchNonExistingKeyAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously touching non-existing key in cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.KeyTouchAsync(key);

                result.Wait();

                if (result.Result)
                {
                    Logger.PrintFailureOutcome("Successfully touched the key in cache");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to touch the key in cache");
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
