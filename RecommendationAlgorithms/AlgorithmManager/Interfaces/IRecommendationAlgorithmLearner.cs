using System;
using System.Collections.Generic;
using Microsoft.ML;
using Models;

namespace AlgorithmManager.Interfaces
{
    public interface IRecommendationAlgorithmLearner
    {
        LearningResult TrainModel(IEnumerable<Tuple<Person, Computer>> personComputerPairs, string label, uint timeoutInMinutes);
    }
}
