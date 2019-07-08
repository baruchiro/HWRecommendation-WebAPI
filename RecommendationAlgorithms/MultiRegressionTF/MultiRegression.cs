using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace MultiRegressionTF
{
    public class MultiRegression
    {
        private readonly IDictionary<string, DataKind> _strToDtypes = new Dictionary<string, DataKind>
        {
            {"int64", DataKind.Int64},
            {"object", DataKind.String},
            {"float64", DataKind.Double}
        };

        private readonly string[] _yColumns = {"age", "computertype", "fieldinterest", "gender", "mainuse", "price"};

        private TextLoader.Column[] ReadColumnsFromList(string[] columns)
        {
            return columns.Select(l => l.Split(','))
                .Select((line, index) => new TextLoader.Column(line[0], _strToDtypes[line[1].ToLower()], index))
                .ToArray();
        }

        public void CreateModel(IFileHandle trainDataCsv, string[] trainDtypesCsv)
        {
            //Create ML Context with seed for repeteable/deterministic results
            MLContext mlContext = new MLContext(seed: 0);

            var columns = ReadColumnsFromList(trainDtypesCsv);
            var categoricalColumns = columns.Where(c => c.DataKind == DataKind.String)
                .Select(c => new InputOutputColumnPair(c.Name));
            var numericColumns = columns.Where(c => c.DataKind == DataKind.Int64 || c.DataKind == DataKind.Double)
                .Select(c => new InputOutputColumnPair(c.Name));

            // STEP 1: Common data loading configuration
            TextLoader textLoader = mlContext.Data.CreateTextLoader(columns,
                hasHeader: true,
                separatorChar: ','
            );

            var fakedDataView = textLoader.Load(new FileHandleSource(trainDataCsv));
            var split = mlContext.Data.TrainTestSplit(fakedDataView, 0.2);

            var baseTrainingDataView = split.TrainSet;
            var testDataView = split.TestSet;

            // Categorical data
            var dataProcessPipeline = mlContext.Transforms.Categorical.OneHotEncoding(categoricalColumns.ToArray())
                    // Normalize
                    .Append(mlContext.Transforms.NormalizeMinMax("age", "age"))
                //.Append(mlContext.Transforms.NormalizeMeanVariance(numericColumns.ToArray()))
                //.Append(mlContext.Transforms.Concatenate("Y", _yColumns))
                //.Append(mlContext.Transforms.Concatenate("X",
                //    "VendorIdEncoded", "RateCodeEncoded", "PaymentTypeEncoded", "PassengerCount", "TripTime",
                //    "TripDistance"));
                //.Append(mlContext.Model.LoadTensorFlowModel("Resources/saved_model.pb")
                //    .ScoreTensorFlowModel(new[] {"Y"}, new[] {"X"}));
                ;
            var trainedModel = dataProcessPipeline.Fit(baseTrainingDataView);
        }
    }
}
