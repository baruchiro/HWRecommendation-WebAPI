using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using AlgorithmManager.Interfaces;
using Microsoft.ML;
using Microsoft.ML.AutoML;
using Microsoft.ML.Data;

namespace RegressionAutoML
{
    public class AutoRegression : IRecommendationAlgorithmLearner
    {
        public LearningResult TrainModel(IDataView dataView, string label, uint minutes)
        {
            if (dataView == null) throw new ArgumentNullException(nameof(dataView));
                var mlContext = new MLContext();

            var cts = new CancellationTokenSource();
            var experimentSettings = new RegressionExperimentSettings
            {
                MaxExperimentTimeInSeconds = minutes * 60,
                CancellationToken = cts.Token,
                OptimizingMetric = RegressionMetric.MeanSquaredError
            };

            var experiment = mlContext.Auto().CreateRegressionExperiment(experimentSettings);

            var experimentResult = experiment.Execute(dataView, label);
            var bestResult = experimentResult.BestRun;
            return new LearningResult
            {
                Result = bestResult.ValidationMetrics.ToString(),
                Schema = dataView.Schema,
                TrainedModel = bestResult.Model
            };
        }
    }
}
