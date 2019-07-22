﻿using System;
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
        private readonly DataLoader _dataLoader;
        private readonly string _outputDir;
        private ModelSaver _modelSaver;

        public Trainer(string outputDir)
        {
            _modelSaver = new ModelSaver(_mlContext, outputDir);
            _mlContext = new MLContext(0);
            _loader = new AlgorithmLoader();
            _dataLoader = new DataLoader(_mlContext, _fakeDataFilePath, _fakeDataDtypesFilePath);
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
                    BuildEnumerationForTrain,
                    _cancellationTokenSource.Token,
                    TaskCreationOptions.None,
                    TaskScheduler.Default)
                .ContinueWith(
                    task =>
                        algorithm.TrainModel(_mlContext, task.Result, "ComputerMemoriesCapacity", timeoutInMinutes),
                    _cancellationTokenSource.Token,
                    TaskContinuationOptions.LongRunning, TaskScheduler.Current)
                .ContinueWith(
                    (task, name) =>
                        SaveResultsToDir(task.Result, name as string),
                    algorithm.GetType().Name,
                    _cancellationTokenSource.Token,
                    TaskContinuationOptions.None, TaskScheduler.Current);
        }

        private IEnumerable<(Person, Computer)> BuildEnumerationForTrain()
        {
            return _dataLoader.EnumerateData().ToList();
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
