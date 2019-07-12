using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using AlgorithmLoader.Interfaces;
using Microsoft.ML;
using Microsoft.ML.AutoML;
using Microsoft.ML.Data;

namespace RegressionAutoML
{
    public class RegressionAutoML : IRecommendationAlgorithmLearner
    {
        public LearningResult TrainModel(IDataView data, string label, uint minutes)
        {
            var mlContext = new MLContext();

            var cts = new CancellationTokenSource();
            var experimentSettings = new RegressionExperimentSettings
            {
                MaxExperimentTimeInSeconds = minutes * 60,
                CancellationToken = cts.Token,
                OptimizingMetric = RegressionMetric.MeanSquaredError
            };

            var experiment = mlContext.Auto().CreateRegressionExperiment(experimentSettings);

            var experimentResult = experiment.Execute(data, label);
            var bestResult = experimentResult.BestRun;
            return new LearningResult
            {
                Result = bestResult.ValidationMetrics.ToString(),
                Schema = data.Schema,
                TrainedModel = bestResult.Model
            };
        }
    }
}
