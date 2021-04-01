using BasicUsageStackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCache.StackExchange.Redis.Sample.BasicCacheOperations
{
    class StringAppendTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void AppendToExistingString()
        {
            try
            {
                Logger.PrintTestStartInformation("Appending non-empty string to already existing non-empty string in cache");

                var stringInCache = Guid.NewGuid().ToString();

                Program.db.StringSet(stringInCache, Program.myObjectForCaching);

                var result = Program.db.StringAppend(stringInCache, Program.myObjectForCaching);

                if (result == 2 * Program.myObjectForCaching.Length)
                {
                    Logger.PrintSuccessfulOutcome("Successfully appended non-empty string to already existing non-empty string");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to append non-empty string to already existing non-empty string");
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

        public static void AppendEmptyStringToExistingString()
        {
            try
            {
                Logger.PrintTestStartInformation("Appending empty string to already existing non-empty string in cache");

                var stringInCache = Guid.NewGuid().ToString();

                Program.db.StringSet(stringInCache, Program.myObjectForCaching);

                var result = Program.db.StringAppend(stringInCache, "");

                if (result == Program.myObjectForCaching.Length)
                {
                    Logger.PrintSuccessfulOutcome("Successfully appended empty string to already existing non-empty string");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to append empty string to already existing non-empty string");
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

        public static void AppendToNonExistingString()
        {
            try
            {
                Logger.PrintTestStartInformation("Appending non-empty string to non-existing string in cache");

                var stringInCache = Guid.NewGuid().ToString();
                var result = Program.db.StringAppend(stringInCache, Program.myObjectForCaching);

                if (result == Program.myObjectForCaching.Length)
                {
                    Logger.PrintSuccessfulOutcome("Successfully appended non-empty string to non-existing string");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to append non-empty string to non-existing string");
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

        public static void AppendEmptyStringToNonExistingString()
        {
            try
            {
                Logger.PrintTestStartInformation("Appending empty string to non-existing string in cache");

                var stringInCache = Guid.NewGuid().ToString();
                var result = Program.db.StringAppend(stringInCache, "");

                if (result == 0)
                {
                    Logger.PrintSuccessfulOutcome("Successfully appended empty string to non-existing string");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to append empty string to non-existing string");
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

        public static void AppendToExistingInt()
        {
            try
            {
                Logger.PrintTestStartInformation("Appending non-empty string to already existing non-empty int in cache");

                var stringInCache = Guid.NewGuid().ToString();
                int item = 1;

                Program.db.StringSet(stringInCache, item);

                var result = Program.db.StringAppend(stringInCache, Program.myObjectForCaching);

                if (result == Program.myObjectForCaching.Length + 1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully appended non-empty string to already existing non-empty int");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to append non-empty string to already existing non-empty string");
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

        public static void AppendToExistingStringAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously appending non-empty string to already existing non-empty string in cache");

                var stringInCache = Guid.NewGuid().ToString();

                Program.db.StringSet(stringInCache, Program.myObjectForCaching);

                var result = Program.db.StringAppendAsync(stringInCache, Program.myObjectForCaching);

                result.Wait();

                if (result.Result == 2 * Program.myObjectForCaching.Length)
                {
                    Logger.PrintSuccessfulOutcome("Successfully appended non-empty string to already existing non-empty string");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to append non-empty string to already existing non-empty string");
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

        public static void AppendEmptyStringToExistingStringAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously appending empty string to already existing non-empty string in cache");

                var stringInCache = Guid.NewGuid().ToString();

                Program.db.StringSet(stringInCache, Program.myObjectForCaching);

                var result = Program.db.StringAppendAsync(stringInCache, "");

                result.Wait();

                if (result.Result == Program.myObjectForCaching.Length)
                {
                    Logger.PrintSuccessfulOutcome("Successfully appended empty string to already existing non-empty string");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to append empty string to already existing non-empty string");
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

        public static void AppendToNonExistingStringAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously appending non-empty string to non-existing string in cache");

                var stringInCache = Guid.NewGuid().ToString();
                var result = Program.db.StringAppendAsync(stringInCache, Program.myObjectForCaching);

                result.Wait();

                if (result.Result == Program.myObjectForCaching.Length)
                {
                    Logger.PrintSuccessfulOutcome("Successfully appended non-empty string to non-existing string");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to append non-empty string to non-existing string");
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

        public static void AppendEmptyStringToNonExistingStringAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously appending empty string to non-existing string in cache");

                var stringInCache = Guid.NewGuid().ToString();
                var result = Program.db.StringAppendAsync(stringInCache, "");

                result.Wait();

                if (result.Result == 0)
                {
                    Logger.PrintSuccessfulOutcome("Successfully appended empty string to non-existing string");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to append empty string to non-existing string");
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

        public static void AppendToExistingIntAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously appending non-empty string to already existing non-empty string in cache");

                var stringInCache = Guid.NewGuid().ToString();
                int item = 1;

                Program.db.StringSet(stringInCache, item);

                var result = Program.db.StringAppendAsync(stringInCache, Program.myObjectForCaching);

                result.Wait();

                if (result.Result == Program.myObjectForCaching.Length + 1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully appended non-empty string to already existing non-empty int");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to append non-empty string to already existing non-empty string");
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
