using Microsoft.ML;
using Models;
using System.Collections.Generic;
using System.Threading;

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
