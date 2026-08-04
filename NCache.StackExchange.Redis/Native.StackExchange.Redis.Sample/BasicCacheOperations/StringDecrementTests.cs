
using System;
using System.Collections.Generic;
using System.Text;

namespace Native.StackExchange.Redis.Sample.BasicCacheOperations
{
    class StringDecrementTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void DecrementExistingValidString()
        {
            try
            {
                Logger.PrintTestStartInformation("Decrementing already existing valid string in cache");

                var key = Guid.NewGuid().ToString();
                var item = "5";

                Program.db.StringSet(key, item);

                var result = Program.db.StringDecrement(key);

                if (result == 4)
                {
                    Logger.PrintSuccessfulOutcome("Successfully decremented already existing valid string in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to decrement already existing valid string in cache");
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

        public static void DecrementExistingInValidString()
        {
            try
            {
                Logger.PrintTestStartInformation("Decrementing already existing invalid string in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.StringSet(key, Program.myObjectForCaching);

                var result = Program.db.StringDecrement(key);

                Logger.PrintFailureOutcome("Successfully decremented already existing invalid string in cache");
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

        public static void DecrementNonExistingString()
        {
            try
            {
                Logger.PrintTestStartInformation("Decrementing non-existing string in cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.StringDecrement(key);

                if (result == -1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully decremented non-existing string");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to decrement non-existing string");
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

        public static void DecrementExistingValidStringByLongValue()
        {
            try
            {
                Logger.PrintTestStartInformation("Decrementing already existing valid string by long value in cache");

                var key = Guid.NewGuid().ToString();
                var item = "5";
                var decrementBy = 4;

                Program.db.StringSet(key, item);

                var result = Program.db.StringDecrement(key, decrementBy);

                if (result == 1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully decremented already existing valid string by long value in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to decrement already existing valid string by long value in cache");
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

        public static void DecrementExistingValidStringByDoubleValue()
        {
            try
            {
                Logger.PrintTestStartInformation("Decrementing already existing valid string by double value in cache");

                var key = Guid.NewGuid().ToString();
                var item = "5";
                var decrementBy = 4.5;

                Program.db.StringSet(key, item);

                var result = Program.db.StringDecrement(key, decrementBy);

                if (result == 0.5)
                {
                    Logger.PrintSuccessfulOutcome("Successfully decremented already existing valid string by double value in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to decrement already existing valid string by double value in cache");
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

        public static void DecrementExistingValidStringAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously decrementing already existing valid string in cache");

                var key = Guid.NewGuid().ToString();
                var item = "5";

                Program.db.StringSet(key, item);

                var result = Program.db.StringDecrementAsync(key);

                result.Wait();

                if (result.Result == 4)
                {
                    Logger.PrintSuccessfulOutcome("Successfully decremented already existing valid string in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to decrement already existing valid string in cache");
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

        public static void DecrementExistingInValidStringAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously decrementing already existing invalid string in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.StringSet(key, Program.myObjectForCaching);

                var result = Program.db.StringDecrementAsync(key);

                result.Wait();

                Logger.PrintFailureOutcome("Successfully decremented already existing invalid string in cache");
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

        public static void DecrementNonExistingStringAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously decrementing non-existing string in cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.StringDecrementAsync(key);

                result.Wait();

                if (result.Result == -1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully decremented non-existing string");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to decrement non-existing string");
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

        public static void DecrementExistingValidStringByLongValueAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously decrementing already existing valid string by long value in cache");

                var key = Guid.NewGuid().ToString();
                var item = "5";
                var decrementBy = 4;

                Program.db.StringSet(key, item);

                var result = Program.db.StringDecrementAsync(key, decrementBy);

                result.Wait();

                if (result.Result == 1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully decremented already existing valid string by long value in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to decrement already existing valid string by long value in cache");
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

        public static void DecrementExistingValidStringByDoubleValueAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously decrementing already existing valid string by double value in cache");

                var key = Guid.NewGuid().ToString();
                var item = "5";
                var decrementBy = 4.5;

                Program.db.StringSet(key, item);

                var result = Program.db.StringDecrementAsync(key, decrementBy);

                result.Wait();

                if (result.Result == 0.5)
                {
                    Logger.PrintSuccessfulOutcome("Successfully decremented already existing valid string by double value in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to decrement already existing valid string by double value in cache");
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
