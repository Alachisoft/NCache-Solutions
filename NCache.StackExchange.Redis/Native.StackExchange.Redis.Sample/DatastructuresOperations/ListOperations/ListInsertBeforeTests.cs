
using System;
using System.Collections.Generic;
using System.Text;

namespace Native.StackExchange.Redis.Sample.DatastructuresOperations.ListOperations
{
    class ListInsertBeforeTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        internal static void InsertBeforeExistingPivotAndList()
        {
            try
            {
                Logger.PrintTestStartInformation("Inserting item before existing pivot in existing list in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.ListLeftPush(key, Program.myObjectForCaching);

                var result = Program.db.ListInsertBefore(key, Program.myObjectForCaching, Program.myObjectForCaching);

                if (result != -1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully inserted item before existing pivot in existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to insert item before existing pivot in existing list");
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

        internal static void InsertBeforeNonExistingPivotAndExistingList()
        {
            try
            {
                Logger.PrintTestStartInformation("Inserting item before non-existing pivot in existing list in cache");

                var key = Guid.NewGuid().ToString();
                var pivot = Guid.NewGuid().ToString();

                Program.db.ListLeftPush(key, Program.myObjectForCaching);

                var result = Program.db.ListInsertBefore(key, pivot, Program.myObjectForCaching);

                if (result == -1)
                {
                    Logger.PrintSuccessfulOutcome("Unable to insert item before non-existing pivot in existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully inserted item before non-existing pivot in existing list");
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

        internal static void InsertBeforeNonExistingPivotAndList()
        {
            try
            {
                Logger.PrintTestStartInformation("Inserting item before pivot in non-existing list in cache");

                var key = Guid.NewGuid().ToString();
                var pivot = Guid.NewGuid().ToString();

                var result = Program.db.ListInsertBefore(key, pivot, Program.myObjectForCaching);

                if (result == 0)
                {
                    Logger.PrintSuccessfulOutcome("Unable to insert item before pivot in non-existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully inserted item before pivot in non-existing list");
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

        internal static void InsertBeforeExistingPivotAndListAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously inserting item before existing pivot in existing list in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.ListLeftPush(key, Program.myObjectForCaching);

                var result = Program.db.ListInsertBeforeAsync(key, Program.myObjectForCaching, Program.myObjectForCaching);

                result.Wait();

                if (result.Result != -1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully inserted item before existing pivot in existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to insert item before existing pivot in existing list");
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

        internal static void InsertBeforeNonExistingPivotAndExistingListAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously inserting item before non-existing pivot in existing list in cache");

                var key = Guid.NewGuid().ToString();
                var pivot = Guid.NewGuid().ToString();

                Program.db.ListLeftPush(key, Program.myObjectForCaching);

                var result = Program.db.ListInsertBeforeAsync(key, pivot, Program.myObjectForCaching);

                result.Wait();

                if (result.Result == -1)
                {
                    Logger.PrintSuccessfulOutcome("Unable to insert item before non-existing pivot in existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully inserted item before non-existing pivot in existing list");
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

        internal static void InsertBeforeNonExistingPivotAndListAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously inserting item before pivot in non-existing list in cache");

                var key = Guid.NewGuid().ToString();
                var pivot = Guid.NewGuid().ToString();

                var result = Program.db.ListInsertBeforeAsync(key, pivot, Program.myObjectForCaching);

                result.Wait();

                if (result.Result == 0)
                {
                    Logger.PrintSuccessfulOutcome("Unable to insert item before pivot in non-existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully inserted item before pivot in non-existing list");
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
