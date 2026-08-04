
using System;
using System.Collections.Generic;
using System.Text;

namespace Native.StackExchange.Redis.Sample.BasicCacheOperations
{
    class StringGetRangeTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void GetExistingStringRange()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting range of already existing non-empty string in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.StringSet(key, Program.myObjectForCaching);

                var result = Program.db.StringGetRange(key, 0, 3);

                if (result.Length() == 4)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got range of already existing non-empty string");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get range of already existing non-empty string");
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

        public static void GetExistingStringRangeWithOutOfBoundEnd()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting range of already existing non-empty string by passing out of bound end value in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.StringSet(key, Program.myObjectForCaching);

                var result = Program.db.StringGetRange(key, 0, Program.myObjectForCaching.Length + 5);

                if (result.Length() == Program.myObjectForCaching.Length)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got range of already existing non-empty string by passing out of bound end value");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get range of already existing non-empty string by passing out of bound end value");
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

        public static void GetExistingStringRangeWithOutOfBoundStart()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting range of already existing non-empty string by passing out of bound start value in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.StringSet(key, Program.myObjectForCaching);

                var result = Program.db.StringGetRange(key, Program.myObjectForCaching.Length + 5, 4);

                if (result.IsNullOrEmpty)
                {
                    Logger.PrintSuccessfulOutcome("Unable to get range of already existing non-empty string by passing out of bound start value");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Successfully got range of already existing non-empty string by passing out of bound start value");
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

        public static void GetNonExistingStringRange()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting range of non-existing string in cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.StringGetRange(key, 0, 3);

                if (result.IsNullOrEmpty)
                {
                    Logger.PrintSuccessfulOutcome("Unable to get range of non-existing string");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Successfully got range of non-existing string");
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

        public static void GetExistingStringRangeWithNegativeStart()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting range of already existing non-empty string by passing negative start value in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.StringSet(key, Program.myObjectForCaching);

                var result = Program.db.StringGetRange(key, -1, 5);

                if (result.IsNullOrEmpty)
                {
                    Logger.PrintSuccessfulOutcome("Unable to get range of already existing non-empty string by passing negative start value");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully got range of already existing non-empty string by passing negative start value");
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

        public static void GetExistingStringRangeWithNegativeEnd()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting range of already existing non-empty string by passing negative end value in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.StringSet(key, Program.myObjectForCaching);

                var result = Program.db.StringGetRange(key, 0, -3);

                if (result.Length() == Program.myObjectForCaching.Length - 2)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got range of already existing non-empty string by passing negative end value");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get range of already existing non-empty string by passing negative end value");
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

        public static void GetExistingStringRangeAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting range of already existing non-empty string in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.StringSet(key, Program.myObjectForCaching);

                var result = Program.db.StringGetRangeAsync(key, 0, 3);

                result.Wait();

                if (result.Result.Length() == 4)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got range of already existing non-empty string");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to get range of already existing non-empty string");
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

        public static void GetExistingStringRangeWithOutOfBoundEndAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting range of already existing non-empty string by passing out of bound end value in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.StringSet(key, Program.myObjectForCaching);

                var result = Program.db.StringGetRangeAsync(key, 0, Program.myObjectForCaching.Length + 5);

                result.Wait();

                if (result.Result.Length() == Program.myObjectForCaching.Length)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got range of already existing non-empty string by passing out of bound end value");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get range of already existing non-empty string by passing out of bound end value");
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

        public static void GetExistingStringRangeWithOutOfBoundStartAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting range of already existing non-empty string by passing out of bound start value in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.StringSet(key, Program.myObjectForCaching);

                var result = Program.db.StringGetRangeAsync(key, Program.myObjectForCaching.Length + 5, 4);

                result.Wait();

                if (result.Result.IsNullOrEmpty)
                {
                    Logger.PrintSuccessfulOutcome("Unable to get range of already existing non-empty string by passing out of bound start value");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Successfully got range of already existing non-empty string by passing out of bound start value");
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

        public static void GetNonExistingStringRangeAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting range of non-existing string in cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.StringGetRangeAsync(key, 0, 3);

                result.Wait();

                if (result.Result.IsNullOrEmpty)
                {
                    Logger.PrintSuccessfulOutcome("Unable to get range of non-existing string");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Successfully got range of non-existing string");
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

        public static void GetExistingStringRangeWithNegativeStartAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting range of already existing non-empty string by passing negative start value in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.StringSet(key, Program.myObjectForCaching);

                var result = Program.db.StringGetRangeAsync(key, -1, 5);

                result.Wait();

                if (result.Result.IsNullOrEmpty)
                {
                    Logger.PrintSuccessfulOutcome("Unable to get range of already existing non-empty string by passing negative start value");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully got range of already existing non-empty string by passing negative start value");
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

        public static void GetExistingStringRangeWithNegativeEndAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting range of already existing non-empty string by passing negative end value in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.StringSet(key, Program.myObjectForCaching);

                var result = Program.db.StringGetRangeAsync(key, 0, -3);

                result.Wait();

                if (result.Result.Length() == Program.myObjectForCaching.Length - 2)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got range of already existing non-empty string by passing negative end value");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get range of already existing non-empty string by passing negative end value");
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
