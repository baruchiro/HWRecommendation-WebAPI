using System;
using System.Collections.Generic;
using System.Text;
using ComputerUpgradeStrategies.DevicesInterfaces;
using ComputerUpgradeStrategies.Recommendations;
using ComputerUpgradeStrategies.Recommendations.Memory;
using Models;

namespace ComputerUpgradeStrategies.Strategies.Simple
{
    class SimpleMemoryStrategy : IRecommendationStrategy<IMemoryDevice>
    {
        public IEnumerable<IUpgradeRecommendation> GetRecommendations(IMemoryDevice device)
        {
            if (device.FreeMemorySockets > 0)
            {
                yield return new MemoryAddRecommendation($"You can add more {device.FreeMemorySockets} {device.Type} memories sticks, with {device.Ghz}");
            }
        }
    }
}
