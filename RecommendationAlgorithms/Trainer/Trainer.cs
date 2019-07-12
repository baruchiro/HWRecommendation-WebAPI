using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataTestsUtils;
using Microsoft.ML;
using Microsoft.ML.AutoML;
using Microsoft.ML.Data;

namespace Trainer
{
    class Trainer
    {
        public static void Main(string[] args)
        {

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

    }
}
