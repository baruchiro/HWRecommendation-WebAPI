using ComputerUpgradeStrategies.DevicesInterfaces;
using Models;

namespace ComputerUpgradeStrategies.Recommendations.Disk
{
    class DiskReplaceRecommendation : IUpgradeRecommendation
    {
        private IDiskDevice _source;

        public DiskReplaceRecommendation(IDiskDevice disk, string explanation)
        {
            _source = disk;
            Explanation = explanation;
        }

        public override string ToString()
        {
            return Explanation;
        }

        public string Explanation { get; }
    }
}
