using BasicUsageStackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCache.StackExchange.Redis.Sample.DatastructuresOperations.ListOperations
{
    class BulkListRightPushTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        internal static void RightPushBulkItemsInExistingList()
        {
            try
            {
                Logger.PrintTestStartInformation("Right pushinging bulk items in existing list in cache");

                var key = Guid.NewGuid().ToString();
                var values = new RedisValue[10];

                for (int i = 0; i < 10; i++)
                {
                    values[i] = Program.myObjectForCaching;
                }

                Program.db.ListRightPush(key, Program.myObjectForCaching);

                var result = Program.db.ListRightPush(key, values);

                if (result != -1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully right pushed bulk items in existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to right push bulk items in existing list");
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

        internal static void RightPushBulkItemsInNonExistingList()
        {
            try
            {
                Logger.PrintTestStartInformation("Right pushing bulk items in non-existing list in cache");

                var key = Guid.NewGuid().ToString();
                var values = new RedisValue[10];

                for (int i = 0; i < 10; i++)
                {
                    values[i] = Program.myObjectForCaching;
                }

                var result = Program.db.ListRightPush(key, values);

                if (result != -1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully right pushed bulk items in non-existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to right push bulk items in non-existing list");
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
                Logger.PrintTestStartInformation("Asynchronously right pushing bulk items in existing list in cache");

                var key = Guid.NewGuid().ToString();
                var values = new RedisValue[10];

                for (int i = 0; i < 10; i++)
                {
                    values[i] = Program.myObjectForCaching;
                }

                Program.db.ListRightPush(key, Program.myObjectForCaching);

                var result = Program.db.ListRightPushAsync(key, values);

                result.Wait();

                if (result.Result != -1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully right pushed bulk items in existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to right push bulk items in existing list");
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
                Logger.PrintTestStartInformation("Asynchronously right pushing bulk items in non-existing list in cache");

                var key = Guid.NewGuid().ToString();
                var values = new RedisValue[10];

                for (int i = 0; i < 10; i++)
                {
                    values[i] = Program.myObjectForCaching;
                }

                var result = Program.db.ListRightPushAsync(key, values);

                result.Wait();

                if (result.Result != -1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully right pushed bulk items in non-existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to right push bulk items in non-existing list");
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
