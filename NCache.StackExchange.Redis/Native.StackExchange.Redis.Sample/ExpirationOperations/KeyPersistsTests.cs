
using System;
using System.Collections.Generic;
using System.Text;

namespace Native.StackExchange.Redis.Sample.ExpirationOperations
{
    class KeyPersistsTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        internal static void PersistExistingKey()
        {
            try
            {
                Logger.PrintTestStartInformation("Persisting already existing item in cache");

                var key = Guid.NewGuid().ToString();
                Program.db.StringSet(key, Program.myObjectForCaching, TimeSpan.FromSeconds(60));

                if (Program.db.KeyPersist(key))
                {
                    Logger.PrintSuccessfulOutcome("Key persisted successfully");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to persist key");
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

        internal static void PersistNonExistingKey()
        {
            try
            {
                Logger.PrintTestStartInformation("Persisting non-existing item in cache");

                var key = Guid.NewGuid().ToString();

                if (Program.db.KeyPersist(key))
                {
                    Logger.PrintFailureOutcome("Key persisted successfully");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to persist key");
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

        internal static void PersistExistingKeyAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously persisting already existing item in cache");

                var key = Guid.NewGuid().ToString();
                Program.db.StringSet(key, Program.myObjectForCaching, TimeSpan.FromSeconds(60));

                var result = Program.db.KeyPersistAsync(key);
                result.Wait();
                
                if (result.Result)
                {
                    Logger.PrintSuccessfulOutcome("Key persisted successfully");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to persist key");
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

        internal static void PersistNonExistingKeyAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously persisting non-existing item in cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.KeyPersistAsync(key);

                result.Wait();

                if (result.Result)
                {
                    Logger.PrintFailureOutcome("Key persisted successfully");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to persist key");
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
