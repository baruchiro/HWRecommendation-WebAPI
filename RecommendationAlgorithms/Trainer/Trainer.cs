using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AlgorithmManager;
using Microsoft.ML;
using AlgorithmManager.Interfaces;
using Models;

namespace Trainer
{
    class Trainer : IDisposable
    {
        private readonly string _fakeDataFilePath = Path.Combine("Resources", "fake-data-out.csv");
        private readonly string _fakeDataDtypesFilePath = Path.Combine("Resources", "fake-data-out.dtypes.csv");
        private IEnumerable<IRecommendationAlgorithmLearner> _algorithms;
        private readonly List<Task> _running = new List<Task>();
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly MLContext _mlContext;
        private readonly AlgorithmLoader _loader;
        private ModelSaver _modelSaver;
        private readonly ICollection<(Person, Computer)> _data;

        public Trainer(string outputDir)
        {
            _mlContext = new MLContext(0);
            _modelSaver = new ModelSaver(_mlContext, outputDir);
            _loader = new AlgorithmLoader();
            _data = new DataLoader(_mlContext, _fakeDataFilePath, _fakeDataDtypesFilePath)
                .EnumerateData().ToList();
        }

        public void TrainAll(uint minutes)
        {
            try
            {
                _algorithms = _loader.LoadAllRegressionAlgorithms();
                RunAllAlgorithms(minutes);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void RunAllAlgorithms(uint minutes)
        {
            var tasks = _algorithms.Select(a=> StartAlgorithmTask(a, minutes));
            _running.AddRange(tasks);
        }

        private Task StartAlgorithmTask(IRecommendationAlgorithmLearner algorithm, uint timeoutInMinutes)
        {
            return new TaskFactory<ICollection<LearningResult>>().StartNew(() =>
                        algorithm.TrainModelParallel(_mlContext, _data, timeoutInMinutes, _cancellationTokenSource.Token),
                    _cancellationTokenSource.Token,
                    TaskCreationOptions.LongRunning,
                    TaskScheduler.Current)
                .ContinueWith(
                    (task, name) =>// TODO: foreach
                        SaveResultsToDir(task.Result.FirstOrDefault(), name as string),
                    algorithm.GetType().Name,
                    _cancellationTokenSource.Token,
                    TaskContinuationOptions.None, TaskScheduler.Current);
        }

        private void SaveResultsToDir(LearningResult learningResult, string name)
        {
            _modelSaver.SaveModel(name, learningResult);
        }

        public void WaitAll()
        {
            Task.WaitAll(_running.ToArray(), _cancellationTokenSource.Token);
        }

        public void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Dispose();
        }
    }
}
