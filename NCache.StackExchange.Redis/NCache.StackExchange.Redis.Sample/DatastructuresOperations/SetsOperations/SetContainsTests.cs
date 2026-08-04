using BasicUsageStackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCache.StackExchange.Redis.Sample.DatastructuresultOperations.SetsOperations
{
    class SetContainsTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void CheckExistingItemInExistingSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Checking presence of already existing item in already existing set in cache");

                var key = Guid.NewGuid().ToString();
                var item = 1;

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(key, i);
                }

                var result = Program.db.SetContains(key, item);

                if (result)
                {
                    Logger.PrintSuccessfulOutcome("Specified item is present in set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Specified item is not present in set");
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

        public static void CheckNonExistingItemInExistingSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Checking presence of non-existing item in already existing set in cache");

                var key = Guid.NewGuid().ToString();
                var item = 10;

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(key, i);
                }

                var result = Program.db.SetContains(key, item);

                if (result)
                {
                    Logger.PrintFailureOutcome("Specified item is present in set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Specified item is not present in set");
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

        public static void CheckItemInNonExistingSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Checking presence of item in non-existing set in cache");

                var key = Guid.NewGuid().ToString();
                var item = 10;
                var result = Program.db.SetContains(key, item);

                if (result)
                {
                    Logger.PrintFailureOutcome("Specified item is present in set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Specified item is not present in set");
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

        public static void CheckExistingItemInExistingSetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously checking presence of already existing item in already existing set in cache");

                var key = Guid.NewGuid().ToString();
                var item = 1;

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(key, i);
                }

                var result = Program.db.SetContainsAsync(key, item);

                result.Wait();

                if (result.Result)
                {
                    Logger.PrintSuccessfulOutcome("Specified item is present in set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Specified item is not present in set");
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

        public static void CheckNonExistingItemInExistingSetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously checking presence of non-existing item in already existing set in cache");

                var key = Guid.NewGuid().ToString();
                var item = 10;

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(key, i);
                }

                var result = Program.db.SetContainsAsync(key, item);

                result.Wait();

                if (result.Result)
                {
                    Logger.PrintFailureOutcome("Specified item is present in set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Specified item is not present in set");
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

        public static void CheckItemInNonExistingSetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously checking presence of item in non-existing set in cache");

                var key = Guid.NewGuid().ToString();
                var item = 10;
                var result = Program.db.SetContainsAsync(key, item);

                result.Wait();

                if (result.Result)
                {
                    Logger.PrintFailureOutcome("Specified item is present in set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Specified item is not present in set");
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
