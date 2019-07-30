using System.Collections.Generic;
using ComputerUpgradeStrategies.DevicesInterfaces;
using ComputerUpgradeStrategies.Recommendations;
using ComputerUpgradeStrategies.Recommendations.Disk;
using Models;

namespace ComputerUpgradeStrategies.Strategies.Simple
{
    class SimpleDiskStrategy : IRecommendationStrategy<IDiskDevice>
    {
        public IEnumerable<IUpgradeRecommendation> GetRecommendations(IDiskDevice disk)
        {
            switch (disk.Type)
            {
                case DiskType.HDD:
                    yield return new DiskReplaceRecommendation(disk, DiskRecommendations.Replace_HDD_SDD);
                    break;
                case DiskType.Unknown:
                case null:
                    yield return new DiskReplaceRecommendation(disk, DiskRecommendations.Replace_Unknown_SDD);
                    break;
                case DiskType.SSD:
                    break;
                default:
                    yield return new DiskReplaceRecommendation(disk, "System does not recognize disk type:" + disk.Type);
                    break;
            }

            // TODO Capacity in bytes?
            if (disk.Capacity < 300)
            {
                yield return new DiskReplaceRecommendation(disk, DiskRecommendations.Get_More_Capacity);
            }
        }
    }
}
