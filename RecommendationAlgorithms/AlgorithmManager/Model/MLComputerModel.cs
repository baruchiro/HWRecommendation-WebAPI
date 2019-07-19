using System.Collections.Generic;
using AlgorithmManager.Interfaces;
using Models;

namespace AlgorithmManager.Model
{
    public class MLComputerModel : IMLModel
    {
        public long Id { get; set; }
        public long ProcessorId { get; set; }
        public string ProcessorName { get; set; }
        public long? ProcessorGHz { get; set; }
        public int? ProcessorNumOfCores { get; set; }
        public int? ProcessorArchitecture { get; set; }
        public string ProcessorManufacturer { get; set; }
        public ICollection<long> MemoriesId { get; set; }
        public ICollection<long> MemoriesCapacity { get; set; }
        public ICollection<int> MemoriesType { get; set; }
        public ICollection<long> MemoriesGhz { get; set; }
        public ICollection<string> MemoriesBankLabel { get; set; }
        public ICollection<string> MemoriesDeviceLocator { get; set; }
        public ICollection<int> MemoriesGeneration { get; set; }
        public ICollection<long> DisksId { get; set; }
        public ICollection<string> DisksModel { get; set; }
        public ICollection<int?> DisksType { get; set; }
        public ICollection<int?> DisksRpm { get; set; }
        public ICollection<long?> DisksCapacity { get; set; }
        public long MotherBoardId { get; set; }
        public int? MotherBoardDdrSockets { get; set; }
        public long? MotherBoardMaxRam { get; set; }
        public int? MotherBoardSataConnections { get; set; }
        public int? MotherBoardArchitecture { get; set; }
        public string MotherBoardManufacturer { get; set; }
        public string MotherBoardProduct { get; set; }
        public ICollection<long> GpusId { get; set; }
        public ICollection<string> GpusName { get; set; }
        public ICollection<string> GpusProcessor { get; set; }
        public ICollection<int?> GpusCores { get; set; }
        public ICollection<string> GpusManufacturer { get; set; }
        public ICollection<long> GpusMemoryId { get; set; }
        public ICollection<long> GpusMemoryCapacity { get; set; }
        public ICollection<int> GpusMemoryType { get; set; }
        public ICollection<long> GpusMemoryGhz { get; set; }
        public ICollection<string> GpusMemoryBankLabel { get; set; }
        public ICollection<string> GpusMemoryDeviceLocator { get; set; }
        public ICollection<int> GpusMemoryGeneration { get; set; }
        public ICollection<int> GpusVersion { get; set; }
        public int ComputerType { get; set; }
    }
}