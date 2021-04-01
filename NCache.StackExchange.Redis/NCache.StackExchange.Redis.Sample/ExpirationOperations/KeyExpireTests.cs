using BasicUsageStackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCache.StackExchange.Redis.Sample.KeyOperations
{
    class KeyExpireTests
    {

        //-------------------------------------------------Sync Methods-------------------------------------------------\\

        public static void SetTimeSpanExpirationForExistingItem()
        {
            try
            {
                Logger.PrintTestStartInformation("Setting 2 seconds expiration using TimeSpan for already existing item");

                var key = Guid.NewGuid().ToString();
                Program.db.StringSet(key, Program.myObjectForCaching);

                if (Program.db.KeyExpire(key, TimeSpan.FromSeconds(2)))
                {
                    Logger.PrintSuccessfulOutcome("Expiration set successfully");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to set expiration");
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

        public static void SetTimeSpanExpirationForNonExistingItem()
        {
            try
            {
                Logger.PrintTestStartInformation("Setting 2 seconds expiration using TimeSpan for non-existing item");

                var key = Guid.NewGuid().ToString();

                if (Program.db.KeyExpire(key, TimeSpan.FromSeconds(2)))
                {
                    Logger.PrintFailureOutcome("Expiration set successfully");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to set expiration");
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

        public static void SetDateTimeExpirationForExistingItem()
        {
            try
            {
                Logger.PrintTestStartInformation("Setting 2 seconds using DateTime expiration for already existing item");

                var key = Guid.NewGuid().ToString();
                Program.db.StringSet(key, Program.myObjectForCaching);

                if (Program.db.KeyExpire(key, DateTime.Now.AddSeconds(2)))
                {
                    Logger.PrintSuccessfulOutcome("Expiration set successfully");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to set expiration");
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

        public static void SetDateTimeExpirationForNonExistingItem()
        {
            try
            {
                Logger.PrintTestStartInformation("Setting 2 seconds using DateTime expiration for non-existing item");

                var key = Guid.NewGuid().ToString();

                if (Program.db.KeyExpire(key, DateTime.Now.AddSeconds(2)))
                {
                    Logger.PrintFailureOutcome("Expiration set successfully");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to set expiration");
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

        public static void SetTimeSpanExpirationForExistingItemAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously setting 2 seconds expiration using TimeSpan for already existing item");

                var key = Guid.NewGuid().ToString();
                Program.db.StringSet(key, Program.myObjectForCaching);

                var result = Program.db.KeyExpireAsync(key, TimeSpan.FromSeconds(2));
                
                result.Wait();

                if (result.Result)
                {
                    Logger.PrintSuccessfulOutcome("Expiration set successfully");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to set expiration");
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

        public static void SetTimeSpanExpirationForNonExistingItemAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously setting 2 seconds expiration using TimeSpan for non-existing item");

                var key = Guid.NewGuid().ToString();

                var result = Program.db.KeyExpireAsync(key, TimeSpan.FromSeconds(2));

                result.Wait();

                if (result.Result)
                {
                    Logger.PrintFailureOutcome("Expiration set successfully");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to set expiration");
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

        public static void SetDateTimeExpirationForExistingItemAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously setting 2 seconds using DateTime expiration for already existing item");

                var key = Guid.NewGuid().ToString();
                Program.db.StringSet(key, Program.myObjectForCaching);

                var result = Program.db.KeyExpireAsync(key, DateTime.Now.AddSeconds(2));

                result.Wait();

                if (result.Result)
                {
                    Logger.PrintSuccessfulOutcome("Expiration set successfully");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to set expiration");
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

        public static void SetDateTimeExpirationForNonExistingItemAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously setting 2 seconds using DateTime expiration for non-existing item");

                var key = Guid.NewGuid().ToString();

                var result = Program.db.KeyExpireAsync(key, DateTime.Now.AddSeconds(2));

                result.Wait();

                if (result.Result)
                {
                    Logger.PrintFailureOutcome("Expiration set successfully");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to set expiration");
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
