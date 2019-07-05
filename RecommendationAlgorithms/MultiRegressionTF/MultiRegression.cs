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

            // STEP 1: Common data loading configuration
            TextLoader textLoader = mlContext.Data.CreateTextLoader(columns,
                hasHeader: true,
                separatorChar: ','
            );

            var fakedDataView = textLoader.Load(new FileHandleSource(trainDataCsv));
            var split = mlContext.Data.TrainTestSplit(fakedDataView, 0.2);

            var baseTrainingDataView = split.TrainSet;
            var testDataView = split.TestSet;

            var dataProcessPipeline = mlContext.Transforms.Categorical.OneHotEncoding(columns
                .Where(c=>c.DataKind == DataKind.Single)
                .Select(c=>new InputOutputColumnPair($"cat_{c.Name}", c.Name)).ToArray(), keyOrdinality:ValueToKeyMappingEstimator.KeyOrdinality.ByValue )
                .Append(mlContext.Transforms.Concatenate("Y", _yColumns))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("VendorId", "VendorIdEncoded"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("RateCode", "RateCodeEncoded"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("PaymentType", "PaymentTypeEncoded"))
                .Append(mlContext.Transforms.NormalizeMeanVariance(inputName: "PassengerCount", mode: NormalizerMode.MeanVariance))
                .Append(mlContext.Transforms.Normalize(inputName: "TripTime", mode: NormalizerMode.MeanVariance))
                .Append(mlContext.Transforms.Normalize(inputName: "TripDistance", mode: NormalizerMode.MeanVariance))
                .Append(mlContext.Transforms.Concatenate("X", "VendorIdEncoded", "RateCodeEncoded", "PaymentTypeEncoded", "PassengerCount", "TripTime", "TripDistance"))
                .Append(new TensorFlowEstimator(mlContext, new TensorFlowTransform.Arguments()
                {
                    ModelLocation = "NYCTaxi/model", // Model is created with this script: DeepLearningWithMLdotNet\NYCTaxiMultiOutputRegression\TF_MultiOutputLR.py
                    InputColumns = new[] { "X" },
                    OutputColumns = new[] { "RegScores" },
                    LabelColumn = "Y",
                    TensorFlowLabel = "Y",
                    OptimizationOperation = "MomentumOptimizer",
                    LossOperation = "Loss",
                    Epoch = 10,
                    LearningRateOperation = "learning_rate",
                    LearningRate = 0.01f,
                    BatchSize = 20,
                    ReTrain = true
                }));
        }
    }
}
