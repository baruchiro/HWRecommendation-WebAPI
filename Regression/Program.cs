using System;
using System.IO;
using Microsoft.ML;
using Regression.DataClasses;

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

            var model = Train(mlContext, _originalDataPath);
        }

        public static ITransformer Train(MLContext mlContext, string dataPath)
        {
            var dataView =
                mlContext.Data.LoadFromTextFile<PersonAndComputer>(dataPath, hasHeader: true, separatorChar: ',');

            var pipeline = mlContext.Transforms.Categorical.OneHotEncoding("GpuProcessorNumeric", "GpuProcessor")
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("GpuNameNumeric", "GpuName"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("MotherBoardNameNumeric", "MotherBoardName"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("DiskCapacityNumeric", "DiskCapacity"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("DiskTypeNumeric", "DiskType"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("DiskModelNumeric", "DiskModel"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("MemoryTypeNumeric", "MemoryType"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("MemoryCapacityNumeric", "MemoryCapacity"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(
                    "ProcessorArchitectureNumeric", "ProcessorArchitecture"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("ProcessorNameNumeric", "ProcessorName"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("FieldInterestNumeric", "FieldInterest"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("MainUseNumeric", "MainUse"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("GenderNumeric", "Gender"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("ComputerTypeNumeric", "ComputerType"))

                .Append(mlContext.Transforms.Concatenate("Label", "GpuProcessor", "GpuName",
                    "MotherBoardSataConnections", "MotherBoardMaxRam", "MotherBoardDdrSockets", "MotherBoardName",
                    "DiskCapacity", "DiskRpm", "DiskType", "DiskModel", "MemoryMHz", "MemoryType", "MemoryCapacity",
                    "ProcessorArchitecture", "ProcessorNumOfCores", "ProcessorGHz", "ProcessorName"))

                .Append(mlContext.Transforms.Concatenate("Features", "Age", "FieldInterestNumeric", "MainUseNumeric",
                    "GenderNumeric", "People", "ComputerTypeNumeric", "Price"))

                .Append(mlContext.Regression.Trainers.FastTree());

            
            var model = pipeline.Fit(dataView);
            return model;
        }
    }
}
