using System;
using System.Collections.Generic;
using ComputerUpgradeStrategies.Extensions;
using ComputerUpgradeStrategies.Recommendations;
using ComputerUpgradeStrategies.Recommendations.Disk;
using Models;

namespace ComputerUpgradeStrategies.Strategies.Simple
{
    class SimpleDiskStrategy : IDiskStrategy
    {
        public IEnumerable<IUpgradeRecommendation> GetRecommendations(Disk disk)
        {
            if (disk.Type == DiskType.HDD)
            {
                yield return new DiskReplaceRecommendation(disk, disk.Replace(d => d.Type = DiskType.SSD),
                    DiskRecommendations.Replace_HDD_SDD);
            }

            if (disk.Type == DiskType.Unknown)
            {
                yield return new DiskReplaceRecommendation(disk, disk.Replace(d => d.Type = DiskType.SSD),
                    DiskRecommendations.Replace_Unknown_SDD);
            }

            // TODO Capacity in bytes?
            if (disk.Capacity < 300)
            {
                yield return new DiskReplaceRecommendation(disk, disk.Replace(d=>d.Capacity = 500), DiskRecommendations.Get_More_Capacity);
            }
        }
    }
}
