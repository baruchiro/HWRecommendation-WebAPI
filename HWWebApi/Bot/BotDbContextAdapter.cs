using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HWWebApi.Controllers;
using HWWebApi.Models;
using PersonalData.Bot.Interfaces;

namespace HWWebApi.Bot
{
    public class BotDbContextAdapter : IDbContext
    {
        private readonly HardwareContext _dbContext;

        public BotDbContextAdapter(HardwareContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<string> GetOrderedWorkList()
        {
            return new[] {"Student", "Hi-Tech"};
        }

        public bool SavePersonalDetails(string channelId, string userId, IPersonalData personalData)
        {
            var worksController = new WorksController(_dbContext);
            //if(worksController.Post(personalData.Work))
            //var work = _dbContext.Works.FirstOrDefault(w =>
            //    w.Name.Equals(personalData.Work, StringComparison.CurrentCultureIgnoreCase));

            //if (string.IsNullOrEmpty(work.Name))
            //{
            //    work.Name = personalData.Work;
            //    _dbContext.Works.Add(work);
            //}
            return false;
        }
    }
}
