using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML;
using Models;

namespace AlgorithmManager.Factories
{
    public class AlgorithmManagerFactory
    {
        private readonly MLContext _mlContext;
        private ModelFlatenner ModelFlattener { get; set; }

        public AlgorithmManagerFactory(MLContext mlContext)
        {
            _mlContext = mlContext ?? throw new ArgumentNullException(nameof(mlContext));
            ModelFlattener = new ModelFlatenner();
        }

        public PipelineBuilder CreatePipelineBuilder(IEnumerable<(Person, Computer)> personComputerPairs)
        {
            var flattenCollection = ModelFlattener.Flatten(personComputerPairs);
            var dataView = _mlContext.Data.LoadFromEnumerable(flattenCollection);
            return new PipelineBuilder(_mlContext, dataView);
        }
    }
}
