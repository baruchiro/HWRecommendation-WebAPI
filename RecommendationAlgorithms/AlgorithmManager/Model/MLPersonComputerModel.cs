using System.Collections.Generic;
using AlgorithmManager.Interfaces;
using AlgorithmManager.ModelAttributes;
using Microsoft.ML.Data;
using Models;

namespace AlgorithmManager.Model
{
    public class MLPersonComputerModel : IMLModel
    {
        public float PersonId { get; set; }
        public string PersonName { get; set; }
        [Feature] public string PersonWorkArea { get; set; }
        [Feature] public float PersonAge { get; set; }
        [Feature] public float PersonGender { get; set; }
        [Feature] public string PersonMainUse { get; set; }
        [Feature] public float PersonPrice { get; set; }
        public float ComputerId { get; set; }
        public float ComputerProcessorId { get; set; }
        public string ComputerProcessorName { get; set; }
        [RegressionLabel] public float ComputerProcessorGHz { get; set; }
        [RegressionLabel] public float ComputerProcessorNumOfCores { get; set; }
        [ClassificationLabel] public float ComputerProcessorArchitecture { get; set; }
        [ClassificationLabel] public string ComputerProcessorManufacturer { get; set; }
        public float[] ComputerMemoriesId { get; set; }
        [Sum, RegressionLabel] public float ComputerMemoriesCapacity { get; set; }
        [AllMustBeSame, ClassificationLabel] public float ComputerMemoriesType { get; set; }
        [Min, RegressionLabel] public float ComputerMemoriesGhz { get; set; }
        public string[] ComputerMemoriesBankLabel { get; set; }
        public string[] ComputerMemoriesDeviceLocator { get; set; }
        [Min, RegressionLabel] public float ComputerMemoriesGeneration { get; set; }
        public float[] ComputerDisksId { get; set; }
        public string[] ComputerDisksModel { get; set; }
        public float[] ComputerDisksType { get; set; }
        [Min, RegressionLabel] public float ComputerDisksRpm { get; set; }
        [Sum, RegressionLabel] public float ComputerDisksCapacity { get; set; }
        public float ComputerMotherBoardId { get; set; }
        [RegressionLabel] public float ComputerMotherBoardDdrSockets { get; set; }
        [RegressionLabel] public float ComputerMotherBoardMaxRam { get; set; }
        [RegressionLabel] public float ComputerMotherBoardSataConnections { get; set; }
        [ClassificationLabel] public float ComputerMotherBoardArchitecture { get; set; }
        public string ComputerMotherBoardManufacturer { get; set; }
        public string ComputerMotherBoardProduct { get; set; }
        public float[] ComputerGpusId { get; set; }
        public string[] ComputerGpusName { get; set; }
        public string[] ComputerGpusProcessor { get; set; }
        public float[] ComputerGpusCores { get; set; }
        public string[] ComputerGpusManufacturer { get; set; }
        public float[] ComputerGpusMemoryId { get; set; }
        public float[] ComputerGpusMemoryCapacity { get; set; }
        public float[] ComputerGpusMemoryType { get; set; }
        public float[] ComputerGpusMemoryGhz { get; set; }
        public string[] ComputerGpusMemoryBankLabel { get; set; }
        public string[] ComputerGpusMemoryDeviceLocator { get; set; }
        public float[] ComputerGpusMemoryGeneration { get; set; }
        public float[] ComputerGpusVersion { get; set; }
        [Feature] public float ComputerComputerType { get; set; }
    }

    public class PersonComputerStructureModel
    {
        public Person Person { get; set; }
        public Computer Computer { get; set; }
    }
}