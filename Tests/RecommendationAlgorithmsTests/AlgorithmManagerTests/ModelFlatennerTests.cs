using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AlgorithmManager.Extensions;
using AlgorithmManager.Model;
using EnumsNET;
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
        public void Flatten_Gpu_ValidateMemberOfMember()
        {
            var gpu = TestUtils.TestUtils.GenerateGpu();
            var memoryCapacity = gpu.Memory.Capacity;
            var memoryType = gpu.Memory.Type;
            Assert.True(memoryCapacity > 0);
            Assert.NotEqual(default, memoryType);
            
            var flattenGpu = TypeExtensions.CreateFilledFlattenObject<FlattenGpu, Gpu>(TestUtils.TestUtils.GenerateGpu());
            Assert.Equal(memoryCapacity, flattenGpu.MemoryCapacity);
            Assert.Equal(Enums.ToInt32(memoryType), flattenGpu.MemoryType);
        }

        [Fact]
        public void Flatten_PersonComputer_ValidateCollections()
        {
            var personComputer = new PersonComputerStructureModel
            {
                Computer = TestUtils.TestUtils.GenerateComputer(),
                Person = TestUtils.TestUtils.GeneratePerson()
            };

            var diskTypeNullableFirst = personComputer.Computer.Disks.FirstOrDefault()?.Type ??
                                          throw new ArgumentNullException(
                                              $"The test must run with value in personComputer.Computer.Disks");
            var diskCapacityFirst = personComputer.Computer.Disks.FirstOrDefault()?.Capacity ??
                                    throw new ArgumentNullException(
                                        $"The test must run with value in personComputer.Computer.Disks");
            Assert.True(diskCapacityFirst > 0);
            Assert.NotEqual(default, diskCapacityFirst);

            var flattenPersonComputer =
                TypeExtensions.CreateFilledFlattenObject<FlattenPersonComputer, PersonComputerStructureModel>(
                    personComputer);

            Assert.Contains(diskCapacityFirst, flattenPersonComputer.ComputerDisksCapacity);
            Assert.Contains(Enums.ToInt32(diskTypeNullableFirst), flattenPersonComputer.ComputerDisksType);
        }

        private void AssertComparePropertyNamesTypesAgainstFlattenType<TType, TFlatten>()
        {
            var expectedPropsNames = typeof(TType).ResolveRecursiveNamesAndType(true);
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
