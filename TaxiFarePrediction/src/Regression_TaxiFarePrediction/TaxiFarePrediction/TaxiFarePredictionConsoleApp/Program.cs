using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Client.DataTypes.Collections;
using Alachisoft.NCache.Runtime.Caching;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Regression_TaxiFarePrediction.DataStructures;

namespace Regression_TaxiFarePrediction
{
    internal static class Program
    {
        private static string ModelPath;
        private static string ModelParamsPath;
        private static ICache Cache;
        private static ITopic Topic;
        private static IDistributedList<TaxiTrip> TrainingData;
        private static bool ModelTrained = false;      

        static void Main(string[] args)
        {
            //Create ML Context with seed for repeatable/deterministic results
            var mlContext = new MLContext(seed: 0);

            //Read values from app.config and set paths
            SetPathValues();

            //Initialize cache and subscribe to topic
            InitializeCache();

            // Create, Train, Evaluate and Save a model
            if (!ModelTrained)           
                TrainModel(mlContext);

            // Make a single test prediction loding the model from .ZIP file
            TestSinglePrediction(mlContext);

            Console.WriteLine("Press any key to exit..");
            Console.ReadLine();
        }

        private static void InitializeCache()
        {
            string cache = ConfigurationManager.AppSettings["CacheID"];

            if (String.IsNullOrEmpty(cache))
            {
                Console.WriteLine("The CacheID cannot be null or empty.");
                return;
            }

            Cache = CacheManager.GetCache(cache);
            Console.WriteLine(string.Format("\nCache '{0}' is initialized.", cache));
            string topic = ConfigurationManager.AppSettings["TopicName"];

            if (String.IsNullOrEmpty(topic))
            {
                Console.WriteLine("Topic name cannot be null or empty.");
                return;
            }
            
            Topic = Cache.MessagingService.GetTopic(topic);
           
            if (Topic == null)
                Topic = Cache.MessagingService.CreateTopic(topic);
            
            Topic.CreateSubscription(TaxiFareDataUpdated);
            string datasetKey = ConfigurationManager.AppSettings["TrainingData"];

            if (String.IsNullOrEmpty(datasetKey))
            {
                Console.WriteLine("Training data key cannot be null or empty.");
                return;
            }

            TrainingData = Cache.DataTypeManager.GetList<TaxiTrip>(datasetKey);

            var modelpath = Cache.Get<string>(ModelPath);

            if (modelpath != null)
            {
                ModelTrained = true;
                ModelPath = modelpath;
            }

            if (TrainingData == null)
                TrainingData = Cache.DataTypeManager.CreateList<TaxiTrip>(datasetKey);
        }

        static void TaxiFareDataUpdated(object sender, MessageEventArgs args)
        {
            RetrainModel(new MLContext(seed: 0));
        }

        private static void RetrainModel(MLContext mlContext)
        {
            // Define DataViewSchema of data prep pipeline and trained model
            DataViewSchema dataPrepPipelineSchema, modelSchema;

            // Load data preparation pipeline
            ITransformer dataPrepPipeline = mlContext.Model.Load(ModelParamsPath, out dataPrepPipelineSchema);

            // Load trained model
            ITransformer trainedModel = mlContext.Model.Load(ModelPath, out modelSchema);

            // Extract trained model parameters
            LinearRegressionModelParameters originalModelParameters =
                ((ISingleFeaturePredictionTransformer<object>)trainedModel).Model as LinearRegressionModelParameters;

            //Load New Data
            IDataView newData = mlContext.Data.LoadFromEnumerable<TaxiTrip>(TrainingData);

            // Preprocess Data
            IDataView transformedNewData = dataPrepPipeline.Transform(newData);

            // Retrain model
            RegressionPredictionTransformer<LinearRegressionModelParameters> retrainedModel =
                mlContext.Regression.Trainers.OnlineGradientDescent()
                    .Fit(transformedNewData, originalModelParameters);

            // Extract Model Parameters of re-trained model
            LinearRegressionModelParameters retrainedModelParameters = retrainedModel.Model as LinearRegressionModelParameters;

            // Inspect Change in Weights
            var weightDiffs =
                originalModelParameters.Weights.Zip(
                    retrainedModelParameters.Weights, (original, retrained) => original - retrained).ToArray();

            Console.WriteLine("Original | Retrained | Difference");
            for (int i = 0; i < weightDiffs.Count(); i++)
            {
                Console.WriteLine($"{originalModelParameters.Weights[i]} | {retrainedModelParameters.Weights[i]} | {weightDiffs[i]}");
            }

            // Define data preparation estimator
            IEstimator<ITransformer> dataPrepEstimator =
                mlContext.Transforms.Concatenate("Features", new string[] { "PassengerCount", "TripTime", "TripDistance", "FareAmount" })
                    .Append(mlContext.Transforms.NormalizeMinMax("Features"));

            Console.WriteLine("The model has been retrained.");
            // Create data preparation transformer
            ITransformer dataPrepTransformer = dataPrepEstimator.Fit(newData);

            // Save Data Prep transformer
            mlContext.Model.Save(dataPrepTransformer, newData.Schema, ModelParamsPath);

            // Save Trained Model
            mlContext.Model.Save(retrainedModel, transformedNewData.Schema, ModelPath);
            Console.WriteLine("The retrained model is saved to {0}", ModelPath);
            TestSinglePrediction(mlContext);
            Console.WriteLine("-----------------------------------------------------------------------------------");
        }

        private static ITransformer TrainModel(MLContext mlContext)
        {
            // Common data loading configuration
            string initialdataKey = ConfigurationManager.AppSettings["InitialDataSet"];

            if (String.IsNullOrEmpty(initialdataKey))
            {
                Console.WriteLine("Initial DataSet key value cannot be null or empty.");
                return null;
            }

            LoadInitialDataInCache(initialdataKey);
            var datasetTraining = Cache.DataTypeManager.GetList<TaxiTrip>(initialdataKey);
            var inputDataView = mlContext.Data.LoadFromEnumerable(datasetTraining);

            //Sample code of removing extreme data like "outliers" for FareAmounts higher than $150 and lower than $1 which can be error-data 
            var cnt = inputDataView.GetColumn<float>(nameof(TaxiTrip.FareAmount)).Count();
            IDataView trainingDataView = mlContext.Data.FilterRowsByColumn(inputDataView, nameof(TaxiTrip.FareAmount), lowerBound: 1, upperBound: 150);
            var cnt2 = trainingDataView.GetColumn<float>(nameof(TaxiTrip.FareAmount)).Count();

            // Define data preparation estimator
            IEstimator<ITransformer> dataPrepEstimator =
                mlContext.Transforms.Concatenate("Features", new string[] { "PassengerCount", "TripTime", "TripDistance", "FareAmount" })
                    .Append(mlContext.Transforms.NormalizeMinMax("Features"));

            // Create data preparation transformer
            ITransformer dataPrepTransformer = dataPrepEstimator.Fit(trainingDataView);

            // Define StochasticDualCoordinateAscent regression algorithm estimator
            var sdcaEstimator = mlContext.Regression.Trainers.Sdca();

            // Pre-process data using data prep operations
            IDataView transformedData = dataPrepTransformer.Transform(trainingDataView);

            // Train regression model
            RegressionPredictionTransformer<LinearRegressionModelParameters> trainedModel = sdcaEstimator.Fit(transformedData);

            Console.WriteLine("Initial model trained.");
            // Save Data Prep transformer
            mlContext.Model.Save(dataPrepTransformer, trainingDataView.Schema, ModelParamsPath);

            // Save Trained Model
            mlContext.Model.Save(trainedModel, transformedData.Schema, ModelPath);
            Console.WriteLine("The model is saved to {0}", ModelPath);
            ModelTrained = true;
            Cache.Add(ModelPath, ModelPath);
            return trainedModel;
        }

        private static void TestSinglePrediction(MLContext mlContext)
        {
            var taxiTripSample = new DataStructures.TaxiTrip()
            {
                RateCode = 1,
                PassengerCount = 1,
                TripTime = 1140,
                TripDistance = 3.75f,
                FareAmount = 0// To predict. Actual/Observed = 15.5
            };
            
            DataViewSchema modelSchema;
            
            ITransformer trainedModel = mlContext.Model.Load(ModelPath, out modelSchema);
            // Load data preparation pipeline
            ITransformer dataPrepPipeline = mlContext.Model.Load(ModelParamsPath, out var dataPrepPipelineSchema);
            List<TaxiTrip> sampleData = new List<TaxiTrip>();
            sampleData.Add(taxiTripSample);
            //Load New Data
            IDataView newData = mlContext.Data.LoadFromEnumerable<TaxiTrip>(sampleData);

            // Preprocess Data
            IDataView transformedNewData = dataPrepPipeline.Transform(newData);

            // Create prediction engine related to the loaded trained model
            var predEngine = mlContext.Model.CreatePredictionEngine<TransformedTaxiTrip, TaxiTripFarePrediction>(trainedModel);

            var trans = mlContext.Data.CreateEnumerable<TransformedTaxiTrip>(transformedNewData, true);
            var input = trans.FirstOrDefault();
            ////Score
            var resultprediction = predEngine.Predict(input);
            ///////

            Console.WriteLine($"**********************************************************************");
            Console.WriteLine($"Predicted: {resultprediction.FareAmount:0.####}, actual: 1.097");
            Console.WriteLine($"**********************************************************************");
        }

        private static void LoadInitialDataInCache(string initialData)
        {
            var currentLine = string.Empty;
            string datapath = ConfigurationManager.AppSettings["BaseDataRelativePath"];

            if (String.IsNullOrEmpty(datapath))
            {
                Console.WriteLine("Data path cannot be null or empty.");
                return;
            }

            string filepath = GetAbsolutePath($"{datapath}/taxi-fare-initial.csv");
            IDistributedList<TaxiTrip> initialDataList = Cache.DataTypeManager.GetList<TaxiTrip>(initialData);

            if (initialDataList == null)
            {
                initialDataList = Cache.DataTypeManager.CreateList<TaxiTrip>(initialData);

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

                        initialDataList.Add(data);
                        TrainingData.Add(data);
                    }
                }
            }
        }

        private static void SetPathValues()
        {
            string modelpath = ConfigurationManager.AppSettings["BaseModelsRelativePath"];

            if (String.IsNullOrEmpty(modelpath))
            {
                Console.WriteLine("Model relative path cannot be null or empty.");
                return;
            }

            string pipelinepath = ConfigurationManager.AppSettings["BaseModelsRelativePath"];

            if (String.IsNullOrEmpty(modelpath))
            {
                Console.WriteLine("The CacheID cannot be null or empty.");
                return;
            }

            ModelPath = GetAbsolutePath($"{modelpath}/TaxiFareModel.zip");
            ModelParamsPath = GetAbsolutePath($"{modelpath}/DataPreparationPipeline.zip");
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
