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
            var mlContext = new MLContext(seed: 0);

            var prepareData = new PrepareData(mlContext);

            prepareData.OneHotEncoding().Preview(5, 5).PrintByColumn();
            prepareData.StringsSingleToDouble().Preview(5, 5).PrintByColumn();
            prepareData.ConcatenateLabel().Preview(5, 5).PrintByColumn();
            prepareData.ConcatenateFeatures().Preview(5, 5).PrintByColumn();


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
