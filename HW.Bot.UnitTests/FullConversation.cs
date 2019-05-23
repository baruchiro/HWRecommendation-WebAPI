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
        [Fact]
        public async Task FullConversation_PersonalData()
        {
            var dbContext = A.Fake<IDbContext>();
            A.CallTo(() => dbContext.GetPersonalDetails(A<string>.Ignored, A<string>.Ignored))
                .Returns(null);
            A.CallTo(
                    () => dbContext.SavePersonalDetails(A<string>.Ignored, A<string>.Ignored, A<IPersonalData>.Ignored))
                .Returns(true);

            var adapter = new TestAdapter()
                .Use(new AutoSaveStateMiddleware(new ConversationState(new MemoryStorage())));

            await new TestFlow(adapter, BotRegistrationExtension.GetBotForTest(dbContext))
                .Send("Hi")
                .AssertReplyContain(BotStrings.MainMenuTitle)

                .Send(BotStrings.Manage_your_personal_information)
                .AssertNewUserInsertData()


                .StartTestAsync();
        }
    }
}
