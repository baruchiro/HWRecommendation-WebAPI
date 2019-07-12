using Microsoft.ML;

namespace AlgorithmLoader.Interfaces
{
    public interface IRecommendationAlgorithmLearner
    {
        LearningResult TrainModel(IDataView data, string label, uint timeoutInMinutes);
    }
}
