
using System;
using System.Collections.Generic;
using System.Text;

namespace Native.StackExchange.Redis.Sample.DatastructuresOperations.ListOperations
{
    class ListRemoveTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        internal static void RemoveExistingItemFromExistingList()
        {
            try
            {
                Logger.PrintTestStartInformation("Removing existing items from existing list by passing no count value");

                var key = Guid.NewGuid().ToString();

                for (int i = 0; i < 5; i++)
                {
                    Program.db.ListLeftPush(key, i.ToString());
                    Program.db.ListLeftPush(key, i.ToString());
                }

                var result = Program.db.ListRemove(key, "3");

                if (result == 2)
                {
                    Logger.PrintSuccessfulOutcome("Successfully removed existing items from existing list by passing no count value");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to remove existing items from existing list by passing no count value");
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

        internal static void RemoveNonExistingItemFromExistingList()
        {
            try
            {
                Logger.PrintTestStartInformation("Removing non-existing items from existing list by passing no count value");

                var key = Guid.NewGuid().ToString();

                for (int i = 0; i < 5; i++)
                {
                    Program.db.ListLeftPush(key, i.ToString());
                    Program.db.ListLeftPush(key, i.ToString());
                }

                var result = Program.db.ListRemove(key, "6");

                if (result == 0)
                {
                    Logger.PrintSuccessfulOutcome("Unable to remove non-existing items from existing list by passing no count value");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully removed non-existing items from existing list by passing no count value");
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

        internal static void RemoveItemFromNonExistingList()
        {
            try
            {
                Logger.PrintTestStartInformation("Removing non-existing items from existing list by passing no count value");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.ListRemove(key, "6");

                if (result == 0)
                {
                    Logger.PrintSuccessfulOutcome("Unable to remove item from non-existing list by passing no count value");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully removed item from non-existing list by passing no count value");
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

        internal static void RemoveItemFromListPassingCount()
        {
            try
            {
                Logger.PrintTestStartInformation("Removing existing items from existing list by passing count value");

                var key = Guid.NewGuid().ToString();

                for (int i = 0; i < 5; i++)
                {
                    Program.db.ListLeftPush(key, i.ToString());
                    Program.db.ListLeftPush(key, i.ToString());
                }

                var result1 = Program.db.ListRemove(key, "3", 2);
                var result2 = Program.db.ListRemove(key, "1", -2);

                if (result1 == 2 && result2 == 2)
                {
                    Logger.PrintSuccessfulOutcome("Successfully removed items from list by passing count value");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to remove items from list by passing count value");
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

        internal static void RemoveExistingItemFromExistingListAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously removing existing items from existing list by passing no count value");

                var key = Guid.NewGuid().ToString();

                for (int i = 0; i < 5; i++)
                {
                    Program.db.ListLeftPush(key, i.ToString());
                    Program.db.ListLeftPush(key, i.ToString());
                }

                var result = Program.db.ListRemoveAsync(key, "3");

                result.Wait();

                if (result.Result == 2)
                {
                    Logger.PrintSuccessfulOutcome("Successfully removed existing items from existing list by passing no count value");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to remove existing items from existing list by passing no count value");
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

        internal static void RemoveNonExistingItemFromExistingListAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously removing non-existing items from existing list by passing no count value");

                var key = Guid.NewGuid().ToString();

                for (int i = 0; i < 5; i++)
                {
                    Program.db.ListLeftPush(key, i.ToString());
                    Program.db.ListLeftPush(key, i.ToString());
                }

                var result = Program.db.ListRemoveAsync(key, "6");

                result.Wait();

                if (result.Result == 0)
                {
                    Logger.PrintSuccessfulOutcome("Unable to remove non-existing items from existing list by passing no count value");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully removed non-existing items from existing list by passing no count value");
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

        internal static void RemoveItemFromNonExistingListAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously removing items from non-existing list by passing no count value");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.ListRemoveAsync(key, "6");

                result.Wait();

                if (result.Result == 0)
                {
                    Logger.PrintSuccessfulOutcome("Unable to remove item from non-existing list by passing no count value");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully removed item from non-existing list by passing no count value");
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

        internal static void RemoveItemFromListPassingCountAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously removing existing items from existing list by passing count value");

                var key = Guid.NewGuid().ToString();

                for (int i = 0; i < 5; i++)
                {
                    Program.db.ListLeftPush(key, i.ToString());
                    Program.db.ListLeftPush(key, i.ToString());
                }

                var result1 = Program.db.ListRemoveAsync(key, "3", 2);

                result1.Wait();

                var result2 = Program.db.ListRemoveAsync(key, "1", -2);

                result2.Wait();

                if (result1.Result == 2 && result2.Result == 2)
                {
                    Logger.PrintSuccessfulOutcome("Successfully removed items from list by passing count value");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to remove items from list by passing count value");
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
