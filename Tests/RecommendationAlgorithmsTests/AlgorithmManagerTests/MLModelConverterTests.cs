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
    public class MLModelConverterTests
    {
        [Fact]
        public void MLModels_PersonComputer_ValidateNamesAndTypes()
        {
            AssertComparePropertyNamesTypesAgainstMLType<PersonComputerStructureModel, MLPersonComputerModel>();
        }

        [Fact]
        public void MLModels_Computer_ValidateNamesAndTypes()
        {
            AssertComparePropertyNamesTypesAgainstMLType<Computer, MLComputerModel>();
        }

        [Fact]
        public void MLModels_Gpu_ValidateNamesAndTypes()
        {
           AssertComparePropertyNamesTypesAgainstMLType<Gpu, MLGpuModel>();
        }

        [Fact]
        public void MLModelConverter_Gpu_ValidateMemberOfMember()
        {
            var gpu = TestUtils.TestUtils.GenerateGpu();
            var memoryCapacity = gpu.Memory.Capacity;
            var memoryType = gpu.Memory.Type;
            Assert.True(memoryCapacity > 0);
            Assert.NotEqual(default, memoryType);
            
            var gpuMLModel = TypeExtensions.CreateFilledMLObject<MLGpuModel, Gpu>(TestUtils.TestUtils.GenerateGpu());
            Assert.Equal(memoryCapacity, gpuMLModel.MemoryCapacity);
            Assert.Equal(Enums.ToInt32(memoryType), gpuMLModel.MemoryType);
        }

        [Fact]
        public void MLModelConverter_PersonComputer_ValidateCollections()
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

            var personComputerMLModel =
                TypeExtensions.CreateFilledMLObject<MLPersonComputerModel, PersonComputerStructureModel>(
                    personComputer);

            Assert.Contains(diskCapacityFirst, personComputerMLModel.ComputerDisksCapacity);
            Assert.Contains(Enums.ToInt32(diskTypeNullableFirst), personComputerMLModel.ComputerDisksType);
        }

        private void AssertComparePropertyNamesTypesAgainstMLType<TType, TMLModel>()
        {
            var expectedPropsNames = typeof(TType).ResolveRecursiveNamesAndType(true);
            var actualPropsNames = typeof(TMLModel).GetInstanceOrPublicProperties()
                .ToDictionary(p => p.Name, p => p.PropertyType);

            var expectedButNotInActual = expectedPropsNames.Except(actualPropsNames).ToList();
            var actualButNotInExpected = actualPropsNames.Except(expectedPropsNames);

            var toWrite = string.Join('\n', expectedButNotInActual.Select(s => $"public {s.Value} {s.Key} {{get; set;}}"));
            Assert.Empty(expectedButNotInActual);
            Assert.Empty(actualButNotInExpected);
        }
    }
}
