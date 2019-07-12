using System;
using System.IO;
using Microsoft.ML;
using Trainer.Interfaces;

namespace Trainer
{
    internal class ModelSaver
    {
        private readonly ILearningResult _learningResult;
        private readonly string _modelPath;
        private readonly string _resultsPath;
        private readonly MLContext _mlContext;

        public ModelSaver(string learningName, ILearningResult learningResult, MLContext mlContext)
        {
            var now = new DateTime().ToString("yy-MM-dd_hh:mm:ss");
            _resultsPath = Path.Combine(learningName, $"{learningName}_{now}.result");
            _modelPath = Path.Combine(learningName, $"{learningName}_{now}.zip");
            _learningResult = learningResult;
            _mlContext = mlContext;
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