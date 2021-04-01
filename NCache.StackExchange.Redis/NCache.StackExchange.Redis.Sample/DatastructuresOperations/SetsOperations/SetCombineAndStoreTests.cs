using BasicUsageStackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCache.StackExchange.Redis.Sample.DatastructuresOperations.SetsOperations
{
    class SetCombineAndStoreTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void CombineAndStoreExistingSets()
        {
            try
            {
                Logger.PrintTestStartInformation("Combining and storing already exisitng sets in cache");

                var firstSet = Guid.NewGuid().ToString();
                var secondtSet = Guid.NewGuid().ToString();
                var unionDestinationSet = Guid.NewGuid().ToString();
                var intersectionDestinationSet = Guid.NewGuid().ToString();
                var differenceDestinationSet = Guid.NewGuid().ToString();

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

                var unionResult = Program.db.SetCombineAndStore(SetOperation.Union, unionDestinationSet, firstSet, secondtSet);
                var intersectionResult = Program.db.SetCombineAndStore(SetOperation.Intersect, intersectionDestinationSet, firstSet, secondtSet);
                var differenceResult = Program.db.SetCombineAndStore(SetOperation.Difference, differenceDestinationSet, firstSet, secondtSet);

                if (unionResult == 15)
                {
                    Logger.PrintSuccessfulOutcome("Union of already existing sets was successfully evaluated and stored in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Union of already existing sets was failed and not stored");
                }

                if (intersectionResult == 5)
                {
                    Logger.PrintSuccessfulOutcome("Intersection of already existing sets was successfully evaluated and stored in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Intersection of already existing sets was failed and not stored");
                }

                if (differenceResult == 5)
                {
                    Logger.PrintSuccessfulOutcome("Difference of already existing sets was successfully evaluated and stored in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Difference of already existing sets was failed and not stored");
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

        public static void CombineAndStoreNonExistingFirstSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Combining and storing non-exisitng first set in cache");

                var firstSet = Guid.NewGuid().ToString();
                var secondtSet = Guid.NewGuid().ToString();
                var unionDestinationSet = Guid.NewGuid().ToString();
                var intersectionDestinationSet = Guid.NewGuid().ToString();
                var differenceDestinationSet = Guid.NewGuid().ToString();

                for (int i = 0; i < 15; i++)
                {
                    Program.db.SetAdd(secondtSet, i + 1);
                }

                var unionResult = Program.db.SetCombineAndStore(SetOperation.Union, unionDestinationSet, firstSet, secondtSet);
                var intersectionResult = Program.db.SetCombineAndStore(SetOperation.Intersect, intersectionDestinationSet, firstSet, secondtSet);
                var differenceResult = Program.db.SetCombineAndStore(SetOperation.Difference, differenceDestinationSet, firstSet, secondtSet);

                if (unionResult == 15)
                {
                    Logger.PrintSuccessfulOutcome("Union of non-existing first set was successfully evaluated and stored in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Union of non-existing first set was failed and not stored");
                }

                if (intersectionResult == 0)
                {
                    Logger.PrintSuccessfulOutcome("Intersection of non-existing first set was successfully evaluated and stored in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Intersection of non-existing first set was failed and not stored");
                }

                if (differenceResult == 0)
                {
                    Logger.PrintSuccessfulOutcome("Difference of non-existing first set was successfully evaluated and stored in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Difference of non-existing first set was failed and not stored");
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

        public static void CombineAndStoreNonExistingSecondSet()
        {
            try
            {
                Logger.PrintTestStartInformation("Combining and storing non-exisitng second set in cache");

                var firstSet = Guid.NewGuid().ToString();
                var secondtSet = Guid.NewGuid().ToString();
                var unionDestinationSet = Guid.NewGuid().ToString();
                var intersectionDestinationSet = Guid.NewGuid().ToString();
                var differenceDestinationSet = Guid.NewGuid().ToString();

                for (int i = 0; i < 15; i++)
                {
                    Program.db.SetAdd(firstSet, i + 1);
                }

                var unionResult = Program.db.SetCombineAndStore(SetOperation.Union, unionDestinationSet, firstSet, secondtSet);
                var intersectionResult = Program.db.SetCombineAndStore(SetOperation.Intersect, intersectionDestinationSet, firstSet, secondtSet);
                var differenceResult = Program.db.SetCombineAndStore(SetOperation.Difference, differenceDestinationSet, firstSet, secondtSet);

                if (unionResult == 15)
                {
                    Logger.PrintSuccessfulOutcome("Union of non-existing second set was successfully evaluated and stored in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Union of non-existing second set was failed and not stored");
                }

                if (intersectionResult == 0)
                {
                    Logger.PrintSuccessfulOutcome("Intersection of non-existing second set was successfully evaluated and stored in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Intersection of non-existing second set was failed and not stored");
                }

                if (differenceResult == 15)
                {
                    Logger.PrintSuccessfulOutcome("Difference of non-existing second set was successfully evaluated and stored in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Difference of non-existing second set was failed and not stored");
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

        public static void CombineAndStoreNonExistingSets()
        {
            try
            {
                Logger.PrintTestStartInformation("Combining and storing non-exisitng sets in cache");

                var firstSet = Guid.NewGuid().ToString();
                var secondtSet = Guid.NewGuid().ToString();
                var unionDestinationSet = Guid.NewGuid().ToString();
                var intersectionDestinationSet = Guid.NewGuid().ToString();
                var differenceDestinationSet = Guid.NewGuid().ToString();
                var unionResult = Program.db.SetCombineAndStore(SetOperation.Union, unionDestinationSet, firstSet, secondtSet);
                var intersectionResult = Program.db.SetCombineAndStore(SetOperation.Intersect, intersectionDestinationSet, firstSet, secondtSet);
                var differenceResult = Program.db.SetCombineAndStore(SetOperation.Difference, differenceDestinationSet, firstSet, secondtSet);

                if (unionResult == 0)
                {
                    Logger.PrintSuccessfulOutcome("Union of non-existing sets was successfully evaluated and stored in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Union of non-existing sets was failed and not stored");
                }

                if (intersectionResult == 0)
                {
                    Logger.PrintSuccessfulOutcome("Intersection of non-existing sets was successfully evaluated and stored in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Intersection of non-existing sets was failed and not stored");
                }

                if (differenceResult == 0)
                {
                    Logger.PrintSuccessfulOutcome("Difference of non-existing sets was successfully evaluated and stored in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Difference of non-existing sets was failed and not stored");
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

        public static void CombineAndStoreExistingSetsAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously combining and storing already exisitng sets in cache");

                var firstSet = Guid.NewGuid().ToString();
                var secondtSet = Guid.NewGuid().ToString();
                var unionDestinationSet = Guid.NewGuid().ToString();
                var intersectionDestinationSet = Guid.NewGuid().ToString();
                var differenceDestinationSet = Guid.NewGuid().ToString();

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

                var unionResult = Program.db.SetCombineAndStoreAsync(SetOperation.Union, unionDestinationSet, firstSet, secondtSet);
                unionResult.Wait();

                var intersectionResult = Program.db.SetCombineAndStoreAsync(SetOperation.Intersect, intersectionDestinationSet, firstSet, secondtSet);
                intersectionResult.Wait();

                var differenceResult = Program.db.SetCombineAndStoreAsync(SetOperation.Difference, differenceDestinationSet, firstSet, secondtSet);
                differenceResult.Wait();

                if (unionResult.Result == 15)
                {
                    Logger.PrintSuccessfulOutcome("Union of already existing sets was successfully evaluated and stored in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Union of already existing sets was failed and not stored");
                }

                if (intersectionResult.Result == 5)
                {
                    Logger.PrintSuccessfulOutcome("Intersection of already existing sets was successfully evaluated and stored in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Intersection of already existing sets was failed and not stored");
                }

                if (differenceResult.Result == 5)
                {
                    Logger.PrintSuccessfulOutcome("Difference of already existing sets was successfully evaluated and stored in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Difference of already existing sets was failed and not stored");
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

        public static void CombineAndStoreNonExistingFirstSetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously combining and storing non-exisitng first set in cache");

                var firstSet = Guid.NewGuid().ToString();
                var secondtSet = Guid.NewGuid().ToString();
                var unionDestinationSet = Guid.NewGuid().ToString();
                var intersectionDestinationSet = Guid.NewGuid().ToString();
                var differenceDestinationSet = Guid.NewGuid().ToString();

                for (int i = 0; i < 15; i++)
                {
                    Program.db.SetAdd(secondtSet, i + 1);
                }

                var unionResult = Program.db.SetCombineAndStoreAsync(SetOperation.Union, unionDestinationSet, firstSet, secondtSet);
                unionResult.Wait();

                var intersectionResult = Program.db.SetCombineAndStoreAsync(SetOperation.Intersect, intersectionDestinationSet, firstSet, secondtSet);
                intersectionResult.Wait();

                var differenceResult = Program.db.SetCombineAndStoreAsync(SetOperation.Difference, differenceDestinationSet, firstSet, secondtSet);
                differenceResult.Wait();

                if (unionResult.Result == 15)
                {
                    Logger.PrintSuccessfulOutcome("Union of non-existing first set was successfully evaluated and stored in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Union of non-existing first set was failed and not stored");
                }

                if (intersectionResult.Result == 0)
                {
                    Logger.PrintSuccessfulOutcome("Intersection of non-existing first set was successfully evaluated and stored in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Intersection of non-existing first set was failed and not stored");
                }

                if (differenceResult.Result == 0)
                {
                    Logger.PrintSuccessfulOutcome("Difference of non-existing first set was successfully evaluated and stored in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Difference of non-existing first set was failed and not stored");
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

        public static void CombineAndStoreNonExistingSecondSetAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously combining and storing non-exisitng second set in cache");

                var firstSet = Guid.NewGuid().ToString();
                var secondtSet = Guid.NewGuid().ToString();
                var unionDestinationSet = Guid.NewGuid().ToString();
                var intersectionDestinationSet = Guid.NewGuid().ToString();
                var differenceDestinationSet = Guid.NewGuid().ToString();

                for (int i = 0; i < 15; i++)
                {
                    Program.db.SetAdd(firstSet, i + 1);
                }

                var unionResult = Program.db.SetCombineAndStoreAsync(SetOperation.Union, unionDestinationSet, firstSet, secondtSet);
                unionResult.Wait();

                var intersectionResult = Program.db.SetCombineAndStoreAsync(SetOperation.Intersect, intersectionDestinationSet, firstSet, secondtSet);
                intersectionResult.Wait();

                var differenceResult = Program.db.SetCombineAndStoreAsync(SetOperation.Difference, differenceDestinationSet, firstSet, secondtSet);
                differenceResult.Wait();

                if (unionResult.Result == 15)
                {
                    Logger.PrintSuccessfulOutcome("Union of non-existing second set was successfully evaluated and stored in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Union of non-existing second set was failed and not stored");
                }

                if (intersectionResult.Result == 0)
                {
                    Logger.PrintSuccessfulOutcome("Intersection of non-existing second set was successfully evaluated and stored in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Intersection of non-existing second set was failed and not stored");
                }

                if (differenceResult.Result == 15)
                {
                    Logger.PrintSuccessfulOutcome("Difference of non-existing second set was successfully evaluated and stored in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Difference of non-existing second set was failed and not stored");
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

        public static void CombineAndStoreNonExistingSetsAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously combining and storing non-exisitng sets in cache");

                var firstSet = Guid.NewGuid().ToString();
                var secondtSet = Guid.NewGuid().ToString();
                var unionDestinationSet = Guid.NewGuid().ToString();
                var intersectionDestinationSet = Guid.NewGuid().ToString();
                var differenceDestinationSet = Guid.NewGuid().ToString();
                var unionResult = Program.db.SetCombineAndStoreAsync(SetOperation.Union, unionDestinationSet, firstSet, secondtSet);
                unionResult.Wait();

                var intersectionResult = Program.db.SetCombineAndStoreAsync(SetOperation.Intersect, intersectionDestinationSet, firstSet, secondtSet);
                intersectionResult.Wait();

                var differenceResult = Program.db.SetCombineAndStoreAsync(SetOperation.Difference, differenceDestinationSet, firstSet, secondtSet);
                differenceResult.Wait();

                if (unionResult.Result == 0)
                {
                    Logger.PrintSuccessfulOutcome("Union of non-existing sets was successfully evaluated and stored in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Union of non-existing sets was failed and not stored");
                }

                if (intersectionResult.Result == 0)
                {
                    Logger.PrintSuccessfulOutcome("Intersection of non-existing sets was successfully evaluated and stored in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Intersection of non-existing sets was failed and not stored");
                }

                if (differenceResult.Result == 0)
                {
                    Logger.PrintSuccessfulOutcome("Difference of non-existing sets was successfully evaluated and stored in cache");
                }
                else
                {
                    Logger.PrintFailureOutcome("Difference of non-existing sets was failed and not stored");
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
