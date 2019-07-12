using Microsoft.ML;

namespace AlgorithmManager.Interfaces
{
    public class LearningResult
    {
        public string Result { get; set; }
        public ITransformer TrainedModel { get; set; }
        public DataViewSchema Schema { get; set; }
    }
}
