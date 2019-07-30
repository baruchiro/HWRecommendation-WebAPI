using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace AlgorithmManager
{
    public class PipelineBuilder
    {
        private readonly List<DataViewSchema.DetachedColumn> _schema;
        private IEstimator<ITransformer> _pipeline;
        private readonly MLContext _mlContext;
        private readonly List<string> _selectedColumns = new List<string>();

        public PipelineBuilder(MLContext mlContext, DataViewSchema dataViewSchema) : this(mlContext,
            dataViewSchema as IEnumerable<DataViewSchema.Column>)
        {
        }

        public PipelineBuilder(MLContext mlContext, IEnumerable<DataViewSchema.Column> dataViewSchema)
        {
            _mlContext = mlContext;
            _schema = dataViewSchema.Select(d => new DataViewSchema.DetachedColumn(d)).ToList();

        }

        public PipelineBuilder SelectColumns(params string[] columnsNames)
        {
            _selectedColumns.AddRange(columnsNames);
            return this;
        }

        public PipelineBuilder ConvertIntToSingle()
        {
            ConvertColumnsToType(SelectSchemaIndex(c => c.Type.RawType == typeof(int)), DataKind.Single);

            return this;
        }

        private List<int> SelectSchemaIndex(Func<DataViewSchema.DetachedColumn, bool> func)
        {
            return _schema
                .Select((c, i) => (Column: c, Index: i))
                .Where(t => func(t.Column))
                .Select(t => t.Item2)
                .ToList();
        }

        /// <summary>
        /// Add <see cref="IEstimator{TTransformer}"/> to convert all <see cref="NumberDataViewType"/> columns to <see cref="DataKind.Single"/>. (Known as <seealso cref="Single"/> or <seealso cref="float"/>
        /// </summary>
        /// <returns>Called <see cref="PipelineBuilder"/></returns>
        public PipelineBuilder ConvertNumberToSingle(bool convertVectors = true)
        {
            ConvertColumnsToType(SelectSchemaIndex(column => column.Type is NumberDataViewType ||
                                                             convertVectors &&
                                                             column.Type is VectorDataViewType vector &&
                                                             vector.ItemType is NumberDataViewType),
                DataKind.Single);

            return this;
        }

        private void ConvertColumnsToType(List<int> convertColumnIndex, DataKind convertedType)
        {
            AddEstimatorAndKeepOldNames(pairs =>
                    _mlContext.Transforms.Conversion.ConvertType(pairs, convertedType),
                convertColumnIndex);

            convertColumnIndex.ForEach(i =>
            {
                var current = _schema[i];
                _schema[i] = new DataViewSchema.DetachedColumn(current.Name,
                    MLIndex.DataKindToDataViewType[convertedType],
                    current.Annotations);
            });
        }

        private void AddEstimatorAndKeepOldNames(
            Func<InputOutputColumnPair[], IEstimator<ITransformer>> estimatorAction,
            IEnumerable<int> columnsToConvertIndex)
        {
            var prefix = "Temp" + new Random().Next(100) + "__";
            var columnsNames = columnsToConvertIndex.Select(i => _schema[i].Name).ToList();

            AddPipelineStage(estimatorAction(columnsNames.Select(c =>
                new InputOutputColumnPair(prefix + c, c)).ToArray()));

            AddPipelineStage(_mlContext.Transforms.DropColumns(columnsNames.ToArray()));

            columnsNames.ForEach(c =>
                AddPipelineStage(_mlContext.Transforms.CopyColumns(c, prefix + c)));

            AddPipelineStage(
                _mlContext.Transforms.DropColumns(columnsNames.Select(c => prefix + c).ToArray()));
        }

        private void AddPipelineStage(IEstimator<ITransformer> estimator)
        {
            _pipeline = _pipeline?.Append(estimator) ?? estimator;
        }

        private IEstimator<ITransformer> GetPipelineWithSelectedColumns()
        {
            var resultPipeline = _pipeline;
            // ReSharper disable once InvertIf
            if (_selectedColumns.Count > 0)
            {
                IEstimator<ITransformer> estimator = _mlContext.Transforms.SelectColumns(_selectedColumns.ToArray());
                resultPipeline = resultPipeline?.Append(estimator) ?? estimator;
            }

            return resultPipeline;
        }

        public IDataView TransformData(IDataView dataView)
        {
            if (dataView == null) throw new ArgumentNullException(nameof(dataView));

            return GetPipelineWithSelectedColumns()?.Fit(dataView).Transform(dataView) ?? dataView;
        }

        public IEstimator<ITransformer> GetEstimator()
        {
            return GetPipelineWithSelectedColumns();
        }
    }
}