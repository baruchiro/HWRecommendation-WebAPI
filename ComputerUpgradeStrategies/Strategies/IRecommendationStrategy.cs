using System.Collections.Generic;
using ComputerUpgradeStrategies.Recommendations;

namespace ComputerUpgradeStrategies.Strategies
{
    interface IRecommendationStrategy<in T>
    {
        IEnumerable<IUpgradeRecommendation> GetRecommendations(T t);
    }
}
