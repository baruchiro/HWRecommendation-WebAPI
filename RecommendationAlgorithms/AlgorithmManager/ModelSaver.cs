using System;
using System.IO;
using AlgorithmManager.Interfaces;
using Microsoft.ML;

namespace AlgorithmManager
{
    public class ModelSaver
    {
        private readonly MLContext _mlContext;
        private readonly string _outputDir;

        public ModelSaver(MLContext mlContext, string outputDir)
        {
            _outputDir = outputDir;
            CreateDir(outputDir);
            _mlContext = mlContext;
        }

        private void CreateDir(string dirname)
        {
            if (!Directory.Exists(dirname))
            {
                Directory.CreateDirectory(dirname);
            }
        }

        public void SaveModel(string learningName, LearningResult learningResult, bool includeResults = true)
        {
            var now = new DateTime().ToString("yyMMdd_hhmmss");

            var currentPath = Path.Combine(_outputDir, learningName + "Results");
            CreateDir(currentPath);
            var resultsPath = Path.Combine(currentPath, $"{learningName}_{now}.result");
            var modelPath = Path.Combine(currentPath, $"{learningName}_{now}.zip");

            _mlContext.Model.Save(learningResult.Model, learningResult.Schema, modelPath);
            if (includeResults)
            {
                File.WriteAllText(resultsPath, learningResult.Result.ToString());
            }
        }
    }
}