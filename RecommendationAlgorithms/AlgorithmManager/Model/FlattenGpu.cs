using Models;

namespace AlgorithmManager.Model
{
    public class FlattenGpu
    {
        public long Id { get; set; }
        public string Manufacturer { get; set; }
        public int? Cores { get; set; }
        public string Name { get; set; }
        public string Processor { get; set; }
        public int Version { get; set; }
        public long MemoryId { get; set; }
        public RamType MemoryType { get; set; }
        public string MemoryBankLabel { get; set; }
        public long MemoryCapacity { get; set; }
        public string MemoryDeviceLocator { get; set; }
        public int MemoryGeneration { get; set; }
        public long MemoryGhz { get; set; }
    }
}