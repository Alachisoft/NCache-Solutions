using System;
using System.Collections.Generic;
using System.Text;

namespace Native.StackExchange.Redis.Sample.KeyOperations
{
    class KeyDumpTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        internal static void GetByteArrayForExistingObject()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting byte array for already existing object in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.StringSet(key, Program.myObjectForCaching);

                var result = Program.db.KeyDump(key);

                if (result != null)
                {
                    Logger.PrintSuccessfulOutcome("Byte array returned successfully");
                }
                else
                {
                    Logger.PrintFailureOutcome("Failed to get byte array from cache");
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

        internal static void GetByteArrayForNonExistingObject()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting byte array for non-existing object in cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.KeyDump(key);

                if (result != null)
                {
                    Logger.PrintFailureOutcome("Byte array returned successfully");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Failed to get byte array from cache");
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

        internal static void GetByteArrayForExistingObjectAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting byte array for already existing object in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.StringSet(key, Program.myObjectForCaching);

                var result = Program.db.KeyDumpAsync(key);
                result.Wait();

                if (result.Result != null)
                {
                    Logger.PrintSuccessfulOutcome("Byte array returned successfully");
                }
                else
                {
                    Logger.PrintFailureOutcome("Failed to get byte array from cache");
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

        internal static void GetByteArrayForNonExistingObjectAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting byte array for non-existing object in cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.KeyDumpAsync(key);
                
                result.Wait();

                if (result.Result != null)
                {
                    Logger.PrintFailureOutcome("Byte array returned successfully");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Failed to get byte array from cache");
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
