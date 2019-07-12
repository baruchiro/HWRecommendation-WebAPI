using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AlgorithmManager;
using DataTestsUtils;
using Microsoft.ML;
using AlgorithmManager.Interfaces;

namespace Trainer
{
    class Trainer : IDisposable
    {
        private IEnumerable<IRecommendationAlgorithmLearner> _algorithms;
        private readonly List<Task> _running = new List<Task>();
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly MLContext _mlContext;
        private readonly AlgorithmLoader _loader;
        private readonly DataLoader _dataLoader;
        private const string LABEL = "memory_capacity_as_kb";

        public Trainer()
        {
            _mlContext = new MLContext(0);
            _loader = new AlgorithmManager.AlgorithmLoader();
            _dataLoader = new DataLoader(_mlContext);
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
            return new TaskFactory().StartNew(
                    BuildDataViewForTrain,
                    _cancellationTokenSource.Token,
                    TaskCreationOptions.None,
                    TaskScheduler.Default)
                .ContinueWith(
                    task =>
                        algorithm.TrainModel(task.Result, LABEL, timeoutInMinutes),
                    _cancellationTokenSource.Token,
                    TaskContinuationOptions.LongRunning, TaskScheduler.Current)
                .ContinueWith(
                    (task, name) =>
                        SaveResultsToDir(task.Result, name as string),
                    algorithm.GetType().Name,
                    _cancellationTokenSource.Token,
                    TaskContinuationOptions.None, TaskScheduler.Current);
        }

        private IDataView BuildDataViewForTrain()
        {
            return _dataLoader.CreateBuilder().ConvertIntToSingle()
                    .ConvertNumberToSingle()
                    .SelectFeatureColumns()
                    .SelectColumns(LABEL)
                    .GetData();
        }

        private void SaveResultsToDir(LearningResult learningResult, string name)
        {
            var saver = new ModelSaver(name, learningResult, _mlContext);
            saver.SaveResults();
            saver.SaveModel();
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
