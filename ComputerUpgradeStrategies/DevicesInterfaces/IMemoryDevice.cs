using Models;

namespace ComputerUpgradeStrategies.DevicesInterfaces
{
    interface IMemoryDevice
    {
        int FreeMemorySockets { get; }
        RamType Type { get; }
        long Ghz { get; }
    }
}
