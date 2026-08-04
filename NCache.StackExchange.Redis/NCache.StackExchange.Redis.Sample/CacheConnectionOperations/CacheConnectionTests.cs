using BasicUsageStackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCache.StackExchange.Redis.Sample.CacheConnectionOperations
{
    class CacheConnectionTests
    {
        public static void ChechCacheConnection()
        {
            try
            {
                Logger.PrintTestStartInformation("Checking if cache is connected");

                if (Program.db.IsConnected("DummyKey"))
                {
                    Logger.PrintSuccessfulOutcome("Cache is connected");
                }
                else
                {
                    Logger.PrintFailureOutcome("Cache is not connected");
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
