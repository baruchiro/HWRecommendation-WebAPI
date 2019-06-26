using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComputerUpgradeStrategies.Adapters;
using ComputerUpgradeStrategies.DevicesInterfaces;
using ComputerUpgradeStrategies.Recommendations;
using ComputerUpgradeStrategies.Strategies;
using ComputerUpgradeStrategies.Strategies.Simple;
using Models;

namespace ComputerUpgradeStrategies
{
    class UpgradeRecommend
    {
        private readonly ComputerAdapter _computer;
        private IRecommendationStrategy<IDiskDevice> _diskStrategy;
        private IRecommendationStrategy<IMemoryDevice> _memoryStrategy;

        public UpgradeRecommend(Computer computer, IRecommendationStrategy<IDiskDevice> diskStrategy = null, IRecommendationStrategy<IMemoryDevice> memoryStrategy = null)
        {
            _computer = new ComputerAdapter(computer);
            _diskStrategy = diskStrategy?? new SimpleDiskStrategy();
            _memoryStrategy = memoryStrategy ?? new SimpleMemoryStrategy();
        }

        public IEnumerable<IUpgradeRecommendation> DiskUpgrades()
        {
            return _computer.DiskDevices?.SelectMany(d => _diskStrategy.GetRecommendations(d));
        }

        public IEnumerable<IUpgradeRecommendation> MemoryUpgrades()
        {
            return _memoryStrategy.GetRecommendations(_computer);
        }
    }
}
