using System;
using System.IO;
using System.Threading;
using AlgorithmManager.Interfaces;
using Microsoft.ML;
using Newtonsoft.Json;

namespace AlgorithmManager
{
    public class ModelSaver
    {
        private readonly MLContext _mlContext;
        private readonly string _outputDir;

        public ModelSaver(MLContext mlContext, string outputDir)
        {
            _outputDir = Directory.CreateDirectory(outputDir).FullName;
            
            _mlContext = mlContext;
        }

        public void SaveModel(LearningResult learningResult)
        {
            if (learningResult == null) throw new ArgumentNullException(nameof(learningResult));
            Thread.Sleep(TimeSpan.FromSeconds(5));
            var filename = Path.Combine(_outputDir,
                $"{learningResult.Name}_{new DateTime():yyMMdd_hhmmss}");

            var jsonFile = $"{filename}.json";
            var modelPath = $"{filename}.zip";

            var json = JsonConvert.SerializeObject(learningResult, Formatting.Indented);
            File.WriteAllText(jsonFile, json);
            _mlContext.Model.Save(learningResult.Model, learningResult.Schema, modelPath);
        }
    }
}