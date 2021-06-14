using BasicUsageStackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCache.StackExchange.Redis.Sample.DatastructuresOperations.ListOperations
{
    class ListRightPopTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void RightPopFromExistingList()
        {
            try
            {
                Logger.PrintTestStartInformation("Right Popping item from existing list in cache");

                var key = Guid.NewGuid().ToString();

                for (int i = 0; i < 5; i++)
                {
                    Program.db.ListLeftPush(key, i.ToString());
                }

                var result = Program.db.ListRightPop(key);

                if (!result.IsNull)
                {
                    Logger.PrintSuccessfulOutcome("Successfully right popped item from existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to right pop item from existing list");
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

        public static void RightPopFromExistingEmptyList()
        {
            try
            {
                Logger.PrintTestStartInformation("Right Popping item from existing empty list in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.ListLeftPush(key, Program.myObjectForCaching);
                Program.db.ListLeftPop(key);

                var result = Program.db.ListRightPop(key);

                if (!result.IsNull)
                {
                    Logger.PrintFailureOutcome("Successfully right popped item from existing empty list");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to right pop item from existing empty list");
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

        public static void RightPopFromNonExistingList()
        {
            try
            {
                Logger.PrintTestStartInformation("Right Popping item from non-existing list in cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.ListRightPop(key);

                if (!result.IsNull)
                {
                    Logger.PrintFailureOutcome("Successfully right popped item from non-existing list");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to right pop item from non-existing list");
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

        public static void RightPopFromExistingListAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously Right Popping item from existing list in cache");

                var key = Guid.NewGuid().ToString();

                for (int i = 0; i < 5; i++)
                {
                    Program.db.ListLeftPush(key, i.ToString());
                }

                var result = Program.db.ListRightPopAsync(key);

                result.Wait();

                if (!result.Result.IsNull)
                {
                    Logger.PrintSuccessfulOutcome("Successfully right popped item from existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to right pop item from existing list");
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

        public static void RightPopFromExistingEmptyListAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously Right Popping item from existing empty list in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.ListLeftPush(key, Program.myObjectForCaching);
                Program.db.ListLeftPop(key);

                var result = Program.db.ListRightPopAsync(key);

                result.Wait();

                if (!result.Result.IsNull)
                {
                    Logger.PrintFailureOutcome("Successfully right popped item from existing empty list");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to right pop item from existing empty list");
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

        public static void RightPopFromNonExistingListAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously Right Popping item from non-existing list in cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.ListRightPopAsync(key);

                result.Wait();

                if (!result.Result.IsNull)
                {
                    Logger.PrintFailureOutcome("Successfully right popped item from non-existing list");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to right pop item from non-existing list");
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
