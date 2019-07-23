using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using Microsoft.ML.Data;
using Models;

namespace AutoML
{
    public class AutoRegression : IRecommendationAlgorithmLearner
    {
        private readonly ICollection<string> _labels = 
            typeof(MLPersonComputerModel)
            .GetPropertiesNamesByAttribute<RegressionLabelAttribute>()
            .ToList();

        private IDataView _dataView;
        private MLContext _mlContext;

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
            var factory = new AlgorithmManagerFactory(mlContext);
            _dataView = factory.CreateDataView(personComputerPairs);

            var experimentSettings = new RegressionExperimentSettings
            {
                MaxExperimentTimeInSeconds = timeoutInMinutes * 60,
                CancellationToken = cancellationToken,
                OptimizingMetric = RegressionMetric.MeanSquaredError
            };

            var enumeration =
                parallel ? _labels.AsParallel().WithCancellation(cancellationToken) : _labels.AsEnumerable();

            return enumeration.Select(l => TrainOneLabel(l, experimentSettings));
        }

        private LearningResult TrainOneLabel(string label, RegressionExperimentSettings experimentSettings)
        {
            var dataView = new PipelineBuilder(_mlContext, _dataView.Schema)
                .ConvertNumberToSingle()
                .SelectColumns(TypeExtensions.GetFeatureColumns<MLPersonComputerModel>().ToArray())
                .SelectColumns(label)
                .TransformData(_dataView);
            var experiment = _mlContext.Auto().CreateRegressionExperiment(experimentSettings);

            var experimentResult = experiment.Execute(dataView, label);
            return LearningResult.CreateFromRunDetail(experimentResult.BestRun,
                dataView.Schema,
                experimentResult.BestRun.ValidationMetrics.LossFunction);
        }
    }
}
