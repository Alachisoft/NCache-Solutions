using BasicUsageStackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCache.StackExchange.Redis.Sample.BasicCacheOperations
{
    class StringGetSetTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void GetSetExistingKey()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting already existing key and setting a valid string against it in cache");

                var key = Guid.NewGuid().ToString();
                var newValue = "This is new value";

                Program.db.StringSet(key, Program.myObjectForCaching);

                var result = Program.db.StringGetSet(key, newValue);

                if (result == Program.myObjectForCaching && Program.db.StringGet(key) == newValue)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got already existing key and setted a valid string against it in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get already existing key and set a valid string against it in cache");
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

        public static void GetSetNonExistingKey()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting non-existing key and setting a valid string against it in cache");

                var key = Guid.NewGuid().ToString();
                var newValue = "This is new value";
                var result = Program.db.StringGetSet(key, newValue);

                if (result.IsNullOrEmpty && Program.db.StringGet(key) == newValue)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got non-existing key and setted a valid string against it in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get non-existing key and set a valid string against it in cache");
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

        public static void GetExistingStringAndSetInt()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting already existing string and setting a valid int against it in cache");

                var key = Guid.NewGuid().ToString();
                var newValue = 5;

                Program.db.StringSet(key, Program.myObjectForCaching);

                var result = Program.db.StringGetSet(key, newValue);

                if (result == Program.myObjectForCaching && Program.db.StringGet(key) == newValue)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got already existing string and setted a valid int against it in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get already existing string and set a valid int against it in cache");
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

        public static void GetSetExistingKeyAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting already existing key and setting a valid string against it in cache");

                var key = Guid.NewGuid().ToString();
                var newValue = "This is new value";

                Program.db.StringSet(key, Program.myObjectForCaching);

                var result = Program.db.StringGetSetAsync(key, newValue);

                result.Wait();

                if (result.Result == Program.myObjectForCaching && Program.db.StringGet(key) == newValue)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got already existing key and setted a valid string against it in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get already existing key and set a valid string against it in cache");
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

        public static void GetSetNonExistingKeyAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting non-existing key and setting a valid string against it in cache");

                var key = Guid.NewGuid().ToString();
                var newValue = "This is new value";
                var result = Program.db.StringGetSetAsync(key, newValue);

                result.Wait();

                if (result.Result.IsNullOrEmpty && Program.db.StringGet(key) == newValue)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got non-existing key and setted a valid string against it in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get non-existing key and set a valid string against it in cache");
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

        public static void GetExistingStringAndSetIntAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting already existing string and setting a valid int against it in cache");

                var key = Guid.NewGuid().ToString();
                var newValue = 5;

                Program.db.StringSet(key, Program.myObjectForCaching);

                var result = Program.db.StringGetSetAsync(key, newValue);

                result.Wait();

                if (result.Result == Program.myObjectForCaching && Program.db.StringGet(key) == newValue)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got already existing string and setted a valid int against it in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to get already existing string and set a valid int against it in cache");
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
