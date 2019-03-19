using System.Collections.Generic;
using Microsoft.Bot.Schema;

namespace PersonalData.Bot.Interfaces
{
    public interface IDbContext
    {
        IEnumerable<string> GetOrderedWorkList();
        bool SavePersonalDetails(Model.PersonalData personalData);
    }
}