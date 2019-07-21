using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using AlgorithmManager;
using AlgorithmManager.Extensions;
using AlgorithmManager.Model;
using AlgoTestUtils;
using EnumsNET;
using Xunit;
using Microsoft.ML;
using Models;

namespace AlgorithmManagerIntegration
{
    public class ConvertingTests
    {
        private readonly string _fakeDataFilePath = Path.Combine("Resources", "fake-data-out.csv");
        private readonly string _fakeDataDtypesFilePath = Path.Combine("Resources", "fake-data-out.dtypes.csv");
        private MLContext _mlContext;

        public ConvertingTests()
        {
            _mlContext = new MLContext(0);
        }

        [Fact]
        public void MLModelConverter_ConvertAllFakedData()
        {
            var dataLoader = new DataLoader(_mlContext, _fakeDataFilePath, _fakeDataDtypesFilePath);
            var expected = dataLoader.EnumerateData()
                .Select(t => new PersonComputerStructureModel
            {
                Computer = t.Item2,
                Person = t.Item1

            }).ToList();

            var actual = expected
                .Select(TypeExtensions.CreateFilledMLObject<MLPersonComputerModel, PersonComputerStructureModel>);

            var zip = expected.Zip(actual, (model, computerModel) => (model, computerModel));
            foreach (var (model, computerModel) in zip)
            {
                computerModel.AssertEqual(model);
            }
        }
    }
}
