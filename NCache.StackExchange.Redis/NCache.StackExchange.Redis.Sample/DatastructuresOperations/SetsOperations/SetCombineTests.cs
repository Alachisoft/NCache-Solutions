using BasicUsageStackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCache.StackExchange.Redis.Sample.DatastructuresOperations.SetsOperations
{
    class SetCombineTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void CombineExistingSets()
        {
            try
            {
                Logger.PrintTestStartInformation("Combining already exisitng sets in cache");

                var firstSet = Guid.NewGuid().ToString();
                var secondtSet = Guid.NewGuid().ToString();

                for (int i = 0; i < 15; i++)
                {
                    if (i < 10)
                    {
                        Program.db.SetAdd(firstSet, i + 1);
                    }
                    if (i > 4)
                    {
                        Program.db.SetAdd(secondtSet, i + 1);
                    }
                }

                var unionResult = Program.db.SetCombine(SetOperation.Union, firstSet, secondtSet);
                var intersectionResult = Program.db.SetCombine(SetOperation.Intersect, firstSet, secondtSet);
                var differenceResult = Program.db.SetCombine(SetOperation.Difference, firstSet, secondtSet);

                if (unionResult.Length == 15)
                {
                    Logger.PrintSuccessfulOutcome("Union of already existing sets was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Union of already existing sets was failed");
                }

                if (intersectionResult.Length == 5)
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

        public static void CombineNonExistingFirstSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Combining non-exisitng first set in cache");

                var firstSet = Guid.NewGuid().ToString();
                var secondtSet = Guid.NewGuid().ToString();

                for (int i = 0; i < 15; i++)
                {
                        Program.db.SetAdd(secondtSet, i + 1);
                }

                var unionResult = Program.db.SetCombine(SetOperation.Union, firstSet, secondtSet);
                var intersectionResult = Program.db.SetCombine(SetOperation.Intersect, firstSet, secondtSet);
                var differenceResult = Program.db.SetCombine(SetOperation.Difference, firstSet, secondtSet);

                if (unionResult.Length == 15)
                {
                    Logger.PrintSuccessfulOutcome("Union of non-existing first set was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Union of non-existing first set was failed");
                }

                if (intersectionResult.Length == 0)
                {
                    Logger.PrintSuccessfulOutcome("Intersection of non-existing first set was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Intersection of non-existing first set was failed");
                }

                if (differenceResult.Length == 0)
                {
                    Logger.PrintSuccessfulOutcome("Difference of non-existing first set was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Difference of non-existing first set was failed");
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

        public static void CombineNonExistingSecondSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Combining non-exisitng second set in cache");

                var firstSet = Guid.NewGuid().ToString();
                var secondtSet = Guid.NewGuid().ToString();

                for (int i = 0; i < 15; i++)
                {
                    Program.db.SetAdd(firstSet, i + 1);
                }

                var unionResult = Program.db.SetCombine(SetOperation.Union, firstSet, secondtSet);
                var intersectionResult = Program.db.SetCombine(SetOperation.Intersect, firstSet, secondtSet);
                var differenceResult = Program.db.SetCombine(SetOperation.Difference, firstSet, secondtSet);

                if (unionResult.Length == 15)
                {
                    Logger.PrintSuccessfulOutcome("Union of non-existing second set was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Union of non-existing second set was failed");
                }

                if (intersectionResult.Length == 0)
                {
                    Logger.PrintSuccessfulOutcome("Intersection of non-existing second set was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Intersection of non-existing second set was failed");
                }

                if (differenceResult.Length == 15)
                {
                    Logger.PrintSuccessfulOutcome("Difference of non-existing second set was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Difference of non-existing second set was failed");
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

        public static void CombineNonExistingSets()
        {
            try
            {
                Logger.PrintTestStartInformation("Combining non-exisitng sets in cache");

                var firstSet = Guid.NewGuid().ToString();
                var secondtSet = Guid.NewGuid().ToString();
                var unionResult = Program.db.SetCombine(SetOperation.Union, firstSet, secondtSet);
                var intersectionResult = Program.db.SetCombine(SetOperation.Intersect, firstSet, secondtSet);
                var differenceResult = Program.db.SetCombine(SetOperation.Difference, firstSet, secondtSet);

                if (unionResult.Length == 0)
                {
                    Logger.PrintSuccessfulOutcome("Union of non-existing sets was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Union of non-existing sets was failed");
                }

                if (intersectionResult.Length == 0)
                {
                    Logger.PrintSuccessfulOutcome("Intersection of non-existing sets was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Intersection of non-existing sets was failed");
                }

                if (differenceResult.Length == 0)
                {
                    Logger.PrintSuccessfulOutcome("Difference of non-existing sets was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Difference of non-existing sets was failed");
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

        public static void CombineExistingSetsAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously combining already exisitng sets in cache");

                var firstSet = Guid.NewGuid().ToString();
                var secondtSet = Guid.NewGuid().ToString();

                for (int i = 0; i < 15; i++)
                {
                    if (i < 10)
                    {
                        Program.db.SetAdd(firstSet, i + 1);
                    }
                    if (i > 4)
                    {
                        Program.db.SetAdd(secondtSet, i + 1);
                    }
                }

                var unionResult = Program.db.SetCombineAsync(SetOperation.Union, firstSet, secondtSet);
                unionResult.Wait();

                var intersectionResult = Program.db.SetCombineAsync(SetOperation.Intersect, firstSet, secondtSet);
                intersectionResult.Wait();

                var differenceResult = Program.db.SetCombineAsync(SetOperation.Difference, firstSet, secondtSet);
                differenceResult.Wait();

                if (unionResult.Result.Length == 15)
                {
                    Logger.PrintSuccessfulOutcome("Union of already existing sets was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Union of already existing sets was failed");
                }

                if (intersectionResult.Result.Length == 5)
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

        public static void CombineNonExistingFirstSetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously combining non-exisitng first set in cache");

                var firstSet = Guid.NewGuid().ToString();
                var secondtSet = Guid.NewGuid().ToString();

                for (int i = 0; i < 15; i++)
                {
                    Program.db.SetAdd(secondtSet, i + 1);
                }

                var unionResult = Program.db.SetCombineAsync(SetOperation.Union, firstSet, secondtSet);
                unionResult.Wait();

                var intersectionResult = Program.db.SetCombineAsync(SetOperation.Intersect, firstSet, secondtSet);
                intersectionResult.Wait();

                var differenceResult = Program.db.SetCombineAsync(SetOperation.Difference, firstSet, secondtSet);
                differenceResult.Wait();

                if (unionResult.Result.Length == 15)
                {
                    Logger.PrintSuccessfulOutcome("Union of non-existing first set was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Union of non-existing first set was failed");
                }

                if (intersectionResult.Result.Length == 0)
                {
                    Logger.PrintSuccessfulOutcome("Intersection of non-existing first set was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Intersection of non-existing first set was failed");
                }

                if (differenceResult.Result.Length == 0)
                {
                    Logger.PrintSuccessfulOutcome("Difference of non-existing first set was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Difference of non-existing first set was failed");
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

        public static void CombineNonExistingSecondSetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously combining non-exisitng second set in cache");

                var firstSet = Guid.NewGuid().ToString();
                var secondtSet = Guid.NewGuid().ToString();

                for (int i = 0; i < 15; i++)
                {
                    Program.db.SetAdd(firstSet, i + 1);
                }

                var unionResult = Program.db.SetCombineAsync(SetOperation.Union, firstSet, secondtSet);
                unionResult.Wait();

                var intersectionResult = Program.db.SetCombineAsync(SetOperation.Intersect, firstSet, secondtSet);
                intersectionResult.Wait();

                var differenceResult = Program.db.SetCombineAsync(SetOperation.Difference, firstSet, secondtSet);
                differenceResult.Wait();

                if (unionResult.Result.Length == 15)
                {
                    Logger.PrintSuccessfulOutcome("Union of non-existing second set was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Union of non-existing second set was failed");
                }

                if (intersectionResult.Result.Length == 0)
                {
                    Logger.PrintSuccessfulOutcome("Intersection of non-existing second set was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Intersection of non-existing second set was failed");
                }

                if (differenceResult.Result.Length == 15)
                {
                    Logger.PrintSuccessfulOutcome("Difference of non-existing second set was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Difference of non-existing second set was failed");
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

        public static void CombineNonExistingSetsAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously combining non-exisitng sets in cache");

                var firstSet = Guid.NewGuid().ToString();
                var secondtSet = Guid.NewGuid().ToString();
                var unionResult = Program.db.SetCombineAsync(SetOperation.Union, firstSet, secondtSet);
                unionResult.Wait();

                var intersectionResult = Program.db.SetCombineAsync(SetOperation.Intersect, firstSet, secondtSet);
                intersectionResult.Wait();

                var differenceResult = Program.db.SetCombineAsync(SetOperation.Difference, firstSet, secondtSet);
                differenceResult.Wait();

                if (unionResult.Result.Length == 0)
                {
                    Logger.PrintSuccessfulOutcome("Union of non-existing sets was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Union of non-existing sets was failed");
                }

                if (intersectionResult.Result.Length == 0)
                {
                    Logger.PrintSuccessfulOutcome("Intersection of non-existing sets was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Intersection of non-existing sets was failed");
                }

                if (differenceResult.Result.Length == 0)
                {
                    Logger.PrintSuccessfulOutcome("Difference of non-existing sets was successful");
                }
                else
                {
                    Logger.PrintFailureOutcome("Difference of non-existing sets was failed");
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
