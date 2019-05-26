using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HWWebApi.Bot;
using HWWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HWWebApi.UnitTest.Bot
{
    public class BotDbContextAdapterTests
    {
        private readonly DbContextOptions<HardwareContext> _options;
        public BotDbContextAdapterTests()
        {
            _options = TestUtils.TestUtils.GetInMemoryDbContextOptions<HardwareContext>().Options;
        }
        [Fact]
        public void GetOrderedWorkList_OrderHighToLow()
        {
            var dbContext = new HardwareContext(_options);
            var adapter = new BotDbContextAdapter(dbContext);
            var worksDict = new Dictionary<string, int>
            {
                {"Student", 10 },
                {"HiTech", 4 },
                {"FFF", 3 }
            };

            dbContext.Users.AddRange(worksDict.SelectMany(kv=>GenerateUsersForWork(kv.Key,kv.Value)));
            dbContext.SaveChanges();

            var orderedList = adapter.GetOrderedWorkList().ToArray();

            Assert.Equal(worksDict.Count, orderedList.Length);
            for (int i = 0; i < worksDict.Count; i++)
            {
                Assert.Equal(worksDict.Keys.ElementAt(i), orderedList[i]);
            }
        }

        private static IEnumerable<User> GenerateUsersForWork(string work, int count)
        {
            for (var i = 0; i < count; i++)
            {
                yield return new User {WorkArea = work};
            }
        }
    }
}
