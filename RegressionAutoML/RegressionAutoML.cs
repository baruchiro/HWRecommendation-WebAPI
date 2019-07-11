using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.ML;
using Microsoft.ML.AutoML;
using Microsoft.ML.Data;

namespace RegressionAutoML
{
    public class RegressionAutoML
    {
        public RunDetail<RegressionMetrics> Train(IDataView data)
        {
            var mlContext = new MLContext();

            var cts = new CancellationTokenSource();
            var experimentSettings = new RegressionExperimentSettings
            {
                MaxExperimentTimeInSeconds = 1*60,
                CancellationToken = cts.Token,
                OptimizingMetric = RegressionMetric.MeanSquaredError
            };

            var experiment = mlContext.Auto().CreateRegressionExperiment(experimentSettings);

            var experimentResult = experiment
                .Execute(data, "memory_capacity_as_kb");

            return experimentResult.BestRun;
        }
    }
}
