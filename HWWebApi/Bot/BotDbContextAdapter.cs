using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HWWebApi.Models;
using PersonalData.Bot.Interfaces;

namespace HWWebApi.Bot
{
    public class BotDbContextAdapter : IDbContext
    {
        private readonly HardwareContext _dbContext;

        public BotDbContextAdapter(HardwareContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IEnumerable<string> GetOrderedWorkList()
        {
            return new[] {"Student", "Hi-Tech"};
        }

        public bool SavePersonalDetails(Model.PersonalData personalData)
        {
            return true;
        }
    }
}
