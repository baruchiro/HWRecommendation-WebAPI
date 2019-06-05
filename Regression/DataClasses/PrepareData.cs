using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace Regression.DataClasses
{
    class PrepareData
    {
        readonly string _originalDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "fake-data-orig.csv");

        private readonly MLContext _mlContext;
        private IDataView _dataView;

        private readonly string[] _stringColumns =
        {
            "GpuProcessor", "GpuName", "MotherBoardName", "DiskCapacity", "DiskType", "DiskModel", "MemoryType",
            "MemoryCapacity", "ProcessorArchitecture", "ProcessorName", "FieldInterest", "MainUse", "Gender",
            "ComputerType"
        };

        private readonly string[] _labelsColumns =
        {
            "GpuProcessor", "GpuName",
            "MotherBoardSataConnections", "MotherBoardMaxRam", "MotherBoardDdrSockets", "MotherBoardName",
            "DiskCapacity", "DiskRpm", "DiskType", "DiskModel", "MemoryMHz", "MemoryType", "MemoryCapacity",
            "ProcessorArchitecture", "ProcessorNumOfCores", "ProcessorGHz", "ProcessorName"
        };

        private readonly string[] _featuresColumns =
        {
            "Age", "FieldInterest", "MainUse",
            "Gender", "ComputerType", "Price"
        };

        private readonly InputOutputColumnPair[] _stringColumnsInputOutputColumnPair;
        private IEstimator<ITransformer> _pipeline = null;

        public PrepareData(MLContext mlContext)
        {
            _mlContext = mlContext;
            _dataView = mlContext.Data.LoadFromTextFile<PersonAndComputer>(_originalDataPath, hasHeader: true,
                separatorChar: ',');
            _stringColumnsInputOutputColumnPair = _stringColumns.Select(c => new InputOutputColumnPair(c)).ToArray();
        }

        public PrepareData OneHotEncoding()
        {
            CreateOrAddTransform(_mlContext.Transforms.Categorical.OneHotEncoding(_stringColumnsInputOutputColumnPair));

            return this;
        }

        private void CreateOrAddTransform(IEstimator<ITransformer> estimator)
        {
            _pipeline = _pipeline?.Append(estimator) ?? estimator;
        }

        public PrepareData StringsSingleToDouble()
        {
            CreateOrAddTransform(
                _mlContext.Transforms.Conversion.ConvertType(_stringColumnsInputOutputColumnPair, DataKind.Double));

            return this;
        }

        public PrepareData ConcatenateLabel()
        {
            CreateOrAddTransform(_mlContext.Transforms.Concatenate("Label", _labelsColumns));

            return this;
        }

        public PrepareData ConcatenateFeatures()
        {
            CreateOrAddTransform(_mlContext.Transforms.Concatenate("Features", _featuresColumns));

            return this;
        }

        //public IDataView Apply()
        //{
        //    ITransformer replacementTransformer = _pipeline.Fit(_dataView);
        //    _dataView = replacementTransformer.Transform(_dataView);

        //    _pipeline = null;

        //    return _dataView;
        //}
        public DataDebuggerPreview Preview(int maxRows = 100, int maxTrainingRows = 100)
        {
            return _pipeline?.Preview(_dataView, maxRows, maxTrainingRows) ?? _dataView.Preview(maxRows);
        }
    }
}
