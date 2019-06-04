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
        static readonly string _originalDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "fake-data-orig.csv");
        static readonly string _trainDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "fake-data-train.csv");
        static readonly string _testDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "fake-data-test.csv");
        static readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "Model.zip");
        static void Main(string[] args)
        {
            var mlContext = new MLContext(seed: 0);

            IDataView dataView = mlContext.Data.LoadFromTextFile<PersonAndComputer>(_originalDataPath, hasHeader: true, separatorChar: ',');
            dataView.PrintPreview();

            var model = Train(mlContext, _originalDataPath);
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

        public static ITransformer Train(MLContext mlContext, string dataPath)
        {
            var stringColumns = new[]
            {
                "GpuProcessor", "GpuName", "MotherBoardName", "DiskCapacity", "DiskType", "DiskModel", "MemoryType",
                "MemoryCapacity", "ProcessorArchitecture", "ProcessorName", "FieldInterest", "MainUse", "Gender",
                "ComputerType"
            };

            var stringColumnsInputOutputColumnPair = stringColumns.Select(c => new InputOutputColumnPair(c)).ToArray();

            var dataView =
                mlContext.Data.LoadFromTextFile<PersonAndComputer>(dataPath, hasHeader: true, separatorChar: ',');

            var pipeline = mlContext.Transforms.Categorical.OneHotEncoding(stringColumnsInputOutputColumnPair)

                .Append(
                    mlContext.Transforms.Conversion.ConvertType(stringColumnsInputOutputColumnPair, DataKind.Double))

                .Append(mlContext.Transforms.Concatenate("Label", "GpuProcessor", "GpuName",
                    "MotherBoardSataConnections", "MotherBoardMaxRam", "MotherBoardDdrSockets", "MotherBoardName",
                    "DiskCapacity", "DiskRpm", "DiskType", "DiskModel", "MemoryMHz", "MemoryType", "MemoryCapacity",
                    "ProcessorArchitecture", "ProcessorNumOfCores", "ProcessorGHz", "ProcessorName"))

                .Append(mlContext.Transforms.Concatenate("Features", "Age", "FieldInterest", "MainUse",
                    "Gender", "People", "ComputerType", "Price"))

                .Append(mlContext.Regression.Trainers.FastTree());


            var model = pipeline.Fit(dataView);

            return model;
        }
    }
}
