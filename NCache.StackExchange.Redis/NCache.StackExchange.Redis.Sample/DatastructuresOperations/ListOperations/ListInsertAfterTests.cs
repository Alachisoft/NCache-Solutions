using BasicUsageStackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCache.StackExchange.Redis.Sample.DatastructuresOperations.ListOperations
{
    class ListInsertAfterTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        internal static void InsertAfterExistingPivotAndList()
        {
            try
            {
                Logger.PrintTestStartInformation("Inserting item after existing pivot in existing list in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.ListLeftPush(key, Program.myObjectForCaching);

                var result = Program.db.ListInsertAfter(key, Program.myObjectForCaching, Program.myObjectForCaching);

                if (result != -1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully inserted item after existing pivot in existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to insert item after existing pivot in existing list");
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

        internal static void InsertAfterNonExistingPivotAndExistingList()
        {
            try
            {
                Logger.PrintTestStartInformation("Inserting item after non-existing pivot in existing list in cache");

                var key = Guid.NewGuid().ToString();
                var pivot = Guid.NewGuid().ToString();

                Program.db.ListLeftPush(key, Program.myObjectForCaching);

                var result = Program.db.ListInsertAfter(key, pivot, Program.myObjectForCaching);

                if (result == -1)
                {
                    Logger.PrintSuccessfulOutcome("Unable to insert item after non-existing pivot in existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully inserted item after non-existing pivot in existing list");
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

        internal static void InsertAfterNonExistingPivotAndList()
        {
            try
            {
                Logger.PrintTestStartInformation("Inserting item after pivot in non-existing list in cache");

                var key = Guid.NewGuid().ToString();
                var pivot = Guid.NewGuid().ToString();

                var result = Program.db.ListInsertAfter(key, pivot, Program.myObjectForCaching);

                if (result == 0)
                {
                    Logger.PrintSuccessfulOutcome("Unable to insert item after pivot in non-existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully inserted item after pivot in non-existing list");
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

        internal static void InsertAfterExistingPivotAndListAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously inserting item after existing pivot in existing list in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.ListLeftPush(key, Program.myObjectForCaching);

                var result = Program.db.ListInsertAfterAsync(key, Program.myObjectForCaching, Program.myObjectForCaching);

                result.Wait();

                if (result.Result != -1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully inserted item after existing pivot in existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to insert item after existing pivot in existing list");
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

        internal static void InsertAfterNonExistingPivotAndExistingListAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously inserting item after non-existing pivot in existing list in cache");

                var key = Guid.NewGuid().ToString();
                var pivot = Guid.NewGuid().ToString();

                Program.db.ListLeftPush(key, Program.myObjectForCaching);

                var result = Program.db.ListInsertAfterAsync(key, pivot, Program.myObjectForCaching);

                result.Wait();

                if (result.Result == -1)
                {
                    Logger.PrintSuccessfulOutcome("Unable to insert item after non-existing pivot in existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully inserted item after non-existing pivot in existing list");
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

        internal static void InsertAfterNonExistingPivotAndListAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously inserting item after pivot in non-existing list in cache");

                var key = Guid.NewGuid().ToString();
                var pivot = Guid.NewGuid().ToString();

                var result = Program.db.ListInsertAfterAsync(key, pivot, Program.myObjectForCaching);

                result.Wait();

                if (result.Result == 0)
                {
                    Logger.PrintSuccessfulOutcome("Unable to insert item after pivot in non-existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully inserted item after pivot in non-existing list");
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
