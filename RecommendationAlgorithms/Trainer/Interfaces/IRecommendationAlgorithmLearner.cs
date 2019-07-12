using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML.Data;

namespace Trainer.Interfaces
{
    interface IRecommendationAlgorithmLearner
    {
        ILearningResult TrainModel(int timeoutInMinutes);
        void SaveModel(string fileName);
    }
}
