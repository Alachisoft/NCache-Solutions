using BasicUsageStackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCache.StackExchange.Redis.Sample.DatastructuresOperations.ListOperations
{
    class ListLeftPushTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        internal static void LeftPushItemInExistingList()
        {
            try
            {
                Logger.PrintTestStartInformation("Left pushinging item in existing list in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.ListLeftPush(key, Program.myObjectForCaching);

                var result = Program.db.ListLeftPush(key, Program.myObjectForCaching);

                if (result != -1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully left pushed item in existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to left push item in existing list");
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

        internal static void LeftPushItemInNonExistingList()
        {
            try
            {
                Logger.PrintTestStartInformation("Left pushing item in non-existing list in cache");

                var key = Guid.NewGuid().ToString();

                var result = Program.db.ListLeftPush(key, Program.myObjectForCaching);

                if (result != -1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully left pushed item in non-existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to left push item in non-existing list");
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

        internal static void LeftPushItemInExistingListAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously left pushinging item in existing list in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.ListLeftPush(key, Program.myObjectForCaching);

                var result = Program.db.ListLeftPushAsync(key, Program.myObjectForCaching);

                result.Wait();

                if (result.Result != -1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully left pushed item in existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to left push item in existing list");
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

        internal static void LeftPushItemInNonExistingListAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously left pushing item in non-existing list in cache");

                var key = Guid.NewGuid().ToString();

                var result = Program.db.ListLeftPushAsync(key, Program.myObjectForCaching);

                result.Wait();

                if (result.Result != -1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully left pushed item in non-existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to left push item in non-existing list");
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
