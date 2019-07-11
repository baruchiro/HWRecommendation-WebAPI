using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace DataTestsUtils
{
    public class DataLoader
    {
        private readonly string _fakeDataFilePath = Path.Combine("Resources", "fake-data-out.csv");
        private readonly string _fakeDataDtypesFilePath = Path.Combine("Resources", "fake-data-out.dtypes.csv");
        private readonly MLContext _mlContext;
        private TextLoader.Column[] _columns;
        public IDataView Data { get; }
        public PipelineBuilder Builder => new PipelineBuilder(_mlContext, Data, _columns);

        public DataLoader(MLContext mlContext)
        {
            _mlContext = mlContext;
            _columns = ReadColumnsFromDtypesFile();
            Data = _mlContext.Data.LoadFromTextFile(_fakeDataFilePath, _columns, ',', true);
        }

        private readonly IDictionary<string, DataKind> _strToDtypes = new Dictionary<string, DataKind>
        {
            {"int64", DataKind.Int64},
            {"object", DataKind.String},
            {"float64", DataKind.Double}
        };

        private TypeConvertingEstimator _pipeline;


        private TextLoader.Column[] ReadColumnsFromDtypesFile()
        {
            return File.ReadAllLines(_fakeDataDtypesFilePath)
                .Select(l => l.Split(','))
                .Select((line, index) =>
                    new TextLoader.Column(line[0],
                        _strToDtypes[line[1].ToLower()],
                        index))
                .ToArray();
        }
    }

    public class PipelineBuilder
    {
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
        private TextLoader.Column[] columns;
        private IEstimator<ITransformer> _pipeline;
        private MLContext _mlContext;

        public PipelineBuilder(MLContext mlContext, IDataView data, TextLoader.Column[] columns)
        {
            this.data = data;
            this.columns = columns;
            _mlContext = mlContext;
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
            return _pipeline?.Fit(data).Transform(data) ?? data;
        }

        public PipelineBuilder ConvertNumberToSingle()
        {
            ConvertTypesToType(_intTypes.Concat(_doubleTypes).ToArray(), DataKind.Single);

            return this;
        }
    }
}
