using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Native.StackExchange.Redis.Sample.DatastructuresOperations.SetsOperations
{
    class BulkSetAddTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void AddBulkInAlreadyExistingSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Adding bulk of non-existing items in already existing set in cache");

                var key = Guid.NewGuid().ToString();
                var itemsArray = new RedisValue[10];

                for (int i = 0; i < 10; i++)
                {
                    itemsArray[i] = i + 1;
                }

                Program.db.SetAdd(key, "TestObject");

                var result = Program.db.SetAdd(key, itemsArray);

                if (result == 10)
                {
                    Logger.PrintSuccessfulOutcome("Successfully added bulk of non-existing items in already existing set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to add bulk of non-existing items in already existing set");
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

        public static void AddBulkInNonExistingSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Adding bulk of non-existing items in non-existing set in cache");

                var key = Guid.NewGuid().ToString();
                var itemsArray = new RedisValue[10];

                for (int i = 0; i < 10; i++)
                {
                    itemsArray[i] = i + 1;
                }

                var result = Program.db.SetAdd(key, itemsArray);

                if (result == 10)
                {
                    Logger.PrintSuccessfulOutcome("Successfully added bulk of non-existing items in non-existing set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to add bulk of non-existing items in non-existing set");
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

        public static void AddExistingBulkInSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Adding bulk of already existing items in already existing set in cache");

                var key = Guid.NewGuid().ToString();
                var itemsArray = new RedisValue[10];

                for (int i = 0; i < 10; i++)
                {
                    itemsArray[i] = i + 1;
                }

                Program.db.SetAdd(key, "TestObject");

                var result = Program.db.SetAdd(key, itemsArray);

                result = Program.db.SetAdd(key, itemsArray);

                if (result == 10)
                {
                    Logger.PrintFailureOutcome("Successfully added bulk of already existing items in already existing set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to add bulk of already existing items in already existing set");
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

        public static void AddBulkInAlreadyExistingSetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously adding bulk of non-existing items in already existing set in cache");

                var key = Guid.NewGuid().ToString();
                var itemsArray = new RedisValue[10];

                for (int i = 0; i < 10; i++)
                {
                    itemsArray[i] = i + 1;
                }

                Program.db.SetAdd(key, "TestObject");

                var result = Program.db.SetAddAsync(key, itemsArray);

                result.Wait();

                if (result.Result == 10)
                {
                    Logger.PrintSuccessfulOutcome("Successfully added bulk of non-existing items in already existing set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to add bulk of non-existing items in already existing set");
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

        public static void AddBulkInNonExistingSetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously adding bulk of non-existing items in non-existing set in cache");

                var key = Guid.NewGuid().ToString();
                var itemsArray = new RedisValue[10];

                for (int i = 0; i < 10; i++)
                {
                    itemsArray[i] = i + 1;
                }

                var result = Program.db.SetAddAsync(key, itemsArray);

                result.Wait();

                if (result.Result == 10)
                {
                    Logger.PrintSuccessfulOutcome("Successfully added bulk of non-existing items in non-existing set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to add bulk of non-existing items in non-existing set");
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

        public static void AddExistingBulkInSetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously adding bulk of already existing items in already existing set in cache");

                var key = Guid.NewGuid().ToString();
                var itemsArray = new RedisValue[10];

                for (int i = 0; i < 10; i++)
                {
                    itemsArray[i] = i + 1;
                }

                Program.db.SetAdd(key, "TestObject");

                Program.db.SetAdd(key, itemsArray);

                var result = Program.db.SetAddAsync(key, itemsArray);

                result.Wait();

                if (result.Result == 10)
                {
                    Logger.PrintFailureOutcome("Successfully added bulk of already existing items in already existing set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to add bulk of already existing items in already existing set");
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
