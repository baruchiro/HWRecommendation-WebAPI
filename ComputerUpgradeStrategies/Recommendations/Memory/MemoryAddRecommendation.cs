using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerUpgradeStrategies.Recommendations.Memory
{
    class MemoryAddRecommendation : IUpgradeRecommendation
    {
        public MemoryAddRecommendation(string explanation)
        {
            Explanation = explanation;
        }

        public string Explanation { get; }
    }
}
