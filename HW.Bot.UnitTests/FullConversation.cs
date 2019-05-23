using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using EnumsNET;
using FakeItEasy;
using HW.Bot.Interfaces;
using HW.Bot.Model;
using HW.Bot.Resources;
using HW.Bot.UnitTests.Extensions;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Adapters;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Xunit;

namespace HW.Bot.UnitTests
{
    public class FullConversation
    {
        private readonly IDbContext _dbContext;
        private readonly TestAdapter _adapter;

        public FullConversation()
        {
            _dbContext = A.Fake<IDbContext>();
            A.CallTo(() => _dbContext.GetPersonalDetails(A<string>.Ignored, A<string>.Ignored))
                .Returns(null);
            A.CallTo(
                    () => _dbContext.SavePersonalDetails(A<string>.Ignored, A<string>.Ignored, A<IPersonalData>.Ignored))
                .Returns(true);
            _adapter = new TestAdapter()
                .Use(new AutoSaveStateMiddleware(new ConversationState(new MemoryStorage())));
        }
        [Fact]
        public async Task FullConversation_PersonalData()
        {
            await new TestFlow(_adapter, BotRegistrationExtension.GetBotForTest(_dbContext))
                .Send("Hi")
                .AssertReplyContain(BotStrings.MainMenuTitle)

                .Send(BotStrings.Manage_your_personal_information)
                .AssertNewUserInsertData()


                .StartTestAsync();
        }

        [Fact]
        public async Task FullConversation_ExistComputerRecommendation()
        {
            await new TestFlow(_adapter, BotRegistrationExtension.GetBotForTest(_dbContext))
                .Send("Hi")
                .AssertReplyContain(BotStrings.MainMenuTitle)

                .Send(BotStrings.RecommendationMenuItemTitle)

                // TODO: Bug, duplicate message
                .AssertReplyContain(BotStrings.We_need_some_information)
                .AssertNewUserInsertData()

                .StartTestAsync();
        }
    }
}
