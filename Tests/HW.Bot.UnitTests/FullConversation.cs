using System;
using System.Linq;
using System.Threading.Tasks;
using ComputerUpgradeStrategies.Recommendations.Disk;
using FakeItEasy;
using HW.Bot.Interfaces;
using HW.Bot.Resources;
using HW.Bot.UnitTests.Extensions;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Adapters;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Xunit;

namespace HW.Bot.UnitTests
{
    public class FullConversation
    {
        private readonly IDbContext _dbContext;
        private readonly TestAdapter _adapter;
        private readonly IBot _bot;
        private readonly IRecommender _recommender;

        public FullConversation()
        {
            _dbContext = A.Fake<IDbContext>();
            A.CallTo(() => _dbContext.GetPerson(A<string>.Ignored, A<string>.Ignored))
                .Returns(null);
            A.CallTo(
                    () => _dbContext.SavePerson(A<string>.Ignored, A<string>.Ignored,
                        A<Person>.Ignored))
                .Returns(true);

            _recommender = A.Fake<IRecommender>();

            _adapter = new TestAdapter()
                .Use(new AutoSaveStateMiddleware(new ConversationState(new MemoryStorage())));

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddRecommendationBot(provider => _dbContext,
                provider => _recommender);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            _bot = serviceProvider.GetService<IBot>();
        }

        [Fact]
        public async Task FullConversation_PersonData()
        {
            await new TestFlow(_adapter, _bot)
                .Send("Hi")
                .AssertReplyContain(BotStrings.MainMenuTitle)

                .Send(BotStrings.Manage_your_personal_information)
                .AssertNewUserInsertData()

                .StartTestAsync();
        }

        [Fact]
        public async Task FullConversation_ExistComputerRecommendation()
        {
            A.CallTo(() => _dbContext.GetRecommendationsForScan(A<Guid>.Ignored))
                .Returns(new[] {DiskRecommendations.Replace_HDD_SDD});

            await new TestFlow(_adapter, _bot)
                .Send("Hi")
                .AssertReplyContain(BotStrings.MainMenuTitle)

                .Send(BotStrings.RecommendationMenuItemTitle)

                // TODO: Bug, duplicate message
                .AssertReplyContain(BotStrings.We_need_some_information)
                .AssertNewUserInsertData()

                .AssertReplyContainAll(new[]
                    {BotStrings.DownloadOurSoftware_windows_withoutLink, BotStrings.LinkToLateasSoftware_windows})
                .AssertReplyContainAll(new[] {BotStrings.ScanIdOrExit})

                .Send(Guid.NewGuid().ToString())
                .AssertReplyContain(BotStrings.Here_our_recommendations_for_you)
                .AssertReplyContain(DiskRecommendations.Replace_HDD_SDD)

                .StartTestAsync();
        }

        [Fact]
        public async Task FullConversation_NewComputerRecommendation()
        {
            var recommendations = new[]
            {
                "DDR: 3",
                "Processor: 4"
            };
            var recommendFake = A.Fake<IRecommend>();
            A.CallTo(() => recommendFake.RecommendMessage()).ReturnsNextFromSequence(recommendations);

            A.CallTo(() => _recommender.IsReadyToGiveRecommendation()).Returns(true);
            A.CallTo(() => _recommender.GetNewComputerRecommendations(A<Person>.Ignored))
                .Returns(Enumerable.Repeat(recommendFake, recommendations.Length));

            await new TestFlow(_adapter, _bot)
                .Send("Hi")
                .AssertReplyContain(BotStrings.MainMenuTitle, "Main menu")

                .Send("NewComputerDialogComponent")

                // TODO: Bug, duplicate message
                .AssertReplyContain(BotStrings.We_need_some_information, "The user not exist")
                .AssertNewUserInsertData()

                .AssertReplyContain(BotStrings.We_taking_personal_info_retrieve_recommendations)
                .AssertEachReplyContainOneOf(recommendations)

                .StartTestAsync();
        }
    }
}
