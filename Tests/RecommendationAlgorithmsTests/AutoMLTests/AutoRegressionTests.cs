using System.Linq;
using AlgorithmManager.Extensions;
using AlgorithmManager.Model;
using AlgorithmManager.ModelAttributes;
using AlgoTestUtils;
using AutoML;
using Microsoft.ML;
using Xunit;

namespace AutoMLTests
{
    public class AutoRegressionTests
    {
        [Fact]
        public void TrainAll_ZeroTime_ValidateResultsCount()
        {
            var mlContext =  new MLContext(0);
            var fakeData = new FakeData(mlContext);
            var autoRegression = new AutoRegression();
            var labels = typeof(MLPersonComputerModel)
                .GetPropertiesNamesByAttribute<RegressionLabelAttribute>()
                .ToList();
            Assert.NotEmpty(labels);

            var results = autoRegression.TrainModel(mlContext, fakeData.EnumerateData(), 0)
                .ToList();

            Assert.NotEmpty(results);
            Assert.Equal(labels.Count, results.Count);
        }
    }
}
