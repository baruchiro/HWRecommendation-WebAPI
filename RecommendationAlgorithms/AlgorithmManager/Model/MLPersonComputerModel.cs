using System.Collections.Generic;
using AlgorithmManager.Interfaces;
using Models;

namespace AlgorithmManager.Model
{
    public class MLPersonComputerModel : IMLModel
    {
        public long PersonId { get; set; }
        public string PersonName { get; set; }
        public string PersonWorkArea { get; set; }
        public int PersonAge { get; set; }
        public int PersonGender { get; set; }
        public string PersonMainUse { get; set; }
        public int PersonPrice { get; set; }
        public long ComputerId { get; set; }
        public long ComputerProcessorId { get; set; }
        public string ComputerProcessorName { get; set; }
        public long? ComputerProcessorGHz { get; set; }
        public int? ComputerProcessorNumOfCores { get; set; }
        public int? ComputerProcessorArchitecture { get; set; }
        public string ComputerProcessorManufacturer { get; set; }
        public ICollection<long> ComputerMemoriesId { get; set; }
        public ICollection<long> ComputerMemoriesCapacity { get; set; }
        public ICollection<int> ComputerMemoriesType { get; set; }
        public ICollection<long> ComputerMemoriesGhz { get; set; }
        public ICollection<string> ComputerMemoriesBankLabel { get; set; }
        public ICollection<string> ComputerMemoriesDeviceLocator { get; set; }
        public ICollection<int> ComputerMemoriesGeneration { get; set; }
        public ICollection<long> ComputerDisksId { get; set; }
        public ICollection<string> ComputerDisksModel { get; set; }
        public ICollection<int?> ComputerDisksType { get; set; }
        public ICollection<int?> ComputerDisksRpm { get; set; }
        public ICollection<long?> ComputerDisksCapacity { get; set; }
        public long ComputerMotherBoardId { get; set; }
        public int? ComputerMotherBoardDdrSockets { get; set; }
        public long? ComputerMotherBoardMaxRam { get; set; }
        public int? ComputerMotherBoardSataConnections { get; set; }
        public int? ComputerMotherBoardArchitecture { get; set; }
        public string ComputerMotherBoardManufacturer { get; set; }
        public string ComputerMotherBoardProduct { get; set; }
        public ICollection<long> ComputerGpusId { get; set; }
        public ICollection<string> ComputerGpusName { get; set; }
        public ICollection<string> ComputerGpusProcessor { get; set; }
        public ICollection<int?> ComputerGpusCores { get; set; }
        public ICollection<string> ComputerGpusManufacturer { get; set; }
        public ICollection<long> ComputerGpusMemoryId { get; set; }
        public ICollection<long> ComputerGpusMemoryCapacity { get; set; }
        public ICollection<int> ComputerGpusMemoryType { get; set; }
        public ICollection<long> ComputerGpusMemoryGhz { get; set; }
        public ICollection<string> ComputerGpusMemoryBankLabel { get; set; }
        public ICollection<string> ComputerGpusMemoryDeviceLocator { get; set; }
        public ICollection<int> ComputerGpusMemoryGeneration { get; set; }
        public ICollection<int> ComputerGpusVersion { get; set; }
        public int ComputerComputerType { get; set; }
    }

    public class PersonComputerStructureModel
    {
        public Person Person { get; set; }
        public Computer Computer { get; set; }
    }
}