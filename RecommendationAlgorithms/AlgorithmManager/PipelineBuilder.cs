using System;
using System.Collections.Generic;
using System.Linq;
using AlgorithmManager.Factories;
using Microsoft.ML;
using Microsoft.ML.Data;
using Models;

namespace AlgorithmManager
{
    public class PipelineBuilder
    {
        private readonly string[] _featureColumns =
        {
            "age",
            "computertype",
            "fieldinterest",
            "mainuse",
            "price",
            "gender"
        };
        private readonly DataKind[] _intTypes = {
            DataKind.Byte,
            DataKind.SByte,
            DataKind.Int16,
            DataKind.Int32,
            DataKind.Int64,
            DataKind.UInt16,
            DataKind.UInt32,
            DataKind.UInt64
        };
        private readonly DataKind[] _doubleTypes =
        {
            DataKind.Double
        };

        private IDataView data;
        private readonly TextLoader.Column[] columns;
        private IEstimator<ITransformer> _pipeline;
        private readonly MLContext _mlContext;
        private readonly List<string> _selectedColumns = new List<string>();

        public PipelineBuilder(MLContext mlContext, IDataView data)
        {
            this.data = data;
            this.columns = columns;
            _mlContext = mlContext;
        }

        public PipelineBuilder SelectFeatureColumns() => SelectColumns(_featureColumns);

        public PipelineBuilder SelectColumns(params string[] columnsNames)
        {
            _selectedColumns.AddRange(columnsNames);
            return this;
        }

        public PipelineBuilder ConvertIntToSingle()
        {
            ConvertTypesToType(_intTypes, DataKind.Single);

            return this;
        }

        public void ConvertTypesToType(DataKind[] typesToConvert, DataKind convertedType)
        {
            var convertColumnNames = columns
                .Where(c => typesToConvert.Contains(c.DataKind))
                .Select(c => c.Name).ToList();
            const string addedName = "single_";

            AddPipelineStage(_mlContext.Transforms.Conversion.ConvertType(
                convertColumnNames.Select(c =>
                    new InputOutputColumnPair(addedName + c, c)).ToArray(),
                convertedType));
            AddPipelineStage(_mlContext.Transforms.DropColumns(convertColumnNames.ToArray()));
            convertColumnNames.ForEach(c => AddPipelineStage(_mlContext.Transforms.CopyColumns(c, addedName + c)));
            AddPipelineStage(_mlContext.Transforms.DropColumns(convertColumnNames.Select(c => addedName + c).ToArray()));

            foreach (var column in columns.Where(c=>convertColumnNames.Contains(c.Name)))
            {
                column.DataKind = convertedType;
            }
        }

        private void AddPipelineStage(IEstimator<ITransformer> estimator)
        {
            _pipeline = _pipeline?.Append(estimator) ?? estimator;
        }

        public IDataView GetData()
        {
            return _pipeline?.Append(_mlContext.Transforms.SelectColumns(_selectedColumns.ToArray()))
                       .Fit(data).Transform(data) ?? data;
        }

        public PipelineBuilder ConvertNumberToSingle()
        {
            ConvertTypesToType(_intTypes.Concat(_doubleTypes).ToArray(), DataKind.Single);

            return this;
        }
    }
}