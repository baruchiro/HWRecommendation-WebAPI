using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AlgorithmManager;
using AlgorithmManager.Extensions;
using AlgorithmManager.Factories;
using AlgorithmManager.Interfaces;
using AlgorithmManager.Model;
using AlgorithmManager.ModelAttributes;
using Microsoft.ML;
using Microsoft.ML.AutoML;
using Models;

namespace AutoML
{
    public class AutoRegression : IRecommendationAlgorithmLearner
    {
        private readonly ICollection<string> _labels = 
            typeof(MLPersonComputerModel)
            .GetPropertiesNamesByAttribute<RegressionLabelAttribute>()
            .ToList();

        private MLContext _mlContext;
        private AlgorithmManagerFactory _factory;

        public IEnumerable<LearningResult> TrainModel(MLContext mlContext,
            IEnumerable<(Person, Computer)> personComputerPairs, uint timeoutInMinutes)
        {
            return TrainAllLabels(mlContext, personComputerPairs, timeoutInMinutes, CancellationToken.None, false);
        }

        public ICollection<LearningResult> TrainModelParallel(MLContext mlContext,
            IEnumerable<(Person, Computer)> personComputerPairs,
            uint timeoutInMinutes, CancellationToken cancellationToken)
        {
            return TrainAllLabels(mlContext, personComputerPairs, timeoutInMinutes, cancellationToken, true).ToList();
        }

        private IEnumerable<LearningResult> TrainAllLabels(MLContext mlContext,
            IEnumerable<(Person, Computer)> personComputerPairs,
            uint timeoutInMinutes, CancellationToken cancellationToken, bool parallel = false)
        {
            _mlContext = mlContext;
            _factory = new AlgorithmManagerFactory(mlContext);
            var data = personComputerPairs.ToList();

            var experimentSettings = new RegressionExperimentSettings
            {
                MaxExperimentTimeInSeconds = timeoutInMinutes * 60,
                CancellationToken = cancellationToken,
                OptimizingMetric = RegressionMetric.MeanSquaredError
            };

            if (!parallel) return _labels.Select(l => TrainOneLabel(l, experimentSettings, data));

            var parallelOptions = new ParallelOptions {CancellationToken = cancellationToken};
            var results = new BlockingCollection<LearningResult>();
            Parallel.ForEach(_labels,
                parallelOptions,
                label =>
                    results.Add(
                        TrainOneLabel(label, experimentSettings, data),
                        cancellationToken)
            );
            return results;
        }

        private LearningResult TrainOneLabel(string label, RegressionExperimentSettings experimentSettings,
            IEnumerable<(Person, Computer)> data)
        {
            var dataView = _factory.CreateDataView(data);
            dataView = new PipelineBuilder(_mlContext, dataView.Schema)
                .ConvertNumberToSingle()
                .SelectColumns(TypeExtensions.GetFeatureColumns<MLPersonComputerModel>().ToArray())
                .SelectColumns(label)
                .TransformData(dataView);
            var experiment = _mlContext.Auto().CreateRegressionExperiment(experimentSettings);

            var experimentResult = experiment.Execute(dataView, label);
            
            return LearningResult.CreateFromRunDetail(experimentResult.BestRun,
                experimentResult.BestRun.ValidationMetrics.LossFunction,
                dataView.Schema,
                label);
        }
    }
}
