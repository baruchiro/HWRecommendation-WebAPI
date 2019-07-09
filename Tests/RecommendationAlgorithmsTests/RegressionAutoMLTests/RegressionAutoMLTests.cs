using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using Xunit;

namespace RegressionAutoMLTests
{
    public class RegressionAutoMLTests
    {
        private readonly string _fakeDataFilePath = Path.Combine("Resources", "fake-data-out.csv");
        private readonly string _fakeDataDtypesFilePath = Path.Combine("Resources", "fake-data-out.dtypes.csv");

        [Fact]
        public void Train()
        {
            var mlContext = new MLContext();

            var columns = ReadColumnsFromDtypesFile(_fakeDataDtypesFilePath);
            var trainData = mlContext.Data.LoadFromTextFile(_fakeDataFilePath, columns, ',', true);

            var regressionAutoML = new RegressionAutoML.RegressionAutoML();
            regressionAutoML.Train(trainData);
        }

        private readonly IDictionary<string, DataKind> _strToDtypes = new Dictionary<string, DataKind>
        {
            {"int64", DataKind.Int64},
            {"object", DataKind.String},
            {"float64", DataKind.Double}
        };

        private TextLoader.Column[] ReadColumnsFromDtypesFile(string dtypesFilePath)
        {
            return File.ReadAllLines(dtypesFilePath)
                .Select(l => l.Split(','))
                .Select((line, index) => 
                    new TextLoader.Column(line[0],
                        _strToDtypes[line[1].ToLower()],
                        index))
                .ToArray();
        }
    }
}
