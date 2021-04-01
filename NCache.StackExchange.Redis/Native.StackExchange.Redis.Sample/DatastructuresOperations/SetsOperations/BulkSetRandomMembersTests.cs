
using System;
using System.Collections.Generic;
using System.Text;

namespace Native.StackExchange.Redis.Sample.DatastructuresOperations.SetsOperations
{
    class BulkSetRandomMembersTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void GetRandomItemsBulkFromNonEmptySet()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting random items bulk from already existing non-empty set in cache");

                var key = Guid.NewGuid().ToString();

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(key, i);
                }

                var result = Program.db.SetRandomMembers(key, 5);

                if (result.Length == 5)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got random items bulk from already existing non-empty set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get random items bulk from already existing non-empty set");
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

        public static void GetRandomItemsBulkFromNonEmptySetWithCountGreaterThanSetCount()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting random items bulk from already existing non-empty set with count greater than set count in cache");

                var key = Guid.NewGuid().ToString();

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(key, i);
                }

                var result = Program.db.SetRandomMembers(key, 15);

                if (result.Length == 10)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got random items bulk from already existing non-empty set with count greater than set count");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get random items bulk from already existing non-empty set with count greater than set count");
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

        public static void GetRandomItemsBulkFromNonExistingSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting random items bulk from non-existing set in cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.SetRandomMembers(key, 5);

                if (result.Length == 0)
                {
                    Logger.PrintSuccessfulOutcome("Unable to get random items bulk from non-existing set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully got random items bulk from non-existing set");
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

        public static void GetRandomItemsBulkFromEmptySet()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting random items bulk from already existing empty set in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.SetAdd(key, 1);
                Program.db.SetRemove(key, 1);

                var result = Program.db.SetRandomMembers(key, 5);

                if (result.Length == 0)
                {
                    Logger.PrintSuccessfulOutcome("Unable to get random items bulk from already existing empty set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully got random items bulk from already existing empty set");
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

        public static void GetRandomItemsBulkFromNonEmptySetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting random items bulk from already existing non-empty set in cache");

                var key = Guid.NewGuid().ToString();

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(key, i);
                }

                var result = Program.db.SetRandomMembersAsync(key, 5);

                result.Wait();

                if (result.Result.Length == 5)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got random items bulk from already existing non-empty set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get random items bulk from already existing non-empty set");
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

        public static void GetRandomItemsBulkFromNonEmptySetWithCountGreaterThanSetCountAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting random items bulk from already existing non-empty set with count greater than set count in cache");

                var key = Guid.NewGuid().ToString();

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(key, i);
                }

                var result = Program.db.SetRandomMembersAsync(key, 15);

                result.Wait();

                if (result.Result.Length == 10)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got random items bulk from already existing non-empty set with count greater than set count");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get random items bulk from already existing non-empty set with count greater than set count");
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

        public static void GetRandomItemsBulkFromNonExistingSetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting random items bulk from non-existing set in cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.SetRandomMembersAsync(key, 5);

                result.Wait();

                if (result.Result.Length == 0)
                {
                    Logger.PrintSuccessfulOutcome("Unable to get random items bulk from non-existing set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully got random items bulk from non-existing set");
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

        public static void GetRandomItemsBulkFromEmptySetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting random items bulk from already existing empty set in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.SetAdd(key, 1);
                Program.db.SetRemove(key, 1);

                var result = Program.db.SetRandomMembersAsync(key, 5);

                result.Wait();

                if (result.Result.Length == 0)
                {
                    Logger.PrintSuccessfulOutcome("Unable to get random items bulk from already existing empty set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully got random items bulk from already existing empty set");
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
