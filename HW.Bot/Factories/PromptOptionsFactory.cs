﻿using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Linq;

namespace HW.Bot.Factories
{
    internal class PromptOptionsFactory
    {
        internal PromptOptions CreateActionsPromptOptions(string text, IEnumerable<string> actions)
        {
            var activityPrompt = MessageFactory.Text(text);
            activityPrompt.SuggestedActions = new SuggestedActions
            {
                Actions = actions.Select(w => new CardAction(ActionTypes.ImBack, w, value: w)).ToList()
            };

            var promptOptions = new PromptOptions
            {
                Prompt = activityPrompt
            };
            return promptOptions;
        }
    }
}