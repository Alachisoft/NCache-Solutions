
using System;
using System.Collections.Generic;
using System.Text;

namespace Native.StackExchange.Redis.Sample.DatastructuresOperations.SetsOperations
{
    class SetLengthTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void GetLengthOfExistingSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting length of already existing set in cache");

                var key = Guid.NewGuid().ToString();

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(key, i);
                }

                var result = Program.db.SetLength(key);

                if (result == 10)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got length of already existing set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to get length of already existing set");
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

        public static void GetLengthOfEmptySet()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting length of empty set in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.SetAdd(key, 1);
                Program.db.SetRemove(key, 1);

                var result = Program.db.SetLength(key);

                if (result == 0)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got length of empty set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to get length of empty set");
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

        public static void GetLengthOfNonExistingSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting length of non-existing set in cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.SetLength(key);

                if (result != 0)
                {
                    Logger.PrintFailureOutcome("Successfully got length of non-existing set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to get length of non-existing set");
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


        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void GetLengthOfExistingSetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting length of already existing set in cache");

                var key = Guid.NewGuid().ToString();

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(key, i);
                }

                var result = Program.db.SetLengthAsync(key);

                result.Wait();

                if (result.Result == 10)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got length of already existing set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to get length of already existing set");
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

        public static void GetLengthOfEmptySetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting length of empty set in cache");

                var key = Guid.NewGuid().ToString();

                Program.db.SetAdd(key, 1);
                Program.db.SetRemove(key, 1);

                var result = Program.db.SetLengthAsync(key);

                result.Wait();

                if (result.Result == 0)
                {
                    Logger.PrintSuccessfulOutcome("Successfully got length of empty set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to get length of empty set");
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

        public static void GetLengthOfNonExistingSetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously getting length of non-existing set in cache");

                var key = Guid.NewGuid().ToString();
                var result = Program.db.SetLengthAsync(key);

                result.Wait();

                if (result.Result != 0)
                {
                    Logger.PrintFailureOutcome("Successfully got length of non-existing set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to get length of non-existing set");
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
