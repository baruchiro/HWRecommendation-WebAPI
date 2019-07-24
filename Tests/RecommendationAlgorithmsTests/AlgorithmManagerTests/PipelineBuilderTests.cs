using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AlgorithmManager;
using AlgorithmManager.Factories;
using Microsoft.ML;
using Microsoft.ML.Data;
using Xunit;

namespace AlgorithmManagerTests
{
    public class PipelineBuilderTests
    {
        private readonly string _fakeDataFilePath = Path.Combine("Resources", "fake-data-out.csv");
        private readonly string _fakeDataDtypesFilePath = Path.Combine("Resources", "fake-data-out.dtypes.csv");
        private readonly MLContext _mlContext;

        public PipelineBuilderTests()
        {
            _mlContext = new MLContext(0);
        }

        [Fact]
        public void SelectColumns_FeatureColumns_OnlyThem()
        {
            var schemaBuilder = new DataViewSchema.Builder();
            schemaBuilder.AddColumn("Test1", TextDataViewType.Instance);
            schemaBuilder.AddColumn("Test2", TextDataViewType.Instance);
            var schema = schemaBuilder.ToSchema();

            var dataView = _mlContext.Data.LoadFromEnumerable(new[]
            {
                new
                {
                    Test1 = "Hii", Test2 = "Xii"

                },
                new
                {
                    Test1 = "GGG", Test2 = "kkk"
                }
            }, schema);

            var actualSchema = new PipelineBuilder(_mlContext, schema)
                .SelectColumns("Test1")
                .TransformData(dataView)
                .Schema;

            Assert.Single(actualSchema);
            Assert.Contains("Test1", actualSchema.Select(c => c.Name));
        }

        [Fact]
        public void ConvertTypeToType_NumberToSingle_ValidateResources()
        {
            var dataView = new AlgorithmManagerFactory(_mlContext)
                .CreateDataViewFromCsv(_fakeDataFilePath, _fakeDataDtypesFilePath);
            var originalSchema = dataView.Schema.OrderBy(c=>c.Name).ToList();

            var convertedDataView = new PipelineBuilder(_mlContext, originalSchema)
                .ConvertNumberToSingle(false)
                .TransformData(dataView);

            var zip = originalSchema.Zip(convertedDataView.Schema.OrderBy(c=>c.Name),
                (orig, convert) => (orig.Type, convert.Type));

            Assert.All(zip, ConvertedFromTo<NumberDataViewType, float>);
        }

        [Fact]
        public void ConvertTypeToType_VectorNumberToVectorSingle_ValidateResources()
        {
            var dataView = new AlgorithmManagerFactory(_mlContext)
                .CreateDataViewFromCsv(_fakeDataFilePath, _fakeDataDtypesFilePath);
            var originalSchema = dataView.Schema.OrderBy(c => c.Name).ToList();

            var convertedDataView = new PipelineBuilder(_mlContext, originalSchema)
                .ConvertNumberToSingle()
                .TransformData(dataView);

            var zip = originalSchema.Zip(convertedDataView.Schema.OrderBy(c => c.Name),
                (orig, convert) =>
                    (orig.Type is VectorDataViewType origVector ? origVector.ItemType : orig.Type,
                        convert.Type is VectorDataViewType convertVector ? convertVector.ItemType : convert.Type));

            Assert.All(zip, ConvertedFromTo<NumberDataViewType, float>);
        }

        private void ConvertedFromTo<TFrom, TTo>((DataViewType from, DataViewType to) obj)
        {
            if (obj.from is TFrom)
            {
                Assert.Equal(typeof(TTo), obj.to.RawType);
            }
            else
            {
                Assert.Equal(obj.from, obj.to);
            }
        }
    }
}
