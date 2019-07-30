using Models;

namespace ComputerUpgradeStrategies.DevicesInterfaces
{
    interface IDiskDevice
    {
        DiskType? Type { get; }
        long? Capacity { get; }
    }
}
