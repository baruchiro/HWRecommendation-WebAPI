using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HW.Bot.Dialogs.MenuDialog;
using HW.Bot.Resources;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;

namespace HW.Bot.Dialogs.AtomicDialogs
{
    class ListChoicePrompt : ChoicePrompt, IMenuItemDialog
    {
        private readonly PromptOptions _promptOptions;
        public string MenuItemOptionText { get; }
        public Func<ITurnContext, object, CancellationToken, Task> HandleResult { get; set; }

        public ListChoicePrompt([Localizable(false)] string dialogId, string text,
            IList<string> choices,
            string menuItemOptionText = null, PromptValidator<FoundChoice> validator = null, string defaultLocale = null, Func<ITurnContext, object, CancellationToken, Task> handleResult = null) : base(dialogId, validator, defaultLocale)
        {
            _promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text(text),
                Choices = ChoiceFactory.ToChoices(choices)
            };
            MenuItemOptionText = menuItemOptionText ?? dialogId;
            HandleResult = handleResult;
        }

        internal async Task<DialogTurnResult> PromptAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync(Id, _promptOptions, cancellationToken);
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
