using Microsoft.ML;

namespace AlgorithmManager.Interfaces
{
    public interface IRecommendationAlgorithmLearner
    {
        LearningResult TrainModel(IDataView dataView, string label, uint timeoutInMinutes);
    }
}
