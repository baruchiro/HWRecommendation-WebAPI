using System;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using Regression.DataClasses;
using Regression.Extensions;

namespace Regression
{
    class Program
    {
        static readonly string _trainDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "fake-data-train.csv");
        static readonly string _testDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "fake-data-test.csv");
        static readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "Model.zip");

        static void Main(string[] args)
        {
            var separatorLine = "------------------------------------------------------------------------------";
            var mlContext = new MLContext(seed: 0);

            var prepareData = new PrepareData(mlContext);

            prepareData.Preview(3, 3).PrintByColumn(true, separatorLine: separatorLine);
            prepareData.OneHotEncoding().Preview(3, 3).PrintByColumn(separatorLine:separatorLine);
            prepareData.ConcatenateLabel().Preview(3, 3).PrintByColumn(separatorLine: separatorLine);
            prepareData.ConcatenateFeatures().Preview(3, 3).PrintByColumn(separatorLine: separatorLine);

            var dataView = prepareData.Apply().DataView();
            var csv = dataView.ToCsv(mlContext);

            var model = Train(mlContext, dataView);

            Evaluate(mlContext, model, dataView);
        }

        private static ITransformer Train(MLContext mlContext, IDataView dataView)
        {
            return mlContext.Regression.Trainers.FastTree().Fit(dataView);
        }

        private static void Evaluate(MLContext mlContext, ITransformer model, IDataView dataView)
        {
            var predictions = model.Transform(dataView);
            var metrics = mlContext.Regression.Evaluate(predictions);

            Console.WriteLine();
            Console.WriteLine($"*************************************************");
            Console.WriteLine($"*       Model quality metrics evaluation         ");
            Console.WriteLine($"*------------------------------------------------");

            Console.WriteLine($"*       RSquared Score:      {metrics.RSquared:0.##}");

            Console.WriteLine($"*       Root Mean Squared Error:      {metrics.RootMeanSquaredError:#.##}");
        }

        public static IDataView Transform(MLContext mlContext, IDataView input)
        {
            // Define pipeline
            var pipeline = mlContext.Transforms.Categorical
                .OneHotEncoding("GpuProcessor", "GpuProcessor")
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("GpuName", "GpuName"));

            // Fitting generates a transformer that applies the operations of defined by estimator
            ITransformer replacementTransformer = pipeline.Fit(input);

            // Transform data
            IDataView output = replacementTransformer.Transform(input);

            return output;
        }
    }
}
