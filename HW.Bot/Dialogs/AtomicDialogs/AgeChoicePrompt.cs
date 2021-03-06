﻿using HW.Bot.Dialogs.MenuDialog;
using HW.Bot.Resources;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HW.Bot.Dialogs.AtomicDialogs
{
    internal class AgeNumberPrompt : NumberPrompt<int>, IMenuItemDialog
    {
        private readonly PromptOptions _promptOptions;
        public string MenuItemOptionText { get; }
        public string Message { get; }
        public Func<ITurnContext, object, CancellationToken, Task> HandleResult { get; set; }

        public AgeNumberPrompt(string dialogId, string menuItemOptionText = null, PromptValidator<int> validator = null, string defaultLocale = null) : base(dialogId, validator, defaultLocale)
        {
            _promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text(BotStrings.Enter_your_age),
                RetryPrompt = MessageFactory.Text(BotStrings.Enter_your_age_Repeat)
            };
            MenuItemOptionText = menuItemOptionText ?? dialogId;
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
