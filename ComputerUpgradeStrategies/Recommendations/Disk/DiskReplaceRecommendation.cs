using Models;

namespace ComputerUpgradeStrategies.Recommendations.Disk
{
    class DiskReplaceRecommendation : IUpgradeRecommendation
    {
        private Models.Disk _source;
        private Models.Disk _replace;
        private readonly string _explanation;

        public DiskReplaceRecommendation(Models.Disk disk, Models.Disk replace, string explanation)
        {
            _source = disk;
            _replace = replace;
            _explanation = explanation;
        }

        public override string ToString()
        {
            return _explanation;
        }
    }
}
