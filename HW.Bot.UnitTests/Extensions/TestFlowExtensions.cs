using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Bot.Builder.Adapters;

namespace HW.Bot.UnitTests.Extensions
{
    public static class TestFlowExtensions
    {
        public static TestFlow AssertReplyContain(this TestFlow testFlow, string expected, string description = null, uint timeout = 3000)
        {
            return testFlow.AssertReply(
                reply =>
                {
                    if (reply.AsMessageActivity().Text.Contains(expected)) return;

                    throw new Exception(
                        $"{(description==null? "": description + "\n")}" +
                        $"Expected:{expected}\nReceived:{reply.AsMessageActivity().Text}");
                },
                description,
                timeout);
        }
        public static TestFlow AssertReplyContainAll(this TestFlow testFlow, string[] expected, string description = null, uint timeout = 3000)
        {
            return testFlow.AssertReply(
                reply =>
                {
                    if (expected.All(e=> reply.AsMessageActivity().Text.Contains(e))) return;

                    throw new Exception(
                        $"{(description == null ? "" : description + "\n")}" +
                        $"Expected:{expected}\nReceived:{reply.AsMessageActivity().Text}");
                },
                description,
                timeout);
        }

    }
}
