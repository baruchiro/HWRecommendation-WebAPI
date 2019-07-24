using System;
using System.Linq;
using AlgorithmManager.Model;
using EnumsNET;
using Models;
using Xunit;

namespace AlgoTestUtils
{
    public static class MLModelConverterAssertionExtensions
    {
        public static void AssertEqual(this MLGpuModel mlGpuModel, Gpu gpu)
        {
            if (mlGpuModel == null) throw new ArgumentNullException(nameof(mlGpuModel));
            if (gpu == null) throw new ArgumentNullException(nameof(gpu));

            Assert.Equal(gpu.Memory.Capacity, mlGpuModel.MemoryCapacity);
            Assert.Equal(Enums.ToInt32(gpu.Memory.Type), mlGpuModel.MemoryType);
        }

        public static void AssertNotDefault(this Gpu gpu)
        {
            if (gpu == null) throw new ArgumentNullException(nameof(gpu));

            Assert.True(gpu.Memory.Capacity > 0);
            Assert.NotEqual(default, gpu.Memory.Type);
        }

        public static void AssertEqual(this MLPersonComputerModel mlPersonComputerModel,
            PersonComputerStructureModel personComputerStructureModel)
        {
            if (mlPersonComputerModel == null) throw new ArgumentNullException(nameof(mlPersonComputerModel));
            if (personComputerStructureModel == null)
                throw new ArgumentNullException(nameof(personComputerStructureModel));

            if (personComputerStructureModel.Computer.Disks.Count == 0)
            {
                Assert.Empty(mlPersonComputerModel.ComputerDisksModel);
                Assert.Empty(mlPersonComputerModel.ComputerDisksType);
            }
            else
            {
                Assert.NotEmpty(mlPersonComputerModel.ComputerDisksModel);
                Assert.NotEmpty(mlPersonComputerModel.ComputerDisksType);

                var diskTypeNullableFirst = personComputerStructureModel.Computer.Disks.First().Type?? default;
                var diskModelFirst = personComputerStructureModel.Computer.Disks.First().Model ?? default;


                Assert.Contains(diskModelFirst, mlPersonComputerModel.ComputerDisksModel);
                Assert.Contains(Enums.ToInt32(diskTypeNullableFirst), mlPersonComputerModel.ComputerDisksType);
            }
        }

        public static void AssertNotDefault(this PersonComputerStructureModel personComputerStructureModel)
        {
            if (personComputerStructureModel == null)
                throw new ArgumentNullException(nameof(personComputerStructureModel));

            var diskTypeNullableFirst = personComputerStructureModel.Computer.Disks.FirstOrDefault()?.Type ??
                                        throw new ArgumentNullException(
                                            $"The test must run with value in personComputer.Computer.Disks");
            var diskCapacityFirst = personComputerStructureModel.Computer.Disks.FirstOrDefault()?.Capacity ??
                                    throw new ArgumentNullException(
                                        $"The test must run with value in personComputer.Computer.Disks");
            Assert.True(diskCapacityFirst > 0);
            Assert.NotEqual(default, diskCapacityFirst);
        }
    }
}
