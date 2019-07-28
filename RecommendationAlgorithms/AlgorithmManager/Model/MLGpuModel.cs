using AlgorithmManager.Interfaces;
using Models;

namespace AlgorithmManager.Model
{
    public class MLGpuModel : IMLModel
    {
        public float Id { get; set; }
        public string Manufacturer { get; set; }
        public float Cores { get; set; }
        public string Name { get; set; }
        public string Processor { get; set; }
        public float Version { get; set; }
        public float MemoryId { get; set; }
        public float MemoryType { get; set; }
        public string MemoryBankLabel { get; set; }
        public float MemoryCapacity { get; set; }
        public string MemoryDeviceLocator { get; set; }
        public float MemoryGeneration { get; set; }
        public float MemoryGhz { get; set; }
    }
}