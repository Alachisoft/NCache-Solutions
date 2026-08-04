using System;
using System.Collections.Generic;
using System.Text;

namespace Native.StackExchange.Redis.Sample.ObjectToOrFroByteArrayOperations
{
    class KeyRestoreTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        internal static void RestoreNonExistingKey()
        {
            try
            {
                Logger.PrintTestStartInformation("Restoring a non-existing key in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.StringSet(key, Program.myObjectForCaching);
                var byteValue = Program.db.KeyDump(key);

                Program.db.KeyDelete(key);
                Program.db.KeyRestore(key, byteValue);

                if (Program.db.KeyExists(key))
                {
                    Logger.PrintSuccessfulOutcome("Successfully restored item in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to restore item in cache");
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

        internal static void RestoreExistingKey()
        {
            try
            {
                Logger.PrintTestStartInformation("Restoring already existing key in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.StringSet(key, Program.myObjectForCaching);
                var byteValue = Program.db.KeyDump(key);

                Program.db.KeyRestore(key, byteValue);

                if (Program.db.KeyExists(key))
                {
                    Logger.PrintFailureOutcome("Successfully restored item in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to restore item in cache");
                }
            }
            catch (Exception e)
            {
                Logger.PrintSuccessfulOutcome("Exception: " + e.Message);
            }
            finally
            {
                Logger.PrintBreakLine();
            }
        }

        //------------------------------------------------Async Methods------------------------------------------------\\

        internal static void RestoreNonExistingKeyAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously restoring a non-existing key in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.StringSet(key, Program.myObjectForCaching);
                var byteValue = Program.db.KeyDump(key);

                Program.db.KeyDelete(key);
                Program.db.KeyRestoreAsync(key, byteValue).Wait();

                if (Program.db.KeyExists(key))
                {
                    Logger.PrintSuccessfulOutcome("Successfully restored item in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to restore item in cache");
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

        internal static void RestoreExistingKeyAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously restoring already existing key in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.StringSet(key, Program.myObjectForCaching);
                var byteValue = Program.db.KeyDump(key);

                Program.db.KeyRestoreAsync(key, byteValue).Wait();

                if (Program.db.KeyExists(key))
                {
                    Logger.PrintFailureOutcome("Successfully restored item in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to restore item in cache");
                }
            }
            catch (Exception e)
            {
                Logger.PrintSuccessfulOutcome("Exception: " + e.Message);
            }
            finally
            {
                Logger.PrintBreakLine();
            }
        }
    }
}
