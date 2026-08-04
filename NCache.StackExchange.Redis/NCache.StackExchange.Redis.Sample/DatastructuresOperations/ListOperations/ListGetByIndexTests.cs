using BasicUsageStackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCache.StackExchange.Redis.Sample.DatastructuresOperations.ListOperations
{
    class ListGetByIndexTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        internal static void GetExistingItemFromExistingList()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting existing item by index from an existing list in cache");

                var key = Guid.NewGuid().ToString();
                long index = 0;

                Program.db.ListLeftPush(key, Program.myObjectForCaching);

                var result = Program.db.ListGetByIndex(key, index);

                if (!result.IsNull)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got existing item from an existing list in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get existing item from an existing list in cache");
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

        internal static void GetNonExistingItemFromExistingList()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting non-existing item by index from an existing list in cache");

                var key = Guid.NewGuid().ToString();
                long index = 0;

                Program.db.ListLeftPush(key, Program.myObjectForCaching);
                Program.db.ListRemove(key, Program.myObjectForCaching);

                var result = Program.db.ListGetByIndex(key, index);

                if (!result.IsNull)
                {
                    Logger.PrintFailureOutcome("Successfully got non-existing item from an existing list in cache");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to get non-existing item from an existing list in cache");
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

        internal static void GetItemFromNonExistingList()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting item by index from non-existing list in cache");

                var key = Guid.NewGuid().ToString();
                var index = 0;
                var result = Program.db.ListGetByIndex(key, index);

                if (!result.IsNull)
                {
                    Logger.PrintFailureOutcome("Successfully got item from non-existing list in cache");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to get item from non-existing list in cache");
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

        internal static void GetExistingItemFromExistingListAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting existing item by index from an existing list in cache");

                var key = Guid.NewGuid().ToString();
                long index = 0;

                Program.db.ListLeftPush(key, Program.myObjectForCaching);

                var result = Program.db.ListGetByIndexAsync(key, index);

                result.Wait();

                if (!result.Result.IsNull)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got existing item from an existing list in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get existing item from an existing list in cache");
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

        internal static void GetNonExistingItemFromExistingListAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting non-existing item by index from an existing list in cache");

                var key = Guid.NewGuid().ToString();
                long index = 0;

                Program.db.ListLeftPush(key, Program.myObjectForCaching);
                Program.db.ListRemove(key, Program.myObjectForCaching);

                var result = Program.db.ListGetByIndexAsync(key, index);

                result.Wait();

                if (!result.Result.IsNull)
                {
                    Logger.PrintFailureOutcome("Successfully got non-existing item from an existing list in cache");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to get non-existing item from an existing list in cache");
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

        internal static void GetItemFromNonExistingListAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting item by index from non-existing list in cache");

                var key = Guid.NewGuid().ToString();
                var index = 0;
                var result = Program.db.ListGetByIndexAsync(key, index);

                if (!result.Result.IsNull)
                {
                    Logger.PrintFailureOutcome("Successfully got item from non-existing list in cache");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to get item from non-existing list in cache");
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
