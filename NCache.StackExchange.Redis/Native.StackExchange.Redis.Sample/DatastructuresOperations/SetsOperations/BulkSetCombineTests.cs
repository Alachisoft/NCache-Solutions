using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Native.StackExchange.Redis.Sample.DatastructuresOperations.SetsOperations
{
    class BulkSetCombineTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void CombineExistingBulkSets()
        {
            try
            {
                Logger.PrintTestStartInformation("Combining bulk of already exisitng sets in cache");

                var keys = new RedisKey[3];

                for (int i = 0; i < 3; i++)
                {
                    keys[i] = Guid.NewGuid().ToString();
                }
                for (int i = 1; i <= 17; i++)
                {
                    if (i <= 10)
                    {
                        Program.db.SetAdd(keys[0], i);
                    }
                    if (i >= 6 && i <= 15)
                    {
                        Program.db.SetAdd(keys[1], i);
                    }
                    if (i >= 8)
                    {
                        Program.db.SetAdd(keys[2], i);
                    }
                }

                


                var unionResult = Program.db.SetCombine(SetOperation.Union, keys);
                var intersectionResult = Program.db.SetCombine(SetOperation.Intersect, keys);
                var differenceResult = Program.db.SetCombine(SetOperation.Difference, keys);

                if (unionResult.Length == 17)
                {
                    Logger.PrintSuccessfulOutcome("Union of bulk of already existing sets was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Union of bulk of already existing sets was failed");
                }

                if (intersectionResult.Length == 3)
                {
                    Logger.PrintSuccessfulOutcome("Intersection of already existing sets was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Intersection of already existing sets was failed");
                }

                if (differenceResult.Length == 5)
                {
                    Logger.PrintSuccessfulOutcome("Difference of already existing sets was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Difference of already existing sets was failed");
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

        public static void CombineNonExistingBulkSets()
        {
            try
            {
                Logger.PrintTestStartInformation("Combining bulk of non-exisitng sets in cache");

                var keys = new RedisKey[3];

                for (int i = 0; i < 3; i++)
                {
                    keys[i] = Guid.NewGuid().ToString();
                }

                var unionResult = Program.db.SetCombine(SetOperation.Union, keys);
                var intersectionResult = Program.db.SetCombine(SetOperation.Intersect, keys);
                var differenceResult = Program.db.SetCombine(SetOperation.Difference, keys);

                if (unionResult.Length == 0)
                {
                    Logger.PrintSuccessfulOutcome("Union of bulk of non-existing sets was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Union of bulk of non-existing sets was failed");
                }

                if (intersectionResult.Length == 0)
                {
                    Logger.PrintSuccessfulOutcome("Intersection of bulk of non-existing sets was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Intersection of bulk of non-existing sets was failed");
                }

                if (differenceResult.Length == 0)
                {
                    Logger.PrintSuccessfulOutcome("Difference of bulk of non-existing sets was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Difference of bulk of non-existing sets was failed");
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

        public static void CombineExistingBulkSetsAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously combining bulk of already exisitng sets in cache");

                var keys = new RedisKey[3];

                for (int i = 0; i < 3; i++)
                {
                    keys[i] = Guid.NewGuid().ToString();
                }
                for (int i = 1; i <= 17; i++)
                {
                    if (i <= 10)
                    {
                        Program.db.SetAdd(keys[0], i);
                    }
                    if (i >= 6 && i <= 15)
                    {
                        Program.db.SetAdd(keys[1], i);
                    }
                    if (i >= 8)
                    {
                        Program.db.SetAdd(keys[2], i);
                    }
                }

                var unionResult = Program.db.SetCombineAsync(SetOperation.Union, keys);
                unionResult.Wait();

                var intersectionResult = Program.db.SetCombineAsync(SetOperation.Intersect, keys);
                intersectionResult.Wait();

                var differenceResult = Program.db.SetCombineAsync(SetOperation.Difference, keys);
                differenceResult.Wait();

                if (unionResult.Result.Length == 17)
                {
                    Logger.PrintSuccessfulOutcome("Union of bulk of already existing sets was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Union of bulk of already existing sets was failed");
                }

                if (intersectionResult.Result.Length == 3)
                {
                    Logger.PrintSuccessfulOutcome("Intersection of already existing sets was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Intersection of already existing sets was failed");
                }

                if (differenceResult.Result.Length == 5)
                {
                    Logger.PrintSuccessfulOutcome("Difference of already existing sets was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Difference of already existing sets was failed");
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

        public static void CombineNonExistingBulkSetsAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously combining bulk of non-exisitng sets in cache");

                var keys = new RedisKey[3];

                for (int i = 0; i < 3; i++)
                {
                    keys[i] = Guid.NewGuid().ToString();
                }

                var unionResult = Program.db.SetCombineAsync(SetOperation.Union, keys);
                unionResult.Wait();

                var intersectionResult = Program.db.SetCombineAsync(SetOperation.Intersect, keys);
                intersectionResult.Wait();

                var differenceResult = Program.db.SetCombineAsync(SetOperation.Difference, keys);
                differenceResult.Wait();

                if (unionResult.Result.Length == 0)
                {
                    Logger.PrintSuccessfulOutcome("Union of bulk of non-existing sets was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Union of bulk of non-existing sets was failed");
                }

                if (intersectionResult.Result.Length == 0)
                {
                    Logger.PrintSuccessfulOutcome("Intersection of bulk of non-existing sets was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Intersection of bulk of non-existing sets was failed");
                }

                if (differenceResult.Result.Length == 0)
                {
                    Logger.PrintSuccessfulOutcome("Difference of bulk of non-existing sets was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Difference of bulk of non-existing sets was failed");
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
