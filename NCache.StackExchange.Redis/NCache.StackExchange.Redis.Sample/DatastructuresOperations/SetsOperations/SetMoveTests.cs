using BasicUsageStackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCache.StackExchange.Redis.Sample.DatastructuresOperations.SetsOperations
{
    class SetMoveTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void MoveItemAmongExistingSets()
        {
            try
            {
                Logger.PrintTestStartInformation("Moving existing item from existing source set to existing destination set in cache");

                var sourceSet = Guid.NewGuid().ToString();
                var destinationSet = Guid.NewGuid().ToString();
                var item = 1;

                for (int i = 0; i < 20; i++)
                {
                    if (i < 10)
                    {
                        Program.db.SetAdd(sourceSet, i);
                    }
                    else
                    {
                        Program.db.SetAdd(destinationSet, i);
                    }
                }

                var result = Program.db.SetMove(sourceSet, destinationSet, item);

                if (result && Program.db.SetContains(destinationSet, item))
                {
                    Logger.PrintSuccessfulOutcome("Successfully moved existing item from existing source set to existing destination set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to move existing item from existing source set to existing destination set");
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

        public static void MoveItemAmongNonExistingSets()
        {
            try
            {
                Logger.PrintTestStartInformation("Moving item from non-existing source set to non-existing destination set in cache");

                var sourceSet = Guid.NewGuid().ToString();
                var destinationSet = Guid.NewGuid().ToString();
                var item = 1;
                var result = Program.db.SetMove(sourceSet, destinationSet, item);

                if (result && Program.db.SetContains(destinationSet, item))
                {
                    Logger.PrintFailureOutcome("Successfully moved item from non-existing source set to non-existing destination set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to move item from non-existing source set to non-existing destination set");
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

        public static void MoveItemToNonExistingDestinationSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Moving existing item from existing source set to non-existing destination set in cache");

                var sourceSet = Guid.NewGuid().ToString();
                var destinationSet = Guid.NewGuid().ToString();
                var item = 1;

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(sourceSet, i);
                }

                var result = Program.db.SetMove(sourceSet, destinationSet, item);

                if (result && Program.db.SetContains(destinationSet, item))
                {
                    Logger.PrintSuccessfulOutcome("Successfully moved existing item from existing source set to non-existing destination set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to move existing item from existing source set to non-existing destination set");
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

        public static void MoveItemFromNonExistingSourceSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Moving item from non-existing source set to existing destination set in cache");

                var sourceSet = Guid.NewGuid().ToString();
                var destinationSet = Guid.NewGuid().ToString();
                var item = 11;

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(sourceSet, i);
                }

                var result = Program.db.SetMove(sourceSet, destinationSet, item);

                if (result && Program.db.SetContains(destinationSet, item))
                {
                    Logger.PrintFailureOutcome("Successfully moved item from non-existing source set to existing destination set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to move item from non-existing source set to existing destination set");
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

        public static void MoveNonExistingItemToNonExistingDestinationSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Moving non-existing item from existing source set to non-existing destination set in cache");

                var sourceSet = Guid.NewGuid().ToString();
                var destinationSet = Guid.NewGuid().ToString();
                var item = 11;

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(sourceSet, i);
                }

                var result = Program.db.SetMove(sourceSet, destinationSet, item);

                if (result && Program.db.SetContains(destinationSet, item))
                {
                    Logger.PrintFailureOutcome("Successfully moved non-existing item from existing source set to non-existing destination set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to move non-existing item from existing source set to non-existing destination set");
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

        public static void MoveItemAmongExistingSetsAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously moving existing item from existing source set to existing destination set in cache");

                var sourceSet = Guid.NewGuid().ToString();
                var destinationSet = Guid.NewGuid().ToString();
                var item = 1;

                for (int i = 0; i < 20; i++)
                {
                    if (i < 10)
                    {
                        Program.db.SetAdd(sourceSet, i);
                    }
                    else
                    {
                        Program.db.SetAdd(destinationSet, i);
                    }
                }

                var result = Program.db.SetMoveAsync(sourceSet, destinationSet, item);

                result.Wait();

                if (result.Result && Program.db.SetContains(destinationSet, item))
                {
                    Logger.PrintSuccessfulOutcome("Successfully moved existing item from existing source set to existing destination set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to move existing item from existing source set to existing destination set");
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

        public static void MoveItemAmongNonExistingSetsAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously moving item from non-existing source set to non-existing destination set in cache");

                var sourceSet = Guid.NewGuid().ToString();
                var destinationSet = Guid.NewGuid().ToString();
                var item = 1;
                var result = Program.db.SetMoveAsync(sourceSet, destinationSet, item);

                result.Wait();

                if (result.Result && Program.db.SetContains(destinationSet, item))
                {
                    Logger.PrintFailureOutcome("Successfully moved item from non-existing source set to non-existing destination set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to move item from non-existing source set to non-existing destination set");
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

        public static void MoveItemToNonExistingDestinationSetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously moving existing item from existing source set to non-existing destination set in cache");

                var sourceSet = Guid.NewGuid().ToString();
                var destinationSet = Guid.NewGuid().ToString();
                var item = 1;

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(sourceSet, i);
                }

                var result = Program.db.SetMoveAsync(sourceSet, destinationSet, item);

                result.Wait();

                if (result.Result && Program.db.SetContains(destinationSet, item))
                {
                    Logger.PrintSuccessfulOutcome("Successfully moved existing item from existing source set to non-existing destination set");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to move existing item from existing source set to non-existing destination set");
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

        public static void MoveItemFromNonExistingSourceSetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously moving item from non-existing source set to existing destination set in cache");

                var sourceSet = Guid.NewGuid().ToString();
                var destinationSet = Guid.NewGuid().ToString();
                var item = 11;

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(sourceSet, i);
                }

                var result = Program.db.SetMoveAsync(sourceSet, destinationSet, item);

                result.Wait();

                if (result.Result && Program.db.SetContains(destinationSet, item))
                {
                    Logger.PrintFailureOutcome("Successfully moved item from non-existing source set to existing destination set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to move item from non-existing source set to existing destination set");
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

        public static void MoveNonExistingItemToNonExistingDestinationSetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously moving non-existing item from existing source set to non-existing destination set in cache");

                var sourceSet = Guid.NewGuid().ToString();
                var destinationSet = Guid.NewGuid().ToString();
                var item = 11;

                for (int i = 0; i < 10; i++)
                {
                    Program.db.SetAdd(sourceSet, i);
                }

                var result = Program.db.SetMoveAsync(sourceSet, destinationSet, item);

                result.Wait();

                if (result.Result && Program.db.SetContains(destinationSet, item))
                {
                    Logger.PrintFailureOutcome("Successfully moved non-existing item from existing source set to non-existing destination set");
                }
                else
                {
                    Logger.PrintSuccessfulOutcome("Unable to move non-existing item from existing source set to non-existing destination set");
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
