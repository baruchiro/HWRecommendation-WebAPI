using AlgorithmManager;
using AlgorithmManager.Factories;
using HW.Bot.Interfaces;
using Microsoft.ML;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace AutoML
{
    public class AutoMLRecommender : IRecommender
    {
        private readonly AutoRegression _regression;

        public AutoMLRecommender(MLContext mlContext, AlgorithmManagerFactory factory, ModelSaver modelSaver)
        {
            _regression = new AutoRegression(mlContext, factory, modelSaver);
        }

        public IEnumerable<IRecommend> GetNewComputerRecommendations(Person person)
        {
            return _regression.GetResults(person).Select(r =>
                new Recommend(r.Field, r.PredictedValue));
        }

        public bool IsReadyToGiveRecommendation()
        {
            return _regression.IsEngineLoaded();
        }
    }

    internal class Recommend : IRecommend
    {
        private readonly string _field;
        private readonly float _predictedValue;

        public Recommend(string field, float predictedValue)
        {
            _field = field;
            _predictedValue = predictedValue;
        }

        public string RecommendMessage()
        {
            return $"{_field}: {_predictedValue}";
        }
    }
}
