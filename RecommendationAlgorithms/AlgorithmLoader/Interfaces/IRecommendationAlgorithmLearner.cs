using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML.Data;

namespace Trainer.Interfaces
{
    public interface IRecommendationAlgorithmLearner
    {
        ILearningResult TrainModel(int timeoutInMinutes);
        void SaveModel(string fileName);
    }
}
