using BasicUsageStackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCache.StackExchange.Redis.Sample.DatastructuresOperations.SetsOperations
{
    class BulkSetPopTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void PopItemsBulkFromNonEmptySet()
        {
            try
            {
                Logger.PrintTestStartInformation("Popping items bulk from already existing non-empty set in cache");

                var key = Guid.NewGuid().ToString();

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(key, i);
                }

                var result = Program.db.SetPop(key, 5);

                if (result.Length == 5)
                {
                    Logger.PrintSuccessfulOutcome("Successfully popped items bulk from already existing non-empty set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to pop items bulk from already existing non-empty set");
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

        public static void PopItemsBulkFromNonEmptySetWithCountGreaterThanSetCount()
        {
            try
            {
                Logger.PrintTestStartInformation("Popping items bulk from already existing non-empty set with count greater than set count in cache");

                var key = Guid.NewGuid().ToString();

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(key, i);
                }

                var result = Program.db.SetPop(key, 15);

                if (result.Length == 10)
                {
                    Logger.PrintSuccessfulOutcome("Successfully popped items bulk from already existing non-empty set with count greater than set count");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to pop items bulk from already existing non-empty set with count greater than set count");
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

        public static void PopItemsBulkFromNonExistingSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Popping items bulk from non-existing set in cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.SetPop(key, 5);

                if (result.Length == 0)
                {
                    Logger.PrintSuccessfulOutcome("Unable to pop items bulk from non-existing set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully popped items bulk from non-existing set");
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

        public static void PopItemsBulkFromEmptySet()
        {
            try
            {
                Logger.PrintTestStartInformation("Popping items bulk from already existing empty set in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.SetAdd(key, 1);
                Program.db.SetRemove(key, 1);

                var result = Program.db.SetPop(key, 5);

                if (result.Length == 0)
                {
                    Logger.PrintSuccessfulOutcome("Unable to pop items bulk from already existing empty set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully popped items bulk from already existing empty set");
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

        public static void PopItemsBulkFromNonEmptySetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously popping items bulk from already existing non-empty set in cache");

                var key = Guid.NewGuid().ToString();

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(key, i);
                }

                var result = Program.db.SetPopAsync(key, 5);

                result.Wait();

                if (result.Result.Length == 5)
                {
                    Logger.PrintSuccessfulOutcome("Successfully popped items bulk from already existing non-empty set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to pop items bulk from already existing non-empty set");
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

        public static void PopItemsBulkFromNonEmptySetWithCountGreaterThanSetCountAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously popping items bulk from already existing non-empty set with count greater than set count in cache");

                var key = Guid.NewGuid().ToString();

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(key, i);
                }

                var result = Program.db.SetPopAsync(key, 15);

                result.Wait();

                if (result.Result.Length == 10)
                {
                    Logger.PrintSuccessfulOutcome("Successfully popped items bulk from already existing non-empty set with count greater than set count");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to pop items bulk from already existing non-empty set with count greater than set count");
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

        public static void PopItemsBulkFromNonExistingSetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously popping items bulk from non-existing set in cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.SetPopAsync(key, 5);

                result.Wait();

                if (result.Result.Length == 0)
                {
                    Logger.PrintSuccessfulOutcome("Unable to pop items bulk from non-existing set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully popped items bulk from non-existing set");
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

        public static void PopItemsBulkFromEmptySetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously popping items bulk from already existing empty set in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.SetAdd(key, 1);
                Program.db.SetRemove(key, 1);

                var result = Program.db.SetPopAsync(key, 5);

                result.Wait();

                if (result.Result.Length == 0)
                {
                    Logger.PrintSuccessfulOutcome("Unable to pop items bulk from already existing empty set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully popped items bulk from already existing empty set");
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
