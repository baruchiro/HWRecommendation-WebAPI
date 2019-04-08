using HW.Bot.Factories;
using Microsoft.Bot.Builder.Dialogs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HW.Bot.Dialogs.AtomicDialogs
{
    internal class WorkTextPrompt : TextPrompt
    {
        private PromptOptions promptOptions;

        public WorkTextPrompt(string dialogId, PromptValidator<string> validator = null, IEnumerable<string> suggestedActions = null) : base(dialogId, validator)
        {
            this.promptOptions = new PromptOptionsFactory()
                     .CreateActionsPromptOptions("Select your work or write a new one:", suggestedActions);
        }

        internal Task<DialogTurnResult> PromptAsync(WaterfallStepContext stepcontext, CancellationToken cancellationtoken)
        {
            return stepcontext.PromptAsync(Id, promptOptions, cancellationtoken);
        }

        public override async Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.BeginDialogAsync(dc, options ?? promptOptions, cancellationToken);
        }
    }
}