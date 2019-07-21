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
        private MLContext _mlContext;

        public PipelineBuilderTests()
        {
            _mlContext = new MLContext(0);
        }

        [Fact]
        public void ConvertTypeToType_NumberToSingle_ValidateResources()
        {
            var dataView = new AlgorithmManagerFactory(_mlContext)
                .CreateDataViewFromCsv(_fakeDataFilePath, _fakeDataDtypesFilePath);
            var originalSchema = dataView.Schema.OrderBy(c=>c.Name).ToList();

            var convertedDataView = new PipelineBuilder(_mlContext, originalSchema)
                .ConvertNumberToSingle()
                .TransformData(dataView);

            var zip = originalSchema.Zip(convertedDataView.Schema.OrderBy(c=>c.Name),
                (orig, convert) => (orig.Type, convert.Type));

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
