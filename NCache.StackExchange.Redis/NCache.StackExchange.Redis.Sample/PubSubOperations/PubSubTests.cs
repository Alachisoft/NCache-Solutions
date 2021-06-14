using BasicUsageStackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace NCache.StackExchange.Redis.Sample.PubSubOperations
{
    class PubSubTests
    {

        //------------------------------------------------Sync Methods------------------------------------------------\\

        public static void PublishMessageOnExistingTopic()
        {
            try
            {
                Logger.PrintTestStartInformation("Publishing message on already existing channel in cache");

                var message = "This is a test message";
                var channelName = "ExistingTestChannel";

                Program.db.Publish(channelName, message);

                var result = Program.db.Publish(channelName, message);

                if (result == 1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully published message on already existing channel");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to publish message on already existing channel");
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

        public static void PublishMessageOnNonExistingTopic()
        {
            try
            {
                Logger.PrintTestStartInformation("Publishing message on non-existing channel in cache");

                var message = "This is a test message";
                var channelName = "NonExistingTestChannel";
                var result = Program.db.Publish(channelName, message);

                if (result == 1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully published message on non-existing channel");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to publish message on non-existing channel");
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

        public static void SubscribeOnChannel()
        {
            try
            {
                Logger.PrintTestStartInformation("Subscribing on already existing channel in cache");

                var channelName = "ExistingTestChannel";
                var result = Program.db.Subscribe(channelName);

                result.OnMessage((ChannelMessage msg) =>
                {
                    Logger.PrintSuccessfulOutcome("Message received \nChannel: " + msg.Channel + " \nMessage: " + msg.Message);
                });
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

        public static void PublishMessageOnExistingTopicAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously publishing message on already existing channel in cache");

                var message = "This is a test message";
                var channelName = "ExistingTestChannel";

                Program.db.Publish(channelName, message);

                var result = Program.db.PublishAsync(channelName, message);

                result.Wait();

                if (result.Result == 1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully published message on already existing channel");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to publish message on already existing channel");
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

        public static void PublishMessageOnNonExistingTopicAsync()
        {
            try
            {
                Logger.PrintTestStartInformation("Asynchronously publishing message on non-existing channel in cache");

                var message = "This is a test message";
                var channelName = "NonExistingTestChannel";
                var result = Program.db.PublishAsync(channelName, message);

                result.Wait();

                if (result.Result == 1)
                {
                    Logger.PrintSuccessfulOutcome("Successfully published message on non-existing channel");
                }
                else
                {
                    Logger.PrintFailureOutcome("Unable to publish message on non-existing channel");
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
