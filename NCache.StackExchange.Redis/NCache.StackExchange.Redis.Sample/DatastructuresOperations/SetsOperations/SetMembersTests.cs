using BasicUsageStackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCache.StackExchange.Redis.Sample.DatastructuresOperations.SetsOperations
{
    class SetMembersTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void GetMembersOfExistingSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting members of already exisitng set in cache");

                var key = Guid.NewGuid().ToString();

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(key, i);
                }

                var result = Program.db.SetMembers(key);

                if (result.Length == 10)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got members of already existing set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get members of already existing set");
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

        public static void GetMembersOfNonExistingSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting members of non-exisitng set in cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.SetMembers(key);

                if (result.Length != 0)
                {
                    Logger.PrintFailureOutcome("Successfully got members of non-existing set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to get members of non-existing set");
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

        public static void GetMembersOfEmptySet()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting members of exisitng empty set in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.SetAdd(key, 1);
                Program.db.SetRemove(key, 1);

                var result = Program.db.SetMembers(key);

                if (result.Length != 0)
                {
                    Logger.PrintFailureOutcome("Successfully got members of existing empty set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to get members of existing empty set");
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

        public static void GetMembersOfExistingSetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting members of already exisitng set in cache");

                var key = Guid.NewGuid().ToString();

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(key, i);
                }

                var result = Program.db.SetMembersAsync(key);

                result.Wait();

                if (result.Result.Length == 10)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got members of already existing set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get members of already existing set");
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

        public static void GetMembersOfNonExistingSetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting members of non-exisitng set in cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.SetMembersAsync(key);

                result.Wait();

                if (result.Result.Length != 0)
                {
                    Logger.PrintFailureOutcome("Successfully got members of non-existing set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to get members of non-existing set");
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

        public static void GetMembersOfEmptySetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting members of exisitng empty set in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.SetAdd(key, 1);
                Program.db.SetRemove(key, 1);

                var result = Program.db.SetMembersAsync(key);

                result.Wait();

                if (result.Result.Length != 0)
                {
                    Logger.PrintFailureOutcome("Successfully got members of existing empty set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to get members of existing empty set");
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
