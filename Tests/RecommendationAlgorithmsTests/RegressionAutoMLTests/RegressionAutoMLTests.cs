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

            var data = dataLoader.Data;

            var regressionAutoML = new RegressionAutoML.RegressionAutoML();
            regressionAutoML.Train(data);
        }
    }
}
