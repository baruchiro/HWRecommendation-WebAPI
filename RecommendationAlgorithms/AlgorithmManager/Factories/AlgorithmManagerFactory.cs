using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlgorithmManager.Model;
using Microsoft.ML;
using Models;

namespace AlgorithmManager.Factories
{
    public class AlgorithmManagerFactory
    {
        private readonly MLContext _mlContext;
        private MLModelConverter MLModelConverter { get; set; }

        public AlgorithmManagerFactory(MLContext mlContext)
        {
            _mlContext = mlContext ?? throw new ArgumentNullException(nameof(mlContext));
            MLModelConverter = new MLModelConverter();
        }

        public PipelineBuilder CreatePipelineBuilder(IEnumerable<(Person, Computer)> personComputerPairs)
        {
            var mlPersonComputerModels = MLModelConverter.Convert(personComputerPairs);
            return CreatePipelineBuilder(mlPersonComputerModels);
        }

        public PipelineBuilder CreatePipelineBuilder(IEnumerable<MLPersonComputerModel> mlPersonComputerModels)
        {
            var dataView = _mlContext.Data.LoadFromEnumerable(mlPersonComputerModels);
            return new PipelineBuilder(_mlContext, dataView);
        }
    }
}
