using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML;

namespace Trainer.Interfaces
{
    public interface ILearningResult
    {
        string Result { get;}
        ITransformer TrainedModel { get; }
        IDataLoader<object> Schema { get; }
    }
}
