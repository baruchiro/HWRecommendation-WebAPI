using System.Collections.Generic;
using System.Threading;
using AlgorithmManager;
using AlgorithmManager.Extensions;
using AlgorithmManager.Factories;
using AlgorithmManager.Interfaces;
using AlgorithmManager.Model;
using Microsoft.ML;
using Microsoft.ML.AutoML;
using Models;

namespace AutoML
{
    public class AutoRegression : IRecommendationAlgorithmLearner
    {
        public LearningResult TrainModel(MLContext mlContext, IEnumerable<(Person, Computer)> personComputerPairs, string label, uint timeoutInMinutes)
        {
            var factory = new AlgorithmManagerFactory(mlContext);
            var dataView = factory.CreateDataView(personComputerPairs);

            dataView = new PipelineBuilder(mlContext, dataView.Schema)
                .ConvertNumberToSingle()
                .SelectColumns(TypeExtensions.GetFeatureColumns<MLPersonComputerModel>().ToArray())
                .SelectColumns(label)
                .TransformData(dataView);

            var cts = new CancellationTokenSource();
            var experimentSettings = new RegressionExperimentSettings
            {
                MaxExperimentTimeInSeconds = timeoutInMinutes * 60,
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
