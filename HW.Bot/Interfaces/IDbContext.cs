using Models;
using System;
using System.Collections.Generic;

namespace HW.Bot.Interfaces
{
    public interface IDbContext
    {
        IEnumerable<string> GetOrderedWorkList();
        bool SavePerson(string channelId, string userId, Person person);
        Person GetPerson(string channelId, string userId);
        IEnumerable<string> GetRecommendationsForScan(Guid guid);
    }
}