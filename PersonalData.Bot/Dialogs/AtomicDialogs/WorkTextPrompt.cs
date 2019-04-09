using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HW.Bot.Dialogs.MenuDialog;
using HW.Bot.Factories;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace HW.Bot.Dialogs.AtomicDialogs
{
    internal class WorkTextPrompt : TextPrompt, IMenuItemDialog
    {
        private PromptOptions promptOptions;
        public Func<ITurnContext, object, CancellationToken, Task> HandleResult { get; set; }

        public WorkTextPrompt(string dialogId, PromptValidator<string> validator = null, IEnumerable<string> suggestedActions = null) : base(dialogId, validator)
        {
            promptOptions = new PromptOptionsFactory()
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

        public Dialog GetDialog()
        {
            return this;
        }

    }
}