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
        public async Task Bot_RunFullConversation()
        {
            var gender = Gender.NOT_DEFINE.AsString(EnumFormat.Description);
            const string age = "25";
            const string work = "student";

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

                .Send(BotStrings.RecommendationMenuItemTitle)
                .AssertReplyContain(BotStrings.We_need_some_information)
                
                .AssertReplyContain(BotStrings.Select_your_gender)
                .Send(gender)
                
                .AssertReplyContain(BotStrings.Enter_your_age)
                .Send(age)

                .AssertReplyContain(BotStrings.Select_your_work)
                .Send(work)

                .AssertReplyContain(string.Format(BotStrings.Saving_info_of_user, "user1", "test"))
                .AssertReplyContainAll(new []{BotStrings.Saving_your_data, gender,age,work})


                .StartTestAsync();
        }
    }
}
