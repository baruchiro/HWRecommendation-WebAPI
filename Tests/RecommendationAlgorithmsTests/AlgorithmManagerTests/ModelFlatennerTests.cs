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
        public void FlattenerClasses_PersonComputer_ValidateNamesAndTypes()
        {
            AssertComparePropertyNamesTypesAgainstFlattenType<PersonComputerStructureModel, FlattenPersonComputer>();
        }

        [Fact]
        public void FlattenerClasses_Computer_ValidateNamesAndTypes()
        {
            AssertComparePropertyNamesTypesAgainstFlattenType<Computer, FlattenComputer>();
        }

        [Fact]
        public void FlattenerClasses_Gpu_ValidateNamesAndTypes()
        {
           AssertComparePropertyNamesTypesAgainstFlattenType<Gpu, FlattenGpu>();
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
