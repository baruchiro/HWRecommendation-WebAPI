using Models;
using System.Collections.Generic;

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