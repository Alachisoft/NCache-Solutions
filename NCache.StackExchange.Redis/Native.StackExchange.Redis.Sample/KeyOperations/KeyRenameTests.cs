
using System;
using System.Collections.Generic;
using System.Text;

namespace Native.StackExchange.Redis.Sample.KeyOperations
{
    class KeyRenameTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        internal static void RenameExistingKey()
        {
            try
            {
                Logger.PrintTestStartInformation("Renaming already existing key in cache");

                var key = Guid.NewGuid().ToString();
                var newKey = Guid.NewGuid().ToString();

                Program.db.StringSet(key, Program.myObjectForCaching);

                if (Program.db.KeyRename(key, newKey))
                {
                    Logger.PrintSuccessfulOutcome("Successfully renamed key in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to rename key in cache");
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

        internal static void RenameNonExistingKey()
        {
            try
            {
                Logger.PrintTestStartInformation("Renaming non-existing key in cache");

                var key = Guid.NewGuid().ToString();
                var newKey = Guid.NewGuid().ToString();

                if (Program.db.KeyRename(key, newKey))
                {
                    Logger.PrintFailureOutcome("Successfully renamed key in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to rename key in cache");
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

        internal static void RenameExistingKeyAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously renaming already existing key in cache");

                var key = Guid.NewGuid().ToString();
                var newKey = Guid.NewGuid().ToString();

                Program.db.StringSet(key, Program.myObjectForCaching);

                var result = Program.db.KeyRenameAsync(key, newKey);

                result.Wait();

                if (result.Result)
                {
                    Logger.PrintSuccessfulOutcome("Successfully renamed key in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to rename key in cache");
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

        internal static void RenameNonExistingKeyAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously renaming non-existing key in cache");

                var key = Guid.NewGuid().ToString();
                var newKey = Guid.NewGuid().ToString();

                var result = Program.db.KeyRenameAsync(key, newKey);

                result.Wait();

                if (result.Result)
                {
                    Logger.PrintFailureOutcome("Successfully renamed key in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to rename key in cache");
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
