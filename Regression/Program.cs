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


            //var model = Train(mlContext, _originalDataPath);
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
