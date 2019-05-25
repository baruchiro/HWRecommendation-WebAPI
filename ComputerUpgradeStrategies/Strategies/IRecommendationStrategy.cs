using System;
using System.Collections.Generic;
using System.Text;
using ComputerUpgradeStrategies.Recommendations;
using Models;

namespace ComputerUpgradeStrategies.Strategies
{
    interface IRecommendationStrategy<in T>
    {
        IEnumerable<IUpgradeRecommendation> GetRecommendations(T t);
    }
}
