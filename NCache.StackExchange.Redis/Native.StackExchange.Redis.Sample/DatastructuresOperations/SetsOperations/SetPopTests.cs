
using System;
using System.Collections.Generic;
using System.Text;

namespace Native.StackExchange.Redis.Sample.DatastructuresOperations.SetsOperations
{
    class SetPopTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void PopItemFromNonEmptySet()
        {
            try
            {
                Logger.PrintTestStartInformation("Popping item from already existing non-empty set in cache");

                var key = Guid.NewGuid().ToString();

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(key, i);
                }

                var result = Program.db.SetPop(key);

                if (!result.IsNullOrEmpty)
                {
                    Logger.PrintSuccessfulOutcome("Successfully popped item from already existing non-empty set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to pop item from already existing non-empty set");
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

        public static void PopItemFromNonExistingSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Popping item from non-existing set in cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.SetPop(key);

                if (result.IsNullOrEmpty)
                {
                    Logger.PrintSuccessfulOutcome("Unable to pop item from non-existing set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully popped item from non-existing set");
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

        public static void PopItemFromEmptySet()
        {
            try
            {
                Logger.PrintTestStartInformation("Popping item from already existing empty set in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.SetAdd(key, 1);
                Program.db.SetRemove(key, 1);

                var result = Program.db.SetPop(key);

                if (result.IsNullOrEmpty)
                {
                    Logger.PrintSuccessfulOutcome("Unable to pop item from already existing empty set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully popped item from already existing empty set");
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

        public static void PopItemFromNonEmptySetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously popping item from already existing non-empty set in cache");

                var key = Guid.NewGuid().ToString();

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(key, i);
                }

                var result = Program.db.SetPopAsync(key);

                result.Wait();

                if (!result.Result.IsNullOrEmpty)
                {
                    Logger.PrintSuccessfulOutcome("Successfully popped item from already existing non-empty set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to pop item from already existing non-empty set");
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

        public static void PopItemFromNonExistingSetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously popping item from non-existing set in cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.SetPopAsync(key);

                result.Wait();

                if (result.Result.IsNullOrEmpty)
                {
                    Logger.PrintSuccessfulOutcome("Unable to pop item from non-existing set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully popped item from non-existing set");
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

        public static void PopItemFromEmptySetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously popping item from already existing empty set in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.SetAdd(key, 1);
                Program.db.SetRemove(key, 1);

                var result = Program.db.SetPopAsync(key);

                result.Wait();

                if (result.Result.IsNullOrEmpty)
                {
                    Logger.PrintSuccessfulOutcome("Unable to pop item from already existing empty set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully popped item from already existing empty set");
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