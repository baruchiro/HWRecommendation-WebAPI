using AlgorithmManager.Interfaces;

namespace AlgorithmManager.Model
{
    public class MLComputerModel : IMLModel
    {
        public float Id { get; set; }
        public float ProcessorId { get; set; }
        public string ProcessorName { get; set; }
        public float ProcessorGHz { get; set; }
        public float ProcessorNumOfCores { get; set; }
        public float ProcessorArchitecture { get; set; }
        public string ProcessorManufacturer { get; set; }
        public float[] MemoriesId { get; set; }
        public float[] MemoriesCapacity { get; set; }
        public float[] MemoriesType { get; set; }
        public float[] MemoriesGhz { get; set; }
        public string[] MemoriesBankLabel { get; set; }
        public string[] MemoriesDeviceLocator { get; set; }
        public float[] MemoriesGeneration { get; set; }
        public float[] DisksId { get; set; }
        public string[] DisksModel { get; set; }
        public float[] DisksType { get; set; }
        public float[] DisksRpm { get; set; }
        public float[] DisksCapacity { get; set; }
        public float MotherBoardId { get; set; }
        public float MotherBoardDdrSockets { get; set; }
        public float MotherBoardMaxRam { get; set; }
        public float MotherBoardSataConnections { get; set; }
        public float MotherBoardArchitecture { get; set; }
        public string MotherBoardManufacturer { get; set; }
        public string MotherBoardProduct { get; set; }
        public float[] GpusId { get; set; }
        public string[] GpusName { get; set; }
        public string[] GpusProcessor { get; set; }
        public float[] GpusCores { get; set; }
        public string[] GpusManufacturer { get; set; }
        public float[] GpusMemoryId { get; set; }
        public float[] GpusMemoryCapacity { get; set; }
        public float[] GpusMemoryType { get; set; }
        public float[] GpusMemoryGhz { get; set; }
        public string[] GpusMemoryBankLabel { get; set; }
        public string[] GpusMemoryDeviceLocator { get; set; }
        public float[] GpusMemoryGeneration { get; set; }
        public float[] GpusVersion { get; set; }
        public float ComputerType { get; set; }
    }
}