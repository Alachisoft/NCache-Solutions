using BasicUsageStackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCache.StackExchange.Redis.Sample.DatastructuresOperations.ListOperations
{
    class ListRightPushTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        internal static void RightPushItemInExistingList()
        {
            try
            {
                Logger.PrintTestStartInformation("Right pushinging item in existing list in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.ListRightPush(key, Program.myObjectForCaching);

                var result = Program.db.ListRightPush(key, Program.myObjectForCaching);

                if (result != -1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully right pushed item in existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to right push item in existing list");
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

        internal static void RightPushItemInNonExistingList()
        {
            try
            {
                Logger.PrintTestStartInformation("Right pushing item in non-existing list in cache");

                var key = Guid.NewGuid().ToString();

                var result = Program.db.ListRightPush(key, Program.myObjectForCaching);

                if (result != -1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully right pushed item in non-existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to right push item in non-existing list");
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

        internal static void RightPushItemInExistingListAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously right pushinging item in existing list in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.ListRightPush(key, Program.myObjectForCaching);

                var result = Program.db.ListRightPushAsync(key, Program.myObjectForCaching);

                result.Wait();

                if (result.Result != -1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully right pushed item in existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to right push item in existing list");
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

        internal static void RightPushItemInNonExistingListAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously right pushing item in non-existing list in cache");

                var key = Guid.NewGuid().ToString();

                var result = Program.db.ListRightPushAsync(key, Program.myObjectForCaching);

                result.Wait();

                if (result.Result != -1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully right pushed item in non-existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to right push item in non-existing list");
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
