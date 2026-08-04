
using System;
using System.Collections.Generic;
using System.Text;

namespace Native.StackExchange.Redis.Sample.BasicCacheOperations
{
    class StringIncrementTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void IncrementExistingValidString()
        {
            try
            {
                Logger.PrintTestStartInformation("Incrementing already existing valid string in cache");

                var key = Guid.NewGuid().ToString();
                var item = "5";

                Program.db.StringSet(key, item);

                var result = Program.db.StringIncrement(key);

                if (result == 6)
                {
                    Logger.PrintSuccessfulOutcome("Successfully incremented already existing valid string in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to increment already existing valid string in cache");
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

        public static void IncrementExistingInValidString()
        {
            try
            {
                Logger.PrintTestStartInformation("Incrementing already existing invalid string in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.StringSet(key, Program.myObjectForCaching);

                var result = Program.db.StringIncrement(key);

                Logger.PrintFailureOutcome("Successfully incremented already existing invalid string in cache");
            }
            catch (Exception e)
            {
                Logger.PrintSuccessfulOutcome(e.Message);
            }
            finally
            {
                Logger.PrintBreakLine();
            }
        }

        public static void IncrementNonExistingString()
        {
            try
            {
                Logger.PrintTestStartInformation("Incrementing non-existing string in cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.StringIncrement(key);

                if (result == 1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully incremented non-existing string");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to increment non-existing string");
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

        public static void IncrementExistingValidStringByLongValue()
        {
            try
            {
                Logger.PrintTestStartInformation("Incrementing already existing valid string by long value in cache");

                var key = Guid.NewGuid().ToString();
                var item = "5";
                var incrementBy = 4;

                Program.db.StringSet(key, item);

                var result = Program.db.StringIncrement(key, incrementBy);

                if (result == 9)
                {
                    Logger.PrintSuccessfulOutcome("Successfully incremented already existing valid string by long value in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to increment already existing valid string by long value in cache");
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

        public static void IncrementExistingValidStringByDoubleValue()
        {
            try
            {
                Logger.PrintTestStartInformation("Incrementing already existing valid string by double value in cache");

                var key = Guid.NewGuid().ToString();
                var item = "5";
                var incrementBy = 4.5;

                Program.db.StringSet(key, item);

                var result = Program.db.StringIncrement(key, incrementBy);

                if (result == 9.5)
                {
                    Logger.PrintSuccessfulOutcome("Successfully incremented already existing valid string by double value in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to increment already existing valid string by double value in cache");
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

        public static void IncrementExistingValidStringAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously incrementing already existing valid string in cache");

                var key = Guid.NewGuid().ToString();
                var item = "5";

                Program.db.StringSet(key, item);

                var result = Program.db.StringIncrementAsync(key);

                result.Wait();

                if (result.Result == 6)
                {
                    Logger.PrintSuccessfulOutcome("Successfully incremented already existing valid string in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to increment already existing valid string in cache");
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

        public static void IncrementExistingInValidStringAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously incrementing already existing invalid string in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.StringSet(key, Program.myObjectForCaching);

                var result = Program.db.StringIncrementAsync(key);

                result.Wait();

                Logger.PrintFailureOutcome("Successfully incremented already existing invalid string in cache");
            }
            catch (Exception e)
            {
                Logger.PrintSuccessfulOutcome(e.Message);
            }
            finally
            {
                Logger.PrintBreakLine();
            }
        }

        public static void IncrementNonExistingStringAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously incrementing non-existing string in cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.StringIncrementAsync(key);

                result.Wait();

                if (result.Result == 1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully incremented non-existing string");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to increment non-existing string");
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

        public static void IncrementExistingValidStringByLongValueAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously incrementing already existing valid string by long value in cache");

                var key = Guid.NewGuid().ToString();
                var item = "5";
                var incrementBy = 4;

                Program.db.StringSet(key, item);

                var result = Program.db.StringIncrementAsync(key, incrementBy);

                result.Wait();

                if (result.Result == 9)
                {
                    Logger.PrintSuccessfulOutcome("Successfully incremented already existing valid string by long value in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to increment already existing valid string by long value in cache");
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

        public static void IncrementExistingValidStringByDoubleValueAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously incrementing already existing valid string by double value in cache");

                var key = Guid.NewGuid().ToString();
                var item = "5";
                var incrementBy = 4.5;

                Program.db.StringSet(key, item);

                var result = Program.db.StringIncrementAsync(key, incrementBy);

                result.Wait();

                if (result.Result == 9.5)
                {
                    Logger.PrintSuccessfulOutcome("Successfully incremented already existing valid string by double value in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to increment already existing valid string by double value in cache");
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
