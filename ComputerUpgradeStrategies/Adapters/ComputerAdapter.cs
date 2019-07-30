using System.Collections.Generic;
using System.Linq;
using ComputerUpgradeStrategies.DevicesInterfaces;
using Models;

namespace ComputerUpgradeStrategies.Adapters
{
    class ComputerAdapter : Computer, IMemoryDevice
    {
        public ComputerAdapter(Computer computer) : base(computer)
        {
        }

        public IEnumerable<IDiskDevice> DiskDevices => Disks?.Select(d => new DiskAdapter(d));

        public int FreeMemorySockets =>
            MotherBoard.DdrSockets != null ? MotherBoard.DdrSockets.Value - Memories.Count : 0;

        public RamType Type => Memories?.FirstOrDefault()?.Type ?? RamType.Unknown;
        public long Ghz => Memories?.Min(m => m.Ghz) ?? 0;
    }
}
