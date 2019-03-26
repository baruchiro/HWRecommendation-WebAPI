using System.Collections.Generic;
using Microsoft.Bot.Schema;

namespace PersonalData.Bot.Interfaces
{
    public interface IDbContext
    {
        IEnumerable<string> GetOrderedWorkList();
        bool SavePersonalDetails(string channelId, string userId, Model.PersonalData personalData);
    }
}