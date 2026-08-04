using BasicUsageStackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCache.StackExchange.Redis.Sample.DatastructuresOperations.ListOperations
{
    class ListTrimTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void TrimExistingList()
        {
            try
            {
                Logger.PrintTestStartInformation("Trimming already existing list with valid start and end points");

                var key = Guid.NewGuid().ToString();
                var start = 0;
                var end = 5;

                for (int i = 0; i < 10; i++)
                {
                    Program.db.ListLeftPush(key, i.ToString());
                }

                Program.db.ListTrim(key, start, end);

                if (Program.db.ListLeftPop(key) == "9")
                {
                    Logger.PrintSuccessfulOutcome("Successfully trimmed already existing list with valid start and end points");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to trim already existing list with valid start and end points");
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

        public static void TrimExistingListWithInvalidStart()
        {
            try
            {
                Logger.PrintTestStartInformation("Trimming already existing list with invalid start point");

                var key = Guid.NewGuid().ToString();
                var start = 7;
                var stop = 5;

                for (int i = 0; i < 10; i++)
                {
                    Program.db.ListLeftPush(key, i.ToString());
                }

                Program.db.ListTrim(key, start, stop);

                if (Program.db.ListRightPop(key) == 9)
                {
                    Logger.PrintFailureOutcome("Successfully trimmed already existing list with invalid start point");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to trim already existing list with invalid start point");
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

        public static void TrimNonExistingList()
        {
            try
            {
                Logger.PrintTestStartInformation("Trimming non-existing list in cache");

                var key = Guid.NewGuid().ToString();
                var start = 0;
                var stop = 5;

                Program.db.ListTrim(key, start, stop);

                if (!Program.db.ListRightPop(key).IsNull)
                {
                    Logger.PrintFailureOutcome("Successfully trimmed non-existing list");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to trim non-existing list");
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

        public static void TrimExistingListAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously trimming already existing list with valid start and end points");

                var key = Guid.NewGuid().ToString();
                var start = 0;
                var end = 5;

                for (int i = 0; i < 10; i++)
                {
                    Program.db.ListLeftPush(key, i.ToString());
                }

                Program.db.ListTrimAsync(key, start, end).Wait();

                if (Program.db.ListLeftPop(key) == "9")
                {
                    Logger.PrintSuccessfulOutcome("Successfully trimmed already existing list with valid start and end points");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to trim already existing list with valid start and end points");
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

        public static void TrimExistingListWithInvalidStartAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously trimming already existing list with invalid start point");

                var key = Guid.NewGuid().ToString();
                var start = 7;
                var stop = 5;

                for (int i = 0; i < 10; i++)
                {
                    Program.db.ListLeftPush(key, i.ToString());
                }

                Program.db.ListTrimAsync(key, start, stop).Wait();

                if (Program.db.ListRightPop(key) == 9)
                {
                    Logger.PrintFailureOutcome("Successfully trimmed already existing list with invalid start point");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to trim already existing list with invalid start point");
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

        public static void TrimNonExistingListAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously trimming non-existing list in cache");

                var key = Guid.NewGuid().ToString();
                var start = 0;
                var stop = 5;

                Program.db.ListTrimAsync(key, start, stop).Wait();

                if (!Program.db.ListRightPop(key).IsNull)
                {
                    Logger.PrintFailureOutcome("Successfully trimmed non-existing list");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to trim non-existing list");
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
