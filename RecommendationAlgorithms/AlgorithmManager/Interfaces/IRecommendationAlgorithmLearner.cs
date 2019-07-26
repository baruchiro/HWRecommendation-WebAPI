using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ML;
using Models;

namespace AlgorithmManager.Interfaces
{
    public interface IRecommendationAlgorithmLearner
    {
        IEnumerable<LearningResult> TrainModel(MLContext mlContext, 
            ModelSaver modelSaver,
            IEnumerable<(Person, Computer)> personComputerPairs,
            uint timeoutInMinutes);
        ICollection<LearningResult> TrainModelParallel(MLContext mlContext, 
            ModelSaver modelSaver,
            IEnumerable<(Person, Computer)> personComputerPairs,
            uint timeoutInMinutes,
            CancellationToken cancellationToken);
    }
}
