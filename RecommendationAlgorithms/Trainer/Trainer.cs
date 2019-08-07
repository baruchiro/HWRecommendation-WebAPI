using AlgorithmManager;
using AlgorithmManager.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.ML;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Trainer
{
    class Trainer : IDisposable
    {
        private readonly string _fakeDataFilePath = Path.Combine("Resources", "fake-data-out.csv");
        private readonly string _fakeDataDtypesFilePath = Path.Combine("Resources", "fake-data-out.dtypes.csv");
        private IEnumerable<IRecommendationAlgorithmLearner> _algorithms;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly MLContext _mlContext;
        private readonly AlgorithmLoader _loader;
        private ModelSaver _modelSaver;
        private readonly ICollection<(Person, Computer)> _data;

        public Trainer(IConfiguration configuration)
        {
            _mlContext = new MLContext(0);
            _modelSaver = new ModelSaver(_mlContext, configuration);
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
            var po = new ParallelOptions
            {
                CancellationToken = _cancellationTokenSource.Token,
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            Parallel.ForEach(_algorithms, po, algorithm => StartAlgorithmTask(algorithm, minutes));
        }

        private void StartAlgorithmTask(IRecommendationAlgorithmLearner algorithm, uint timeoutInMinutes)
        {
            var results = algorithm.TrainModelParallel(_mlContext, _modelSaver,
                _data, timeoutInMinutes,
                _cancellationTokenSource.Token);

            var parallelOptions = new ParallelOptions
            {
                CancellationToken = _cancellationTokenSource.Token
            };
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
