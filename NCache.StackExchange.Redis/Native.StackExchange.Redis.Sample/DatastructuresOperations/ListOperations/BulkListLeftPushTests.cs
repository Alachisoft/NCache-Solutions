using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Native.StackExchange.Redis.Sample.DatastructuresOperations.ListOperations
{
    class BulkListLeftPushTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        internal static void LeftPushBulkItemsInExistingList()
        {
            try
            {
                Logger.PrintTestStartInformation("Left pushinging bulk items in existing list in cache");

                var key = Guid.NewGuid().ToString();
                var values = new RedisValue[10];

                for (int i = 0; i < 10; i++)
                {
                    values[i] = Program.myObjectForCaching;
                }

                Program.db.ListLeftPush(key, Program.myObjectForCaching);

                var result = Program.db.ListLeftPush(key, values);

                if (result != -1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully left pushed bulk items in existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to left push bulk items in existing list");
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

        internal static void LeftPushBulkItemsInNonExistingList()
        {
            try
            {
                Logger.PrintTestStartInformation("Left pushing bulk items in non-existing list in cache");

                var key = Guid.NewGuid().ToString();
                var values = new RedisValue[10];

                for (int i = 0; i < 10; i++)
                {
                    values[i] = Program.myObjectForCaching;
                }

                var result = Program.db.ListLeftPush(key, values);

                if (result != -1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully left pushed bulk items in non-existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to left push bulk items in non-existing list");
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
                Logger.PrintTestStartInformation("Asynchronously left pushing bulk items in existing list in cache");

                var key = Guid.NewGuid().ToString();
                var values = new RedisValue[10];

                for (int i = 0; i < 10; i++)
                {
                    values[i] = Program.myObjectForCaching;
                }

                Program.db.ListLeftPush(key, Program.myObjectForCaching);

                var result = Program.db.ListLeftPushAsync(key, values);

                result.Wait();

                if (result.Result != -1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully left pushed bulk items in existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to left push bulk items in existing list");
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
                Logger.PrintTestStartInformation("Asynchronously left pushing bulk items in non-existing list in cache");

                var key = Guid.NewGuid().ToString();
                var values = new RedisValue[10];

                for (int i = 0; i < 10; i++)
                {
                    values[i] = Program.myObjectForCaching;
                }

                var result = Program.db.ListLeftPushAsync(key, values);

                result.Wait();

                if (result.Result != -1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully left pushed bulk items in non-existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to left push bulk items in non-existing list");
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
