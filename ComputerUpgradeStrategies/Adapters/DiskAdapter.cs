using ComputerUpgradeStrategies.DevicesInterfaces;
using Models;

namespace ComputerUpgradeStrategies.Adapters
{
    class DiskAdapter : Disk, IDiskDevice
    {
        public DiskAdapter(Disk disk) : base(disk)
        {
        } }
}
