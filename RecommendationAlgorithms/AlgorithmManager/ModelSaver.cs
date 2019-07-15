using System;
using System.IO;
using AlgorithmManager.Interfaces;
using Microsoft.ML;

namespace AlgorithmManager
{
    public class ModelSaver
    {
        private readonly LearningResult _learningResult;
        private readonly string _modelPath;
        private readonly string _resultsPath;
        private readonly MLContext _mlContext;

        public ModelSaver(string learningName, LearningResult learningResult, MLContext mlContext)
        {
            var now = new DateTime().ToString("yyMMdd_hhmmss");
            var dirname = learningName + "Results";
            CreateDir(dirname);
            _resultsPath = Path.Combine(dirname, $"{learningName}_{now}.result");
            _modelPath = Path.Combine(dirname, $"{learningName}_{now}.zip");
            _learningResult = learningResult;
            _mlContext = mlContext;
        }

        private void CreateDir(string dirname)
        {
            if (!Directory.Exists(dirname))
            {
                Directory.CreateDirectory(dirname);
            }
        }

        public void SaveResults()
        {
            File.WriteAllText(_resultsPath, _learningResult.Result);
        }

        public void SaveModel()
        {
            _mlContext.Model.Save(_learningResult.TrainedModel, _learningResult.Schema, _modelPath);
        }
    }
}