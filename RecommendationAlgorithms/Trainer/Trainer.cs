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
using Microsoft.ML.AutoML;
using Microsoft.ML.Data;
using Trainer.Interfaces;

namespace Trainer
{
    class Trainer
    {
        private IEnumerable<IRecommendationAlgorithmLearner> algorithms;
        private ICollection<Task> running = new List<Task>();
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly MLContext _mlContext;

        public Trainer()
        {
            _mlContext = new MLContext(0);
        }

        public async Task TrainAsync()
        {
            string[] labels =
            {
                "memory_capacity_as_kb",
                "disk_capacity_as_kb",
                "disk_rpm"
            };
            var mlContext = new MLContext();
            var dataLoader = new DataLoader(mlContext);

            var results = new List<RunDetail<RegressionMetrics>>();

            var taskFactory = new TaskFactory<RunDetail<RegressionMetrics>>();
            var tasks = labels.Select(l =>
                taskFactory.StartNew(() => RunForLabel(dataLoader, l))).ToList();

            while (tasks.Count > 0)
            {
                var finished = await Task.WhenAny(tasks);
                tasks.Remove(finished);
                results.Add(await finished);
            }

            foreach (var result in results)
            {
                var metrics = result.ValidationMetrics;
                var r = $"R-Squared: {metrics.RSquared:0.##}";
                var m = $"Root Mean Squared Error: {metrics.RootMeanSquaredError:0.##}";
            }
        }

        private RunDetail<RegressionMetrics> RunForLabel(DataLoader dataLoader, string label)
        {
            var data = dataLoader.Builder.ConvertIntToSingle()
                .ConvertNumberToSingle()
                .SelectFeatureColumns()
                .SelectColumns(label)
                .GetData();

            var regressionAutoML = new RegressionAutoML.RegressionAutoML();
            var result = regressionAutoML.Train(data, label, 10);
            return result;
        }

        public void TrainAll(int minutes)
        {
            try
            {
                LoadAllAlgorithms();
                RunAllAlgorithms(minutes);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void RunAllAlgorithms(int minutes)
        {
            var tasks = algorithms.Select(CreateTask);
            foreach (var task in tasks)
            {
                task.Start();
                running.Add(task);
            }
        }

        private Task CreateTask(IRecommendationAlgorithmLearner algorithm, int arg2)
        {
            return new Task<ILearningResult>(() =>
                        algorithm.TrainModel(arg2),
                    cancellationTokenSource.Token)
                .ContinueWith((task, name) =>
                        SaveResultsToDir(task.Result, name as string),
                    algorithm.GetType().Name,
                    cancellationTokenSource.Token);
        }

        private void SaveResultsToDir(ILearningResult learningResult, string name)
        {
            var saver = new ModelSaver(name, learningResult, _mlContext);
            saver.SaveResults();
            saver.SaveModel();
        }

        private void LoadAllAlgorithms()
        {
            //var dir = Directory.EnumerateFiles(".", "Regression*.dll")
            //    .Select(Path.GetFullPath).ToList();
            //var ass = dir.Select(Assembly.LoadFile).ToList();
            //var i = GetTypeFromAssembly<IRecommendationAlgorithmLearner>(ass.First()).ToList();
            algorithms = Directory.EnumerateFiles(".", "Regression*.dll")
                .Select(Path.GetFullPath)
                .Select(Assembly.LoadFile)
                .SelectMany(GetTypeFromAssembly<IRecommendationAlgorithmLearner>);
        }

        private IEnumerable<T> GetTypeFromAssembly<T>(Assembly assembly)
        {
            var relevantTypes = assembly.GetTypes()
                .Where(t => typeof(T).IsAssignableFrom(t));

            foreach (var type in relevantTypes)
            {
                if (Activator.CreateInstance(type) is T t)
                    yield return t;
            }
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
