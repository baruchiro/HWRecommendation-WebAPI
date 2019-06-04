using System;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.AutoML;
using Microsoft.ML.Data;
using Regression.DataClasses;

namespace RegressionAutoML
{
    class Program
    {
        static readonly string _originalDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "fake-data.csv");
        static void Main(string[] args)
        {
            var mlContext = new MLContext(seed: 0);

            IDataView trainingDataView = mlContext.Data.LoadFromTextFile<PersonAndComputer>(_originalDataPath, hasHeader: true);
            IDataView testDataView = mlContext.Data.LoadFromTextFile<PersonAndComputer>(_originalDataPath, hasHeader: true);

            // Run AutoML binary classification experiment
            ExperimentResult<RegressionMetrics> experimentResult = mlContext.Auto()
                .CreateRegressionExperiment(20)
                .Execute(trainingDataView, "DiskRpm");

            ITransformer model = experimentResult.BestRun.Model;

            var predictions = model.Transform(testDataView);
            var metrics = mlContext.Regression.Evaluate(predictions, scoreColumnName: "Score");
        }
    }
}
