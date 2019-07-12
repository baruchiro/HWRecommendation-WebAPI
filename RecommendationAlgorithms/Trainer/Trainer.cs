using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using DataTestsUtils;
using DocoptNet;
using Microsoft.ML;
using Microsoft.ML.Data;
using AlgorithmLoader;
using AlgorithmLoader.Interfaces;

namespace Trainer
{
    class Trainer
    {
        private IEnumerable<IRecommendationAlgorithmLearner> algorithms;
        private List<Task> running = new List<Task>();
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly MLContext _mlContext;
        private AlgorithmLoader.AlgorithmLoader _loader;
        private readonly IDataView Data;
        private DataLoader dataLoader;
        private string label = "memory_capacity_as_kb";

        public Trainer()
        {
            _mlContext = new MLContext(0);
            _loader = new AlgorithmLoader.AlgorithmLoader();
            dataLoader = new DataLoader(_mlContext);
        }

        public void TrainAll(uint minutes)
        {
            try
            {
                algorithms = _loader.LoadAllRegressionAlgorithms();
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
            var tasks = algorithms.Select(a=> StartAlgorithmTask(a, minutes));
            running.AddRange(tasks);
        }

        private Task StartAlgorithmTask(IRecommendationAlgorithmLearner algorithm, uint timeoutInMinutes)
        {
            return new TaskFactory().StartNew(
                    BuildDataViewForTrain,
                    cancellationTokenSource.Token,
                    TaskCreationOptions.None,
                    TaskScheduler.Default)
                .ContinueWith(
                    task =>
                        algorithm.TrainModel(task.Result, label, timeoutInMinutes),
                    cancellationTokenSource.Token,
                    TaskContinuationOptions.LongRunning, TaskScheduler.Current)
                .ContinueWith(
                    (task, name) =>
                        SaveResultsToDir(task.Result, name as string),
                    algorithm.GetType().Name,
                    cancellationTokenSource.Token,
                    TaskContinuationOptions.None, TaskScheduler.Current);
        }

        private IDataView BuildDataViewForTrain()
        {
            return dataLoader.CreateBuilder().ConvertIntToSingle()
                    .ConvertNumberToSingle()
                    .SelectFeatureColumns()
                    .SelectColumns(label)
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
            Task.WaitAll(running.ToArray(), cancellationTokenSource.Token);
        }

        public void Cancel()
        {
            cancellationTokenSource.Cancel();
        }
    }
}
