using HW.Bot.Factories;
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

        public WorkTextPrompt(string dialogId, PromptValidator<string> validator = null) : base(dialogId, validator)
        {
        }

        internal Task<DialogTurnResult> PromptAsync(WaterfallStepContext stepcontext, CancellationToken cancellationtoken, IEnumerable<string> suggestedActions = null)
        {
            var promptOptions = new PromptOptionsFactory()
                .CreateActionsPromptOptions("Select your work or write a new one:", suggestedActions);

            return stepcontext.PromptAsync(Id, promptOptions, cancellationtoken);
        }
    }
}