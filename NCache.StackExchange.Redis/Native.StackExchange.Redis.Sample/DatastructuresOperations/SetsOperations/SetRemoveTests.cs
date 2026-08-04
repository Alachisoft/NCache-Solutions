
using System;
using System.Collections.Generic;
using System.Text;

namespace Native.StackExchange.Redis.Sample.DatastructuresOperations.SetsOperations
{
    class SetRemoveTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void RemoveExistingItemFromSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Removing already existing item from already existing set in cache");

                var key = Guid.NewGuid().ToString();
                var item = 1;

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(key, i);
                }

                var result = Program.db.SetRemove(key, item);

                if (result)
                {
                    Logger.PrintSuccessfulOutcome("Successfully removed already existing item from already existing set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to remove already existing item from already existing set");
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

        public static void RemoveNonExistingItemFromSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Removing non-existing item from already existing set in cache");

                var key = Guid.NewGuid().ToString();
                var item = 10;

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(key, i);
                }

                var result = Program.db.SetRemove(key, item);

                if (result)
                {
                    Logger.PrintFailureOutcome("Successfully removed non-existing item from already existing set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to remove non-existing item from already existing set");
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

        public static void RemoveItemFromNonExistingSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Removing item from non-existing set in cache");

                var key = Guid.NewGuid().ToString();
                var item = 1;
                var result = Program.db.SetRemove(key, item);

                if (result)
                {
                    Logger.PrintFailureOutcome("Successfully removed item from non-existing set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to remove item from non-existing set");
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

        public static void RemoveItemFromExistingEmptySet()
        {
            try
            {
                Logger.PrintTestStartInformation("Removing item from already existing empty set in cache");

                var key = Guid.NewGuid().ToString();
                var item = 1;

                Program.db.SetAdd(key, 1);
                Program.db.SetRemove(key, 1);

                var result = Program.db.SetRemove(key, item);

                if (result)
                {
                    Logger.PrintFailureOutcome("Successfully removed item from already existing empty set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to remove item from already existing empty set");
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


        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void RemoveExistingItemFromSetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously removing already existing item from already existing set in cache");

                var key = Guid.NewGuid().ToString();
                var item = 1;

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(key, i);
                }

                var result = Program.db.SetRemoveAsync(key, item);

                result.Wait();

                if (result.Result)
                {
                    Logger.PrintSuccessfulOutcome("Successfully removed already existing item from already existing set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to remove already existing item from already existing set");
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

        public static void RemoveNonExistingItemFromSetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously removing non-existing item from already existing set in cache");

                var key = Guid.NewGuid().ToString();
                var item = 10;

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(key, i);
                }

                var result = Program.db.SetRemoveAsync(key, item);

                result.Wait();

                if (result.Result)
                {
                    Logger.PrintFailureOutcome("Successfully removed non-existing item from already existing set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to remove non-existing item from already existing set");
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

        public static void RemoveItemFromNonExistingSetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously removing item from non-existing set in cache");

                var key = Guid.NewGuid().ToString();
                var item = 1;
                var result = Program.db.SetRemoveAsync(key, item);

                result.Wait();

                if (result.Result)
                {
                    Logger.PrintFailureOutcome("Successfully removed item from non-existing set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to remove item from non-existing set");
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

        public static void RemoveItemFromExistingEmptySetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously removing item from already existing empty set in cache");

                var key = Guid.NewGuid().ToString();
                var item = 1;

                Program.db.SetAdd(key, 1);
                Program.db.SetRemove(key, 1);

                var result = Program.db.SetRemoveAsync(key, item);

                result.Wait();

                if (result.Result)
                {
                    Logger.PrintFailureOutcome("Successfully removed item from already existing empty set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to remove item from already existing empty set");
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
