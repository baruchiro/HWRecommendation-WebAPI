using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AlgorithmManager.Extensions;
using AlgorithmManager.Model;
using Models;
using Xunit;
using TypeExtensions = AlgorithmManager.Extensions.TypeExtensions;

namespace AlgorithmManagerTests
{
    public class ModelFlatennerTests
    {
        [Fact]
        public void FlattenerModels_PersonComputer_ValidateNamesAndTypes()
        {
            AssertComparePropertyNamesTypesAgainstFlattenType<PersonComputerStructureModel, FlattenPersonComputer>();
        }

        [Fact]
        public void FlattenerModels_Computer_ValidateNamesAndTypes()
        {
            AssertComparePropertyNamesTypesAgainstFlattenType<Computer, FlattenComputer>();
        }

        [Fact]
        public void FlattenerModels_Gpu_ValidateNamesAndTypes()
        {
           AssertComparePropertyNamesTypesAgainstFlattenType<Gpu, FlattenGpu>();
        }

        [Fact]
        public void Flatten_Gpu_ShouldPass()
        {
            var gpu = TypeExtensions.CreateFilledFlattenObject<FlattenGpu, Gpu>(TestUtils.TestUtils.GenerateGpu());
        }

        [Fact]
        public void Flatten_PersonComputer_ShouldPass()
        {
            var personComputer = new PersonComputerStructureModel
            {
                Computer = TestUtils.TestUtils.GenerateComputer(),
                Person = TestUtils.TestUtils.GeneratePerson()
            };
            var flattenPersonComputer = TypeExtensions.CreateFilledFlattenObject<FlattenPersonComputer, PersonComputerStructureModel>(personComputer);
        }

        private void AssertComparePropertyNamesTypesAgainstFlattenType<TType, TFlatten>()
        {
            var expectedPropsNames = typeof(TType).ResolveRecursiveNamesAndType();
            var actualPropsNames = typeof(TFlatten).GetInstanceOrPublicProperties()
                .ToDictionary(p => p.Name, p => p.PropertyType);

            var expectedButNotInActual = expectedPropsNames.Except(actualPropsNames).ToList();
            var actualButNotInExpected = actualPropsNames.Except(expectedPropsNames);

            var toWrite = string.Join('\n', expectedButNotInActual.Select(s => $"public {s.Value} {s.Key} {{get; set;}}"));
            Assert.Empty(expectedButNotInActual);
            Assert.Empty(actualButNotInExpected);
        }
    }
}
