using System.Collections.Generic;
using Microsoft.Bot.Schema;

namespace HW.Bot.Interfaces
{
    public interface IDbContext
    {
        IEnumerable<string> GetOrderedWorkList();
        bool SavePersonalDetails(string channelId, string userId, IPersonalData personalData);
    }
}