using BasicUsageStackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCache.StackExchange.Redis.Sample.DatastructuresOperations.ListOperations
{
    class ListRightPopLeftPushTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void RightPopLeftPushInExistingLists()
        {
            try
            {
                Logger.PrintTestStartInformation("Right Popping and Left Pushing item in existing lists in cache");

                var sourceKey = Guid.NewGuid().ToString();
                var destinationKey = Guid.NewGuid().ToString();

                for (int i = 5; i > 0; i--)
                {
                    Program.db.ListLeftPush(sourceKey, i.ToString());
                    Program.db.ListLeftPush(destinationKey, i.ToString());
                }

                var result = Program.db.ListRightPopLeftPush(sourceKey, destinationKey);

                if (result != 0)
                {
                    Logger.PrintSuccessfulOutcome("Successfully right popped and left pushed item in existing lists");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to right pop and left push item in existing lists");
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

        public static void RightPopLeftPushInNonExistingDestinationList()
        {
            try
            {
                Logger.PrintTestStartInformation("Right Popping and Left Pushing item in non-existing destination list in cache");

                var sourceKey = Guid.NewGuid().ToString();
                var destinationKey = Guid.NewGuid().ToString();

                for (int i = 0; i < 5; i++)
                {
                    Program.db.ListLeftPush(destinationKey, i.ToString());
                }

                var result = Program.db.ListRightPopLeftPush(sourceKey, destinationKey);

                if (result != 0)
                {
                    Logger.PrintFailureOutcome("Successfully right popped and left pushed item in non-existing destination list");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to right pop and left push item in non-existing destination list");
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

        public static void RightPopLeftPushInNonExistingSourceList()
        {
            try
            {
                Logger.PrintTestStartInformation("Right Popping and Left Pushing item in non-existing source list in cache");

                var sourceKey = Guid.NewGuid().ToString();
                var destinationKey = Guid.NewGuid().ToString();

                for (int i = 0; i < 5; i++)
                {
                    Program.db.ListLeftPush(sourceKey, i.ToString());
                }

                var result = Program.db.ListRightPopLeftPush(sourceKey, destinationKey);

                if (result != 0)
                {
                    Logger.PrintFailureOutcome("Successfully right popped and left pushed item in non-existing source list");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to right pop and left push item in non-existing source list");
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

        public static void RightPopLeftPushInNonExistingLists()
        {
            try
            {
                Logger.PrintTestStartInformation("Right Popping and Left Pushing item in non-existing lists in cache");

                var sourceKey = Guid.NewGuid().ToString();
                var destinationKey = Guid.NewGuid().ToString();
                var result = Program.db.ListRightPopLeftPush(sourceKey, destinationKey);

                if (result != 0)
                {
                    Logger.PrintFailureOutcome("Successfully right popped and left pushed item in non-existing lists");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to right pop and left push item in non-existing lists");
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

        public static void RightPopLeftPushInExistingListsAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously Right Popping and Left Pushing item in existing lists in cache");

                var sourceKey = Guid.NewGuid().ToString();
                var destinationKey = Guid.NewGuid().ToString();

                for (int i = 5; i > 0; i--)
                {
                    Program.db.ListLeftPush(sourceKey, i.ToString());
                    Program.db.ListLeftPush(destinationKey, i.ToString());
                }

                var result = Program.db.ListRightPopLeftPushAsync(sourceKey, destinationKey);

                result.Wait();

                if (result.Result != 0)
                {
                    Logger.PrintSuccessfulOutcome("Successfully right popped and left pushed item in existing lists");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to right pop and left push item in existing lists");
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

        public static void RightPopLeftPushInNonExistingDestinationListAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously Right Popping and Left Pushing item in non-existing destination list in cache");

                var sourceKey = Guid.NewGuid().ToString();
                var destinationKey = Guid.NewGuid().ToString();

                for (int i = 0; i < 5; i++)
                {
                    Program.db.ListLeftPush(destinationKey, i.ToString());
                }

                var result = Program.db.ListRightPopLeftPushAsync(sourceKey, destinationKey);

                result.Wait();

                if (result.Result != 0)
                {
                    Logger.PrintFailureOutcome("Successfully right popped and left pushed item in non-existing destination list");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to right pop and left push item in non-existing destination list");
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

        public static void RightPopLeftPushInNonExistingSourceListAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously Right Popping and Left Pushing item in non-existing source list in cache");

                var sourceKey = Guid.NewGuid().ToString();
                var destinationKey = Guid.NewGuid().ToString();

                for (int i = 0; i < 5; i++)
                {
                    Program.db.ListLeftPush(sourceKey, i.ToString());
                }

                var result = Program.db.ListRightPopLeftPushAsync(sourceKey, destinationKey);

                result.Wait();

                if (result.Result != 0)
                {
                    Logger.PrintFailureOutcome("Successfully right popped and left pushed item in non-existing source list");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to right pop and left push item in non-existing source list");
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

        public static void RightPopLeftPushInNonExistingListsAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously Right Popping and Left Pushing item in non-existing lists in cache");

                var sourceKey = Guid.NewGuid().ToString();
                var destinationKey = Guid.NewGuid().ToString();
                var result = Program.db.ListRightPopLeftPushAsync(sourceKey, destinationKey);

                result.Wait();

                if (result.Result != 0)
                {
                    Logger.PrintFailureOutcome("Successfully right popped and left pushed item in non-existing lists");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to right pop and left push item in non-existing lists");
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
