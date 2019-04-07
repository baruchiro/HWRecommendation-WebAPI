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
        internal PromptOptions CreateChoicesPromptOptions(string text, IDictionary<string, string> keyToTitle)
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

        internal PromptOptions CreateActionsPromptOptions(string text, IEnumerable<string> actions)
        {
            var activityPrompt = MessageFactory.Text(text);
            activityPrompt.SuggestedActions = new SuggestedActions
            {
                Actions = actions.Select(w => new CardAction(ActionTypes.PostBack, w, value: w)).ToList()
            };

            var promptOptions = new PromptOptions
            {
                Prompt = activityPrompt
            };
            return promptOptions;
        }
    }
}