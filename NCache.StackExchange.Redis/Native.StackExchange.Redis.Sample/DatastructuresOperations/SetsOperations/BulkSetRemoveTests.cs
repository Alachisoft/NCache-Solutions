using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Native.StackExchange.Redis.Sample.DatastructuresOperations.SetsOperations
{
    class BulkSetRemoveTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void RemoveExistingItemsBulkFromSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Removing already existing items bulk from already existing set in cache");

                var key = Guid.NewGuid().ToString();
                var items = new RedisValue[5];

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(key, i);
                    if (i < 5)
                    {
                        items[i] = i;
                    }
                }

                var result = Program.db.SetRemove(key, items);

                if (result == 5)
                {
                    Logger.PrintSuccessfulOutcome("Successfully removed already existing items bulk from already existing set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to remove already existing items bulk from already existing set");
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

        public static void RemoveItemsBulkGreaterThanExistingItemsCountFromSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Removing items bulk greater than existing items count from already existing set in cache");

                var key = Guid.NewGuid().ToString();
                var items = new RedisValue[15];

                for (int i = 0; i < 15; i++)
                {
                    if (i < 10)
                    {
                        Program.db.SetAdd(key, i);
                    }
                    items[i] = i;
                }

                var result = Program.db.SetRemove(key, items);

                if (result == 10)
                {
                    Logger.PrintSuccessfulOutcome("Successfully removed items bulk greater than existing items count from already existing set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to remove items bulk greater than existing items count from already existing set");
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

        public static void RemoveNonExistingItemsBulkFromSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Removing non-existing items bulk from already existing set in cache");

                var key = Guid.NewGuid().ToString();
                var items = new RedisValue[5];

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(key, i);
                    if (i < 5)
                    {
                        items[i] = i + 10;
                    }
                }

                var result = Program.db.SetRemove(key, items);

                if (result != 0)
                {
                    Logger.PrintFailureOutcome("Successfully removed non-existing items bulk from already existing set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to remove non-existing items bulk from already existing set");
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

        public static void RemoveItemsBulkFromNonExistingSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Removing items bulk from non-existing set in cache");

                var key = Guid.NewGuid().ToString();
                var items = new RedisValue[2];

                items[0] = 1;
                items[1] = 2;

                var result = Program.db.SetRemove(key, items);

                if (result != 0)
                {
                    Logger.PrintFailureOutcome("Successfully removed items bulk from non-existing set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to remove items bulk from non-existing set");
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

        public static void RemoveItemsBulkFromExistingEmptySet()
        {
            try
            {
                Logger.PrintTestStartInformation("Removing items bulk from already existing empty set in cache");

                var key = Guid.NewGuid().ToString();
                var items = new RedisValue[2];

                items[0] = 1;
                items[1] = 2;

                Program.db.SetAdd(key, 1);
                Program.db.SetRemove(key, 1);

                var result = Program.db.SetRemove(key, items);

                if (result != 0)
                {
                    Logger.PrintFailureOutcome("Successfully removed items bulk from already existing empty set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to remove items bulk from already existing empty set");
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

        public static void RemoveExistingItemsBulkFromSetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously removing already existing items bulk from already existing set in cache");

                var key = Guid.NewGuid().ToString();
                var items = new RedisValue[5];

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(key, i);
                    if (i < 5)
                    {
                        items[i] = i;
                    }
                }

                var result = Program.db.SetRemoveAsync(key, items);

                result.Wait();

                if (result.Result == 5)
                {
                    Logger.PrintSuccessfulOutcome("Successfully removed already existing items bulk from already existing set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to remove already existing items bulk from already existing set");
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

        public static void RemoveItemsBulkGreaterThanExistingItemsCountFromSetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously removing items bulk greater than existing items count from already existing set in cache");

                var key = Guid.NewGuid().ToString();
                var items = new RedisValue[15];

                for (int i = 0; i < 15; i++)
                {
                    if (i < 10)
                    {
                        Program.db.SetAdd(key, i);
                    }
                    items[i] = i;
                }

                var result = Program.db.SetRemoveAsync(key, items);

                result.Wait();

                if (result.Result == 10)
                {
                    Logger.PrintSuccessfulOutcome("Successfully removed items bulk greater than existing items count from already existing set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to remove items bulk greater than existing items count from already existing set");
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

        public static void RemoveNonExistingItemsBulkFromSetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously removing non-existing items bulk from already existing set in cache");

                var key = Guid.NewGuid().ToString();
                var items = new RedisValue[5];

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(key, i);
                    if (i < 5)
                    {
                        items[i] = i + 10;
                    }
                }

                var result = Program.db.SetRemoveAsync(key, items);

                result.Wait();

                if (result.Result != 0)
                {
                    Logger.PrintFailureOutcome("Successfully removed non-existing items bulk from already existing set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to remove non-existing items bulk from already existing set");
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

        public static void RemoveItemsBulkFromNonExistingSetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously removing items bulk from non-existing set in cache");

                var key = Guid.NewGuid().ToString();
                var items = new RedisValue[2];

                items[0] = 1;
                items[1] = 2;

                var result = Program.db.SetRemoveAsync(key, items);

                result.Wait();

                if (result.Result != 0)
                {
                    Logger.PrintFailureOutcome("Successfully removed items bulk from non-existing set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to remove items bulk from non-existing set");
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

        public static void RemoveItemsBulkFromExistingEmptySetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously removing items bulk from already existing empty set in cache");

                var key = Guid.NewGuid().ToString();
                var items = new RedisValue[2];

                items[0] = 1;
                items[1] = 2;

                Program.db.SetAdd(key, 1);
                Program.db.SetRemove(key, 1);

                var result = Program.db.SetRemoveAsync(key, items);

                result.Wait();

                if (result.Result != 0)
                {
                    Logger.PrintFailureOutcome("Successfully removed items bulk from already existing empty set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to remove items bulk from already existing empty set");
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
