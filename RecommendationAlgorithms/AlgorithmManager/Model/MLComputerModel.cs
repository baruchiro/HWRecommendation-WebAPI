using System.Collections.Generic;
using AlgorithmManager.Interfaces;
using Microsoft.ML.Data;
using Models;

namespace AlgorithmManager.Model
{
    public class MLComputerModel : IMLModel
    {
        public long Id { get; set; }
        public long ProcessorId { get; set; }
        public string ProcessorName { get; set; }
        public long ProcessorGHz { get; set; }
        public int ProcessorNumOfCores { get; set; }
        public int ProcessorArchitecture { get; set; }
        public string ProcessorManufacturer { get; set; }
        public long[] MemoriesId { get; set; }
        public long[] MemoriesCapacity { get; set; }
        public int[] MemoriesType { get; set; }
        public long[] MemoriesGhz { get; set; }
        public string[] MemoriesBankLabel { get; set; }
        public string[] MemoriesDeviceLocator { get; set; }
        public int[] MemoriesGeneration { get; set; }
        public long[] DisksId { get; set; }
        public string[] DisksModel { get; set; }
        public int[] DisksType { get; set; }
        public int[] DisksRpm { get; set; }
        public long[] DisksCapacity { get; set; }
        public long MotherBoardId { get; set; }
        public int MotherBoardDdrSockets { get; set; }
        public long MotherBoardMaxRam { get; set; }
        public int MotherBoardSataConnections { get; set; }
        public int MotherBoardArchitecture { get; set; }
        public string MotherBoardManufacturer { get; set; }
        public string MotherBoardProduct { get; set; }
        public long[] GpusId { get; set; }
        public string[] GpusName { get; set; }
        public string[] GpusProcessor { get; set; }
        public int[] GpusCores { get; set; }
        public string[] GpusManufacturer { get; set; }
        public long[] GpusMemoryId { get; set; }
        public long[] GpusMemoryCapacity { get; set; }
        public int[] GpusMemoryType { get; set; }
        public long[] GpusMemoryGhz { get; set; }
        public string[] GpusMemoryBankLabel { get; set; }
        public string[] GpusMemoryDeviceLocator { get; set; }
        public int[] GpusMemoryGeneration { get; set; }
        public int[] GpusVersion { get; set; }
        public int ComputerType { get; set; }
    }
}