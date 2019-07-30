using System;
using System.Collections.Generic;
using System.Linq;
using ComputerUpgradeStrategies;
using HW.Bot.Interfaces;
using HWWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Models;

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
            return _dbContext.Users.Select(u => u.WorkArea)
                .GroupBy(w => w)
                .OrderByDescending(g => g.Count())
                .Select(w=>w.Key);
        }

        public bool SavePerson(string channelId, string userId, Person person)
        {
            var userChannel = new UserChannel { ChannelId = channelId, UserId = userId };
            var user = GetUserByUserChannel(userChannel);
            if (user != null)
            {
                _dbContext.Entry(user).State = EntityState.Modified;
            }
            else
            {
                user = new User { Channels = new List<UserChannel> { userChannel } };
                _dbContext.Users.Add(user);
            }

            user.Name = person.Name;
            user.WorkArea = person.WorkArea;
            user.Age = person.Age;
            user.Gender = person.Gender;

            return _dbContext.SaveChanges() > 0;
        }

        public Person GetPerson(string channelId, string userId)
        {
            var userChannel = new UserChannel { ChannelId = channelId, UserId = userId };
            return GetUserByUserChannel(userChannel);
        }

        public IEnumerable<string> GetRecommendationsForScan(Guid guid)
        {
            var scan = _dbContext.Scans
                .Include(s=>s.Computer)
                .Include(s=>s.Computer.Disks)
                .Include(s => s.Computer.Gpus)
                .Include(s => s.Computer.Memories)
                .Include(s => s.Computer.MotherBoard)
                .Include(s => s.Computer.Processor)
                .FirstOrDefault(ss => ss.Id == guid);
            var computer = scan?.Computer;

            return computer?.GetUpgradeRecommendations().Select(r=>r.ToString());
        }

        private User GetUserByUserChannel(UserChannel userChannel)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Channels.Any(c => c.EqualByMembers(userChannel)));
        }
    }
}
