using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnumsNET;
using HW.Bot.Model;
using HW.Bot.Resources;
using Microsoft.Bot.Builder.Adapters;

namespace HW.Bot.UnitTests.Extensions
{
    public static class TestFlowExtensions
    {
        public static TestFlow AssertReplyContain(this TestFlow testFlow, string expected, string description = null,
            uint timeout = 3000)
        {
            return testFlow.AssertReply(
                reply =>
                {
                    if (reply.AsMessageActivity().Text.Contains(expected)) return;

                    throw new Exception(
                        $"{(description == null ? "" : description + "\n")}" +
                        $"Expected:{expected}\nReceived:{reply.AsMessageActivity().Text}");
                },
                description,
                timeout);
        }

        public static TestFlow AssertReplyContainAll(this TestFlow testFlow, string[] expected,
            string description = null, uint timeout = 3000)
        {
            return testFlow.AssertReply(
                reply =>
                {
                    if (expected.All(e => reply.AsMessageActivity().Text.Contains(e))) return;

                    throw new Exception(
                        $"{(description == null ? "" : description + "\n")}" +
                        $"Expected:{expected}\nReceived:{reply.AsMessageActivity().Text}");
                },
                description,
                timeout);
        }

        public static TestFlow AssertNewUserInsertData(this TestFlow testFlow)
        {
            var gender = Gender.NOT_DEFINE.AsString(EnumFormat.Description);
            const string age = "25";
            const string work = "student";

            return testFlow.AssertReplyContain(BotStrings.We_need_some_information)

                .AssertReplyContain(BotStrings.Select_your_gender)
                .Send(gender)

                .AssertReplyContain(BotStrings.Enter_your_age)
                .Send(age)

                .AssertReplyContain(BotStrings.Select_your_work)
                .Send(work)

                .AssertReplyContain(string.Format(BotStrings.Saving_info_of_user, "user1", "test"))
                .AssertReplyContainAll(new[] {BotStrings.Saving_your_data, gender, age, work});
        }
    }
}
