
using System;
using System.Collections.Generic;
using System.Text;

namespace Native.StackExchange.Redis.Sample.DatastructuresOperations.ListOperations
{
    class ListSetByIndexTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void SetByIndexInExistingList()
        {
            try
            {
                Logger.PrintTestStartInformation("Setting list value by index in existing list in cache");

                var key = Guid.NewGuid().ToString();
                var index = 0;
                var testObject = "Test Object";

                Program.db.ListLeftPush(key, Program.myObjectForCaching);
                Program.db.ListSetByIndex(key, index, testObject);

                var result = Program.db.ListLeftPop(key);

                if (result == testObject)
                {
                    Logger.PrintSuccessfulOutcome("Successfully set list value by index in existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to set list value by index in existing list");
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

        public static void SetByIndexInNonExistingList()
        {
            try
            {
                Logger.PrintTestStartInformation("Setting list value by index in non-existing list in cache");

                var key = Guid.NewGuid().ToString();
                var index = 0;
                var testObject = "Test Object";

                Program.db.ListSetByIndex(key, index, testObject);

                var result = Program.db.ListLeftPop(key);

                if (result == testObject)
                {
                    Logger.PrintFailureOutcome("Successfully set list value by index in non-existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to set list value by index in non-existing list");
                }
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

        //------------------------------------------------Async Methods------------------------------------------------\\

        public static void SetByIndexInExistingListAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously setting list value by index in existing list in cache");

                var key = Guid.NewGuid().ToString();
                var index = 0;
                var testObject = "Test Object";

                Program.db.ListLeftPush(key, Program.myObjectForCaching);
                Program.db.ListSetByIndexAsync(key, index, testObject).Wait();

                var result = Program.db.ListLeftPop(key);

                if (result == testObject)
                {
                    Logger.PrintSuccessfulOutcome("Successfully set list value by index in existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to set list value by index in existing list");
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

        public static void SetByIndexInNonExistingListAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously setting list value by index in non-existing list in cache");

                var key = Guid.NewGuid().ToString();
                var index = 0;
                var testObject = "Test Object";

                Program.db.ListSetByIndexAsync(key, index, testObject).Wait();

                var result = Program.db.ListLeftPop(key);

                if (result == testObject)
                {
                    Logger.PrintFailureOutcome("Successfully set list value by index in non-existing list");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to set list value by index in non-existing list");
                }
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
    }
}
