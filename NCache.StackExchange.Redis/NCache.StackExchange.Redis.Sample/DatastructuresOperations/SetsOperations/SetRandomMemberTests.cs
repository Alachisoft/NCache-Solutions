using BasicUsageStackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCache.StackExchange.Redis.Sample.DatastructuresOperations.SetsOperations
{
    class SetRandomMemberTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void GetRandomItemFromNonEmptySet()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting random item from already existing non-empty set in cache");

                var key = Guid.NewGuid().ToString();

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(key, i);
                }

                var result = Program.db.SetRandomMember(key);

                if (!result.IsNullOrEmpty)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got random item from already existing non-empty set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get random item from already existing non-empty set");
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

        public static void GetRandomItemFromNonExistingSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting random item from non-existing set in cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.SetRandomMember(key);

                if (result.IsNullOrEmpty)
                {
                    Logger.PrintSuccessfulOutcome("Unable to get random item from non-existing set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully got random item from non-existing set");
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

        public static void GetRandomItemFromEmptySet()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting random item from already existing empty set in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.SetAdd(key, 1);
                Program.db.SetRemove(key, 1);

                var result = Program.db.SetRandomMember(key);

                if (result.IsNullOrEmpty)
                {
                    Logger.PrintSuccessfulOutcome("Unable to get random item from already existing empty set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully got random item from already existing empty set");
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

        public static void GetRandomItemFromNonEmptySetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting random item from already existing non-empty set in cache");

                var key = Guid.NewGuid().ToString();

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(key, i);
                }

                var result = Program.db.SetRandomMemberAsync(key);

                result.Wait();

                if (!result.Result.IsNullOrEmpty)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got random item from already existing non-empty set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get random item from already existing non-empty set");
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
                Logger.PrintTestStartInformation("Asynchronously getting random item from non-existing set in cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.SetRandomMemberAsync(key);

                result.Wait();

                if (result.Result.IsNullOrEmpty)
                {
                    Logger.PrintSuccessfulOutcome("Unable to get random item from non-existing set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully got random item from non-existing set");
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
                Logger.PrintTestStartInformation("Asynchronously getting random item from already existing empty set in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.SetAdd(key, 1);
                Program.db.SetRemove(key, 1);

                var result = Program.db.SetRandomMemberAsync(key);

                result.Wait();

                if (result.Result.IsNullOrEmpty)
                {
                    Logger.PrintSuccessfulOutcome("Unable to get random item from already existing empty set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully got random item from already existing empty set");
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
