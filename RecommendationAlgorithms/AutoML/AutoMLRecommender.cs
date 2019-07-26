using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HW.Bot.Interfaces;
using Models;

namespace AutoML
{
    public class AutoMLRecommender : IRecommender
    {
        private readonly AutoRegression _regression;

        public AutoMLRecommender()
        {
            _regression = new AutoRegression();
        }

        public IEnumerable<IRecommend> GetNewComputerRecommendations(Person person)
        {
            return null;
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
