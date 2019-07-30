using System.Linq;
using AlgoTestUtils;
using Models;
using Xunit;

namespace AlgorithmManagerTests
{
    public class DataLoaderTests
    {
        private readonly FakeData _fakeData;

        public DataLoaderTests()
        {
            _fakeData = new FakeData();
        }

        [Fact]
        public void LoadCsv_CreatePersonAndComputer()
        {
            var firstObject = _fakeData.DataLoader.EnumerateData().First();

            AssertTuple(firstObject);
        }

        [Fact]
        public void LoadCsv_ValidateAllCsvFile()
        {
            var all = _fakeData.DataLoader.EnumerateData();

            foreach (var tuple in all)
            {
                AssertTuple(tuple);
            }
        }

        private void AssertTuple((Person, Computer) tuple)
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
