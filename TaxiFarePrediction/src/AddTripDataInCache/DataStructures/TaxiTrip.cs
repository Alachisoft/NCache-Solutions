
using Microsoft.ML.Data;
using System;

namespace Regression_TaxiFarePrediction.DataStructures
{
    [Serializable]
    public class TaxiTrip
    {
        [ColumnName("Label")]
        public float RateCode;

        [LoadColumn(0)]
        public float PassengerCount;

        [LoadColumn(1)]
        public float TripTime;

        [LoadColumn(2)]
        public float TripDistance;

        [LoadColumn(3)]
        public float FareAmount;
    }

    public class TransformedTaxiTrip
    { 
        [VectorType(4)]
        public float[] Features;
        public float Label;
    }
}