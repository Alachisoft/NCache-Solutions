using BasicUsageStackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCache.StackExchange.Redis.Sample.DatastructuresOperations.ListOperations
{
    class ListRangeTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        internal static void GetExistingListRangeByPassingJustListKey()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting range of values from existing list by passing just list key");

                var key = Guid.NewGuid().ToString();
                var lengthOfList = 10;

                for (int i = 0; i < lengthOfList; i++)
                {
                    Program.db.ListLeftPush(key, Program.myObjectForCaching);
                }

                var result = Program.db.ListRange(key);

                if (result == null)
                {
                    Logger.PrintDataCacheException(new Exception("An exception occured while getting range of values from list by passing just list key"));
                }
                else if (result.Length == lengthOfList)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got range of values from list by passing just list key");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get range of values from existing list by passing just list key");
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

        internal static void GetNonExistingListRangeByPassingJustListKey()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting range of values from non-existing list by passing just list key");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.ListRange(key);

                if (result == null)
                {
                    Logger.PrintDataCacheException(new Exception("An exception occured while getting range of values from non-existing list by passing just list key"));
                }
                else if (result.Length != 0)
                {
                    Logger.PrintFailureOutcome("Successfully got range of values from non-existing list by passing just list key");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to get range of values from non-existing list by passing just list key");
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

        internal static void GetExistingListRangeByPassingValidArguments()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting range of values from existing list by passing valid arguments");

                var key = Guid.NewGuid().ToString();
                var lengthOfList = 10;

                for (int i = 0; i < lengthOfList; i++)
                {
                    Program.db.ListLeftPush(key, Program.myObjectForCaching);
                }

                var result1 = Program.db.ListRange(key, 0, 9);
                var result2 = Program.db.ListRange(key, 0, -1);

                if (result1 == null || result2 == null)
                {
                    Logger.PrintDataCacheException(new Exception("An exception occured while getting range of values from list by passing valid arguments"));
                }
                else if (result1.Length == lengthOfList && result2.Length == lengthOfList)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got range of values from list by by passing valid arguments");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get range of values from non-existing list by passing valid arguments");
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

        internal static void GetExistingListRangeByPassingInValidArguments()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting range of values from existing list by passing invalid arguments");

                var key = Guid.NewGuid().ToString();
                var lengthOfList = 10;

                for (int i = 0; i < lengthOfList; i++)
                {
                    Program.db.ListLeftPush(key, Program.myObjectForCaching);
                }

                var result1 = Program.db.ListRange(key, 0, 15);
                var result2 = Program.db.ListRange(key, 0, -15);

                if (result1.Length == 0 || result2.Length == 0)
                {
                    Logger.PrintSuccessfulOutcome("Unable to get range of values from list by passing invalid arguments");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully got range of values from list by passing invalid arguments");

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

        internal static void GetExistingListRangeByPassingJustListKeyAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting range of values from existing list by passing just list key");

                var key = Guid.NewGuid().ToString();
                var lengthOfList = 10;

                for (int i = 0; i < lengthOfList; i++)
                {
                    Program.db.ListLeftPush(key, Program.myObjectForCaching);
                }

                var result = Program.db.ListRangeAsync(key);

                result.Wait();

                if (result.Result == null)
                {
                    Logger.PrintDataCacheException(new Exception("An exception occured while asynchronously getting range of values from list by passing just list key"));
                }
                else if (result.Result.Length == lengthOfList)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got range of values from list by passing just list key");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get range of values from existing list by passing just list key");
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

        internal static void GetNonExistingListRangeByPassingJustListKeyAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting range of values from non-existing list by passing just list key");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.ListRangeAsync(key);

                result.Wait();

                if (result.Result == null)
                {
                    Logger.PrintDataCacheException(new Exception("An exception occured while asynchronously getting range of values from non-existing list by passing just list key"));
                }
                else if (result.Result.Length != 0)
                {
                    Logger.PrintFailureOutcome("Successfully got range of values from non-existing list by passing just list key");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to get range of values from non-existing list by passing just list key");
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

        internal static void GetExistingListRangeByPassingValidArgumentsAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting range of values from existing list by passing valid arguments");

                var key = Guid.NewGuid().ToString();
                var lengthOfList = 10;

                for (int i = 0; i < lengthOfList; i++)
                {
                    Program.db.ListLeftPush(key, Program.myObjectForCaching);
                }

                var result1 = Program.db.ListRangeAsync(key, 0, 9);
                
                result1.Wait();

                var result2 = Program.db.ListRangeAsync(key, 0, -1);

                result2.Wait();

                if (result1.Result == null || result2.Result == null)
                {
                    Logger.PrintDataCacheException(new Exception("An exception occured while asynchronously getting range of values from list by passing valid arguments"));
                }
                else if (result1.Result.Length == lengthOfList && result2.Result.Length == lengthOfList)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got range of values from list by by passing valid arguments");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get range of values from non-existing list by passing valid arguments");
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

        internal static void GetExistingListRangeByPassingInValidArgumentsAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting range of values from existing list by passing invalid arguments");

                var key = Guid.NewGuid().ToString();
                var lengthOfList = 10;

                for (int i = 0; i < lengthOfList; i++)
                {
                    Program.db.ListLeftPush(key, Program.myObjectForCaching);
                }

                var result1 = Program.db.ListRangeAsync(key, 0, 15);

                result1.Wait();

                var result2 = Program.db.ListRangeAsync(key, 0, -15);

                result2.Wait();

                if (result1.Result.Length == 0 || result2.Result.Length == 0)
                {
                    Logger.PrintSuccessfulOutcome("Unable to get range of values from list by passing invalid arguments");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully got range of values from list by passing invalid arguments");
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
