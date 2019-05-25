using System;
using System.Collections.Generic;
using System.Text;
using Models;

namespace ComputerUpgradeStrategies.DevicesInterfaces
{
    interface IDiskDevice
    {
        DiskType? Type { get; }
        long? Capacity { get; }
    }
}
