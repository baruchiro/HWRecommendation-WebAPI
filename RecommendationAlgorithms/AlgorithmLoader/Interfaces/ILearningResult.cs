using Microsoft.ML;

namespace AlgorithmLoader.Interfaces
{
    public class LearningResult
    {
        public string Result;
        public ITransformer TrainedModel;
        public DataViewSchema Schema;
    }
}
