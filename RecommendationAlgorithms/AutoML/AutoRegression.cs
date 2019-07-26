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

        private MLContext _mlContext;
        private AlgorithmManagerFactory _factory;
        private MLModelConverter _converter;
        private readonly ModelSaver _modelSaver;

        public AutoRegression(){}

        public AutoRegression(MLContext mlContext, MLModelConverter converter, ModelSaver modelSaver)
        {
            _mlContext = mlContext;
            _converter = converter;
            _modelSaver = modelSaver;
        }

        public IEnumerable<LearningResult> TrainModel(MLContext mlContext,
            ModelSaver modelSaver,
            IEnumerable<(Person, Computer)> personComputerPairs,
            uint timeoutInMinutes)
        {
            return TrainAllLabels(mlContext, modelSaver, personComputerPairs, timeoutInMinutes, CancellationToken.None, false);
        }

        public ICollection<LearningResult> TrainModelParallel(MLContext mlContext,
            ModelSaver modelSaver,
            IEnumerable<(Person, Computer)> personComputerPairs,
            uint timeoutInMinutes,
            CancellationToken cancellationToken)
        {
            return TrainAllLabels(mlContext, modelSaver, personComputerPairs, timeoutInMinutes, cancellationToken, true).ToList();
        }

        private IEnumerable<LearningResult> TrainAllLabels(MLContext mlContext,
            ModelSaver modelSaver,
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

            if (!parallel) return _labels.Select(l => TrainOneLabel(l, experimentSettings, data, modelSaver));

            var parallelOptions = new ParallelOptions {CancellationToken = cancellationToken};
            var results = new BlockingCollection<LearningResult>();
            Parallel.ForEach(_labels,
                parallelOptions,
                label =>
                    results.Add(
                        TrainOneLabel(label, experimentSettings, data, modelSaver),
                        cancellationToken)
            );
            return results;
        }

        private LearningResult TrainOneLabel(string label, RegressionExperimentSettings experimentSettings,
            IEnumerable<(Person, Computer)> data, ModelSaver modelSaver)
        {
            var dataView = _factory.CreateDataView(data);
            dataView = new PipelineBuilder(_mlContext, dataView.Schema)
                .ConvertNumberToSingle()
                .SelectColumns(TypeExtensions.GetFeatureColumns<MLPersonComputerModel>().ToArray())
                .SelectColumns(label)
                .TransformData(dataView);
            var experiment = _mlContext.Auto().CreateRegressionExperiment(experimentSettings);

            var experimentResult = experiment.Execute(dataView, label);

            var learningResult = LearningResult.CreateFromRunDetail(experimentResult.BestRun,
                experimentResult.BestRun.ValidationMetrics.LossFunction,
                dataView.Schema,
                label);

            modelSaver?.SaveModel(learningResult);

            return learningResult;
        }

        public IEnumerable<(string Field, float PredictedValue)> GetResults(Person person)
        {
            var dataInsert = _factory.PersonToMLModelPersonComputer(person);

            return _labels
                .Select(l =>
                    (l,
                        _mlContext.Model.CreatePredictionEngine<MLPersonComputerModel, ComputerPrediction>(
                            _modelSaver.LoadModel(l).Model).Predict(dataInsert).PredictedPrice));
        }
    }

    internal class ComputerPrediction : MLPersonComputerModel
    {
        [ColumnName("Score")]
        public float PredictedPrice { get; set; }
    }
}
