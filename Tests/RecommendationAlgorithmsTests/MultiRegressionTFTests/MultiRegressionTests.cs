using System;
using System.IO;
using MultiRegressionTF;
using Xunit;

namespace MultiRegressionTFTests
{
    public class MultiRegressionTests
    {
        [Fact]
        public void CreateModel()
        {
            var trainData = new StringHandler(Resources.Resources.fake_data_out);
            var trainDtypes = Resources.Resources.fake_data_out_dtypes
                .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            var multiRegression = new MultiRegression();
            multiRegression.CreateModel(trainData, trainDtypes);
        }
    }
}
