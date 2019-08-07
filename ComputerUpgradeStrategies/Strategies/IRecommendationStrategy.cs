using ComputerUpgradeStrategies.Recommendations;
using System.Collections.Generic;

namespace ComputerUpgradeStrategies.Strategies
{
    interface IRecommendationStrategy<in T>
    {
        IEnumerable<IUpgradeRecommendation> GetRecommendations(T t);
    }
}
