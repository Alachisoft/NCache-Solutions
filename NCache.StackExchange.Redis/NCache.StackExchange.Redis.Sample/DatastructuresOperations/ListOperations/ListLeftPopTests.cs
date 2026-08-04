using BasicUsageStackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCache.StackExchange.Redis.Sample.DatastructuresOperations.ListOperations
{
    class ListLeftPopTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        internal static void LeftPopExistingItemFromExistingList()
        {
            try
            {
                Logger.PrintTestStartInformation("Left popping existing item from existing list in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.ListLeftPush(key, Program.myObjectForCaching);

                var result = Program.db.ListLeftPop(key);

                if (!result.IsNull)
                {
                    Logger.PrintSuccessfulOutcome("Successfully left popped existing item from existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to left pop existing item from existing list");
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

        internal static void LeftPopNonExistingItemFromExistingList()
        {
            try
            {
                Logger.PrintTestStartInformation("Left popping non-existing item from existing list in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.ListLeftPush(key, Program.myObjectForCaching);

                var result = Program.db.ListLeftPop(key);
                result = Program.db.ListLeftPop(key);

                if (!result.IsNull)
                {
                    Logger.PrintFailureOutcome("Successfully left popped non-existing item from existing list");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to left pop non-existing item from existing list");
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

        internal static void LeftPopItemFromNonExistingList()
        {
            try
            {
                Logger.PrintTestStartInformation("Left popping item from non-existing list in cache");

                var key = Guid.NewGuid().ToString();

                var result = Program.db.ListLeftPop(key);

                if (!result.IsNull)
                {
                    Logger.PrintFailureOutcome("Successfully left popped item from non-existing list");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to left pop item from non-existing list");
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

        internal static void LeftPopExistingItemFromExistingListAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronoisly left popping existing item from existing list in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.ListLeftPush(key, Program.myObjectForCaching);

                var result = Program.db.ListLeftPopAsync(key);

                result.Wait();

                if (!result.Result.IsNull)
                {
                    Logger.PrintSuccessfulOutcome("Successfully left popped existing item from existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to left pop existing item from existing list");
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

        internal static void LeftPopNonExistingItemFromExistingListAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously left popping non-existing item from existing list in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.ListLeftPush(key, Program.myObjectForCaching);

                Program.db.ListLeftPop(key);
                var result = Program.db.ListLeftPopAsync(key);

                result.Wait();

                if (!result.Result.IsNull)
                {
                    Logger.PrintFailureOutcome("Successfully left popped non-existing item from existing list");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to left pop non-existing item from existing list");
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

        internal static void LeftPopItemFromNonExistingListAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously left popping item from non-existing list in cache");

                var key = Guid.NewGuid().ToString();

                var result = Program.db.ListLeftPopAsync(key);

                result.Wait();

                if (!result.Result.IsNull)
                {
                    Logger.PrintFailureOutcome("Successfully left popped item from non-existing list");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to left pop item from non-existing list");
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
