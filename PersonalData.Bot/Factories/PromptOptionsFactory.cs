using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Linq;

namespace HW.Bot.Factories
{
    internal class PromptOptionsFactory
    {
        internal PromptOptions CreatePromptOptions(string text, IDictionary<string, string> keyToTitle)
        {
            return new PromptOptions
            {
                Prompt = MessageFactory.Text(text),
                Choices = keyToTitle.Select(v =>
                    new Choice(v.Key)
                    {
                        Action = new CardAction(ActionTypes.ImBack, v.Value, value: v.Key)
                    })
                    .ToList()
            };
        }
    }
}