using AlgorithmManager;
using Microsoft.ML;
using Models;
using System.Collections.Generic;
using System.IO;

namespace AlgoTestUtils
{
    public class FakeData
    {
        private readonly string _fakeDataFilePath = Path.Combine("Resources", "fake-data-out.csv");
        private readonly string _fakeDataDtypesFilePath = Path.Combine("Resources", "fake-data-out.dtypes.csv");
        private MLContext _mlContext;
        public readonly DataLoader DataLoader;

        public FakeData(MLContext mlContext = null)
        {
            _mlContext = mlContext ?? new MLContext(0);
            DataLoader = new DataLoader(mlContext, _fakeDataFilePath, _fakeDataDtypesFilePath);
        }

        public IEnumerable<(Person, Computer)> EnumerateData()
        {
            return DataLoader.EnumerateData();
        }
    }
}
