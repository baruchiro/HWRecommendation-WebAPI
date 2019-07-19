using System;
using System.Collections.Generic;
using Microsoft.ML;
using Models;

namespace AlgorithmManager.Interfaces
{
    public interface IRecommendationAlgorithmLearner
    {
        LearningResult TrainModel(MLContext mlContext, IEnumerable<(Person, Computer)> personComputerPairs, string label, uint timeoutInMinutes);
    }
}
