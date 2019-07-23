using System.Collections.Generic;
using AlgorithmManager.Interfaces;
using AlgorithmManager.ModelAttributes;
using Microsoft.ML.Data;
using Models;

namespace AlgorithmManager.Model
{
    public class MLPersonComputerModel : IMLModel
    {
        public long PersonId { get; set; }
        public string PersonName { get; set; }
        [Feature] public string PersonWorkArea { get; set; }
        [Feature] public int PersonAge { get; set; }
        [Feature] public int PersonGender { get; set; }
        [Feature] public string PersonMainUse { get; set; }
        [Feature] public int PersonPrice { get; set; }
        public long ComputerId { get; set; }
        public long ComputerProcessorId { get; set; }
        public string ComputerProcessorName { get; set; }
        [RegressionLabel] public long ComputerProcessorGHz { get; set; }
        [RegressionLabel] public int ComputerProcessorNumOfCores { get; set; }
        [ClassificationLabel] public int ComputerProcessorArchitecture { get; set; }
        [ClassificationLabel] public string ComputerProcessorManufacturer { get; set; }
        public long[] ComputerMemoriesId { get; set; }
        [Sum, RegressionLabel] public long ComputerMemoriesCapacity { get; set; }
        [AllMustBeSame, ClassificationLabel] public int ComputerMemoriesType { get; set; }
        [Min, RegressionLabel] public long ComputerMemoriesGhz { get; set; }
        public string[] ComputerMemoriesBankLabel { get; set; }
        public string[] ComputerMemoriesDeviceLocator { get; set; }
        [Min, RegressionLabel] public int ComputerMemoriesGeneration { get; set; }
        public long[] ComputerDisksId { get; set; }
        public string[] ComputerDisksModel { get; set; }
        public int[] ComputerDisksType { get; set; }
        [Min, RegressionLabel] public int ComputerDisksRpm { get; set; }
        [Sum, RegressionLabel] public long ComputerDisksCapacity { get; set; }
        public long ComputerMotherBoardId { get; set; }
        [RegressionLabel] public int ComputerMotherBoardDdrSockets { get; set; }
        [RegressionLabel] public long ComputerMotherBoardMaxRam { get; set; }
        [RegressionLabel] public int ComputerMotherBoardSataConnections { get; set; }
        [ClassificationLabel] public int ComputerMotherBoardArchitecture { get; set; }
        public string ComputerMotherBoardManufacturer { get; set; }
        public string ComputerMotherBoardProduct { get; set; }
        public long[] ComputerGpusId { get; set; }
        public string[] ComputerGpusName { get; set; }
        public string[] ComputerGpusProcessor { get; set; }
        public int[] ComputerGpusCores { get; set; }
        public string[] ComputerGpusManufacturer { get; set; }
        public long[] ComputerGpusMemoryId { get; set; }
        public long[] ComputerGpusMemoryCapacity { get; set; }
        public int[] ComputerGpusMemoryType { get; set; }
        public long[] ComputerGpusMemoryGhz { get; set; }
        public string[] ComputerGpusMemoryBankLabel { get; set; }
        public string[] ComputerGpusMemoryDeviceLocator { get; set; }
        public int[] ComputerGpusMemoryGeneration { get; set; }
        public int[] ComputerGpusVersion { get; set; }
        [Feature] public int ComputerComputerType { get; set; }
    }

    public class PersonComputerStructureModel
    {
        public Person Person { get; set; }
        public Computer Computer { get; set; }
    }
}