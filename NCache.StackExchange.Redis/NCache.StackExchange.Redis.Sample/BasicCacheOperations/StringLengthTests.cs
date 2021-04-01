using BasicUsageStackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCache.StackExchange.Redis.Sample.BasicCacheOperations
{
    class StringLengthTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void GetExistingStringLength()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting length of already existing non-empty string in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.StringSet(key, Program.myObjectForCaching);

                var result = Program.db.StringLength(key);

                if (result == Program.myObjectForCaching.Length)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got length of already existing non-empty string");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get length of already existing non-empty string");
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

        public static void GetNonExistingStringLength()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting length of non-existing string in cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.StringLength(key);

                if (result == 0)
                {
                    Logger.PrintSuccessfulOutcome("Unable to get length of non-existing string");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully got length of non-existing string");
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

        public static void GetNonStringLength()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting length of already existing non-string value in cache");

                var key = Guid.NewGuid().ToString();
                var nonStringItem = 123;

                Program.db.StringSet(key, nonStringItem);

                var result = Program.db.StringLength(key);

                if (result == 3)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got length of already existing non-string value");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get length of already existing non-string value");
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

        public static void GetExistingStringLengthAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting length of already existing non-empty string in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.StringSet(key, Program.myObjectForCaching);

                var result = Program.db.StringLengthAsync(key);

                result.Wait();

                if (result.Result == Program.myObjectForCaching.Length)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got length of already existing non-empty string");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get length of already existing non-empty string");
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

        public static void GetNonExistingStringLengthAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting length of non-existing string in cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.StringLengthAsync(key);

                result.Wait();

                if (result.Result == 0)
                {
                    Logger.PrintSuccessfulOutcome("Unable to get length of non-existing string");
                }
                else
                {
                    Logger.PrintFailureOutcome("Successfully got length of non-existing string");
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

        public static void GetNonStringLengthAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting length of already existing non-string value in cache");

                var key = Guid.NewGuid().ToString();
                var nonStringItem = 123;

                Program.db.StringSet(key, nonStringItem);

                var result = Program.db.StringLengthAsync(key);

                result.Wait();

                if (result.Result == 3)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got length of already existing non-string value");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get length of already existing non-string value");
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
