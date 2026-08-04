using BasicUsageStackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCache.StackExchange.Redis.Sample.DatastructuresOperations.SetsOperations
{
    class SetAddTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void AddInAlreadyExistingSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Adding a non-existing item in already existing set in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.SetAdd(key, "TestObject");
                
                var result = Program.db.SetAdd(key, Program.myObjectForCaching);

                if (result)
                {
                    Logger.PrintSuccessfulOutcome("Successfully added non-existing item in already existing set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to add non-existing item in already existing set");
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

        public static void AddInNonExistingSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Adding a non-existing item in non-existing set in cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.SetAdd(key, Program.myObjectForCaching);

                if (result)
                {
                    Logger.PrintSuccessfulOutcome("Successfully added non-existing item in non-existing set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to add non-existing item in non-existing set");
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

        public static void AddExistingItemInExistingSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Adding already existing item in already existing set in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.SetAdd(key, Program.myObjectForCaching);

                var result = Program.db.SetAdd(key, Program.myObjectForCaching);

                if (result)
                {
                    Logger.PrintFailureOutcome("Successfully added already existing item in already existing set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to add already existing item in already existing set");
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

        public static void AddInAlreadyExistingSetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously adding a non-existing item in already existing set in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.SetAdd(key, "TestObject");

                var result = Program.db.SetAddAsync(key, Program.myObjectForCaching);

                result.Wait();

                if (result.Result)
                {
                    Logger.PrintSuccessfulOutcome("Successfully added non-existing item in already existing set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to add non-existing item in already existing set");
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

        public static void AddInNonExistingSetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously adding a non-existing item in non-existing set in cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.SetAddAsync(key, Program.myObjectForCaching);
                
                result.Wait();

                if (result.Result)
                {
                    Logger.PrintSuccessfulOutcome("Successfully added non-existing item in non-existing set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to add non-existing item in non-existing set");
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

        public static void AddExistingItemInExistingSetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously adding already existing item in already existing set in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.SetAdd(key, Program.myObjectForCaching);

                var result = Program.db.SetAddAsync(key, Program.myObjectForCaching);
                result.Wait();

                if (result.Result)
                {
                    Logger.PrintFailureOutcome("Successfully added already existing item in already existing set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to add already existing item in already existing set");
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
