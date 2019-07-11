using DataTestsUtils;
using Microsoft.ML;
using Xunit;

namespace RegressionAutoMLTests
{
    public class RegressionAutoMLTests
    {

        [Fact]
        public void Train()
        {
            var mlContext = new MLContext();
            var dataLoader = new DataLoader(mlContext);

            var data = dataLoader.Builder.ConvertIntToSingle()
                .ConvertNumberToSingle()
                .SelectFeatureColumns()
                .SelectColumns("memory_capacity_as_kb")
                .GetData();

            var regressionAutoML = new RegressionAutoML.RegressionAutoML();
            var result = regressionAutoML.Train(data);

            var metrics = result.ValidationMetrics;
            var r = $"R-Squared: {metrics.RSquared:0.##}";
            var m = $"Root Mean Squared Error: {metrics.RootMeanSquaredError:0.##}";
        }
    }
}
