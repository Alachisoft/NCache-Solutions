using BasicUsageStackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCache.StackExchange.Redis.Sample.DatastructuresOperations.ListOperations
{
    class ListLengthTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        internal static void GetLentghOfExistingList()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting length of already existing list in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.ListLeftPush(key, Program.myObjectForCaching);

                var result = Program.db.ListLength(key);

                if (result != 0)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got length of already existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get the length of already existing list");
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

        internal static void GetLentghOfNonExistingList()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting length of non-existing list in cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.ListLength(key);

                if (result != 0)
                {
                    Logger.PrintFailureOutcome("Successfully got length of non-existing list");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to get the length of non-existing list");
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

        internal static void GetLentghOfExistingListAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting length of already existing list in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.ListLeftPush(key, Program.myObjectForCaching);

                var result = Program.db.ListLengthAsync(key);

                result.Wait();

                if (result.Result != 0)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got length of already existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get the length of already existing list");
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

        internal static void GetLentghOfNonExistingListAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting length of non-existing list in cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.ListLengthAsync(key);

                result.Wait();

                if (result.Result != 0)
                {
                    Logger.PrintFailureOutcome("Successfully got length of non-existing list");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to get the length of non-existing list");
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
