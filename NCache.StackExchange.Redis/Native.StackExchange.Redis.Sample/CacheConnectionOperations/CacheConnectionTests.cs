
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Native.StackExchange.Redis.Sample.CacheConnectionOperations
{
    class CacheConnectionTests
    {
        public static void ChechCacheConnection()
        {
            try
            {
                Logger.PrintTestStartInformation("Checking if cache is connected");

                if (Program.db.IsConnected(ConfigurationManager.AppSettings["ServerIP"]))
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
