// Copyright (c) 2026 Alachisoft.
using Microsoft.ML.Data;

namespace Regression_TaxiFarePrediction.DataStructures
{
    public class TaxiTripFarePrediction
    {
        [ColumnName("Score")]
        public float FareAmount;
    }
}