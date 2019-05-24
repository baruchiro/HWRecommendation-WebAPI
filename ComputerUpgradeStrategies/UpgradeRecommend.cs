using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComputerUpgradeStrategies.Recommendations;
using ComputerUpgradeStrategies.Strategies;
using ComputerUpgradeStrategies.Strategies.Simple;
using Models;

namespace ComputerUpgradeStrategies
{
    class UpgradeRecommend
    {
        private readonly Computer _computer;
        private IDiskStrategy _diskStrategy;

        public UpgradeRecommend(Computer computer, IDiskStrategy diskStrategy = null)
        {
            _computer = computer;
            _diskStrategy = diskStrategy?? new SimpleDiskStrategy();
        }

        public IEnumerable<IUpgradeRecommendation> DiskUpgrades()
        {
            return _computer.Disks?.SelectMany(d => _diskStrategy.GetRecommendations(d));
        }
    }
}
