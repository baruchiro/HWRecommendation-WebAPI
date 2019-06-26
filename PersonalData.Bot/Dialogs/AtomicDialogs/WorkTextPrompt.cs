using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HW.Bot.Dialogs.MenuDialog;
using HW.Bot.Factories;
using HW.Bot.Resources;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace HW.Bot.Dialogs.AtomicDialogs
{
    internal class WorkTextPrompt : TextPrompt, IMenuItemDialog
    {
        private readonly PromptOptions _promptOptions;
        public string MenuItemOptionText { get; }
        public Func<ITurnContext, object, CancellationToken, Task> HandleResult { get; set; }

        public WorkTextPrompt(string dialogId, string menuItemOptionText = null,
            PromptValidator<string> validator = null, IEnumerable<string> suggestedActions = null,
            Func<ITurnContext, object, CancellationToken, Task> handleResult = null) : base(dialogId, validator)
        {
            _promptOptions = new PromptOptionsFactory()
                .CreateActionsPromptOptions(BotStrings.Select_your_work, suggestedActions);
            MenuItemOptionText = menuItemOptionText ?? dialogId;
            HandleResult = handleResult;
        }

        internal Task<DialogTurnResult> PromptAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return stepContext.PromptAsync(Id, _promptOptions, cancellationToken);
        }

        public override async Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.BeginDialogAsync(dc, options ?? _promptOptions, cancellationToken);
        }

        public Dialog GetDialog()
        {
            return this;
        }
    }
}