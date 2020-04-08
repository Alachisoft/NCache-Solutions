using Alachisoft.NCache.Client;
using Alachisoft.NCache.Client.DataTypes.Collections;
using Alachisoft.NCache.Runtime.Caching;
using Regression_TaxiFarePrediction.DataStructures;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;

namespace AddTripDataInCache
{
    class Program
    {
        static ICache Cache;
        static ITopic Topic;
        
        private static IDistributedList<TaxiTrip> TrainingData;

        static void Main(string[] args)
        {
            string cache = ConfigurationManager.AppSettings["CacheID"];

            if (String.IsNullOrEmpty(cache))
            {
                Console.WriteLine("The CacheID cannot be null or empty.");
                return;
            }

            Cache = CacheManager.GetCache(cache);

            string topic = ConfigurationManager.AppSettings["TopicName"];

            if (String.IsNullOrEmpty(topic))
            {
                Console.WriteLine("Topic name cannot be null or empty.");
                return;
            }

            Topic = Cache.MessagingService.GetTopic(topic);
            string datasetKey = ConfigurationManager.AppSettings["TrainingData"];

            if (String.IsNullOrEmpty(datasetKey))
            {
                Console.WriteLine("Training data key cannot be null or empty.");
                return;
            }
            TrainingData = Cache.DataTypeManager.GetList<TaxiTrip>(datasetKey);

            if (Topic == null)
                Topic = Cache.MessagingService.CreateTopic(topic);

            //Load new data
            Thread loadData = new Thread(LoadDataInCache);
            loadData.Start();
            Console.WriteLine("Data streaming started....");
        }
      

        public static void LoadDataInCache()
        {
            var currentLine = string.Empty;
            string baseDataRelativePath = @"../../../../Data";
            string filepath = GetAbsolutePath($"{baseDataRelativePath}/taxi-fare-full.csv");
            List<TaxiTrip> dataList = new List<TaxiTrip>();
            int i = 0;
            
            using (StreamReader r = new StreamReader(filepath))
            {
                while ((currentLine = r.ReadLine()) != null)
                {
                    string[] incomingData = currentLine.Split(",");
                    TaxiTrip data = new TaxiTrip();

                    data.RateCode = float.Parse(incomingData[0]);
                    data.PassengerCount = float.Parse(incomingData[1]);
                    data.TripTime = float.Parse(incomingData[2]);
                    data.TripDistance = float.Parse(incomingData[3]);
                    data.FareAmount = float.Parse(incomingData[4]);

                    dataList.Add(data);
                    
                    if(dataList.Count > 100)
                    {                       
                        //Remove first entries and move sliding window
                        if (TrainingData.Count > dataList.Count)
                        {
                            TrainingData.RemoveRange(0, dataList.Count);
                        }

                        TrainingData.AddRange(dataList);
                        //publish message for notifying data upate
                        Topic.Publish(new Message("DataSet has been updated."), DeliveryOption.All);
                        dataList.Clear();
                        Console.WriteLine("Chunk of data has been added:" + i);
                        i++;
                        //Sleep for 5 minutes
                        Thread.Sleep(30000);
                    }
                }
            }
        }

        public static string GetAbsolutePath(string relativePath)
        {
            
        FileInfo _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            string fullPath = Path.Combine(assemblyFolderPath, relativePath);

            return fullPath;
        }
    }
}
