using System;
using System.Collections.Generic;
using System.Text;
using AlgorithmManager.Factories;
using AlgorithmManager.Model;
using Microsoft.ML;
using Xunit;

namespace AlgorithmManagerTests
{
    public class AlgorithmManagerFactoryTests
    {
        [Fact]
        public void CreatePipeline_LoadDataView_FromSimpleObject()
        {
            var mlContext = new MLContext();
            var factory = new AlgorithmManagerFactory(mlContext);
            var dataView = factory.CreatePipelineBuilder(new[] {new MLPersonComputerModel()}).GetData();
        }
    }
}
