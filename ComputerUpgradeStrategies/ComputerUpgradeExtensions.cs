using Models;
using System;
using System.Collections.Generic;
using ComputerUpgradeStrategies.Recommendations;

namespace ComputerUpgradeStrategies
{
    public static class ComputerUpgradeExtensions
    {
        public static IEnumerable<IUpgradeRecommendation> GetUpgradeRecommendations(this Computer computer)
        {
            var upgradeRecommend = new UpgradeRecommend(computer);

            foreach (var recommend in upgradeRecommend.DiskUpgrades())
            {
                yield return recommend;
            }

            foreach (var recommend in upgradeRecommend.MemoryUpgrades())
            {
                yield return recommend;
            }
        }
    }
}
