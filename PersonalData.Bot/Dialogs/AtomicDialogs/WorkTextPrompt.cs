using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HW.Bot.Dialogs.AtomicDialogs
{
    internal class WorkTextPrompt : TextPrompt
    {
        private readonly string dialogId;

        public WorkTextPrompt(string dialogId, PromptValidator<string> validator = null) : base(dialogId, validator)
        {
            this.dialogId = dialogId;
        }

        internal Task<DialogTurnResult> PromptAsync(WaterfallStepContext stepcontext, CancellationToken cancellationtoken, IEnumerable<string> suggestedActions = null)
        {
            var activityPrompt = MessageFactory.Text("Select one of this works or write your own.");
            activityPrompt.SuggestedActions = new SuggestedActions
            {
                Actions = suggestedActions?.Select(w => new CardAction(ActionTypes.PostBack, w, value: w)).ToList()
            };

            var promptOptions = new PromptOptions
            {
                Prompt = activityPrompt
            };

            return stepcontext.PromptAsync(this.dialogId, promptOptions, cancellationtoken);
        }
    }
}