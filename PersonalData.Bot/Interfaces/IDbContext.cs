using System;
using System.Collections;
using System.Collections.Generic;
using Models;

namespace HW.Bot.Interfaces
{
    public interface IDbContext
    {
        IEnumerable<string> GetOrderedWorkList();
        bool SavePersonalDetails(string channelId, string userId, Person personalData);
        Person GetPersonalDetails(string channelId, string userId);
        IEnumerable<string> GetRecommendationsForScan(Guid guid);
    }
}