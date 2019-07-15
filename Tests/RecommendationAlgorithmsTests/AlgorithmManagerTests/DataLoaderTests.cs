using System;
using System.IO;
using System.Linq;
using AlgorithmManager;
using Microsoft.ML;
using Models;
using Xunit;

namespace AlgorithmManagerTests
{
    public class DataLoaderTests
    {
        private readonly string _fakeDataFilePath = Path.Combine("Resources", "fake-data-out.csv");
        private readonly string _fakeDataDtypesFilePath = Path.Combine("Resources", "fake-data-out.dtypes.csv");

        [Fact]
        public void LoadCsv_CreatePersonAndComputer()
        {
            var mlContext = new MLContext();
            var dataLoader = new DataLoader(mlContext, _fakeDataFilePath, _fakeDataDtypesFilePath);
            var firstObject = dataLoader.EnumerateData().First();

            AssertTuple(firstObject);
        }

        [Fact]
        public void LoadCsv_ValidateAllCsvFile()
        {
            var mlContext = new MLContext();
            var dataLoader = new DataLoader(mlContext, _fakeDataFilePath, _fakeDataDtypesFilePath);
            var all = dataLoader.EnumerateData();

            foreach (var tuple in all)
            {
                AssertTuple(tuple);
            }
        }

        private void AssertTuple(Tuple<Person, Computer> tuple)
        {
            var (person, computer) = tuple;
            Assert.NotNull(person);
            Assert.NotNull(computer);
            Assert.NotNull(computer.Disks.FirstOrDefault());
            Assert.NotNull(computer.Gpus.FirstOrDefault());
            Assert.NotNull(computer.Gpus.FirstOrDefault()?.Memory);
            Assert.NotNull(computer.Memories.FirstOrDefault());
        }
    }
}
