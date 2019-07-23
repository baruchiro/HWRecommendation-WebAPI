using System;
using Microsoft.ML;
using Microsoft.ML.AutoML;

namespace AlgorithmManager.Interfaces
{
    public class LearningResult
    {
        public static LearningResult CreateFromRunDetail(RunDetail runDetail, DataViewSchema schema, double result)
        {
            var learningResult = new LearningResult
            {
                Estimator = runDetail.Estimator,
                RuntimeInSeconds= runDetail.RuntimeInSeconds,
                TrainerName= runDetail.TrainerName,
                Result = result,
                Schema = schema
            };
            return learningResult;
        }

        public static LearningResult CreateFromRunDetail<T>(RunDetail<T> runDetail,
            DataViewSchema schema, double result)
        {
            var learningResult = new LearningResult
            {
                Estimator = runDetail.Estimator,
                RuntimeInSeconds = runDetail.RuntimeInSeconds,
                TrainerName = runDetail.TrainerName,
                ValidationMetrics = runDetail.ValidationMetrics,
                Result = result,
                Exception = runDetail.Exception,
                Model = runDetail.Model,
                Schema = schema
            };
            return learningResult;
        }

        public double Result {  private set; get; }

        public ITransformer Model {  private set; get; }

        public Exception Exception {  private set; get; }

        public dynamic ValidationMetrics {  private set; get; }

        public string TrainerName {  private set; get; }

        public double RuntimeInSeconds {  private set; get; }

        public IEstimator<ITransformer> Estimator {  private set; get; }
        public DataViewSchema Schema {  private set; get; }
    }
}
