using System.Collections.Generic;
using Models;

namespace HW.Bot.Interfaces
{
    public interface IRecommender
    {
        bool IsReadyToGiveRecommendation();
        IEnumerable<IRecommend> GetNewComputerRecommendations(Person person);
    }

    public interface IRecommend
    {
        string RecommendMessage();
    }
}