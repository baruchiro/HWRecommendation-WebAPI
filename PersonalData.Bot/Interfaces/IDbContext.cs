using System.Collections.Generic;

namespace HW.Bot.Interfaces
{
    public interface IDbContext
    {
        IEnumerable<string> GetOrderedWorkList();
        bool SavePersonalDetails(string channelId, string userId, IPersonalData personalData);
        IPersonalData GetPersonalDetails(string channelId, string userId);
    }
}