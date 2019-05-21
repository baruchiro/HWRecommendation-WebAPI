using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HW.Bot.Dialogs.MenuDialog;
using HW.Bot.Extensions;
using HW.Bot.Factories;
using HW.Bot.Resources;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;

namespace HW.Bot.Dialogs.AtomicDialogs
{
    internal class EnumChoicePrompt<TEnum> : ChoicePrompt, IMenuItemDialog
        where TEnum : struct
    {
        private readonly PromptOptions _promptOptions;
        private string _title;
        public Func<ITurnContext, object, CancellationToken, Task> HandleResult { get; set; }

        public EnumChoicePrompt(string dialogId, string title = null, PromptValidator<FoundChoice> validator = null, string defaultLocale = null) : base(dialogId, validator, defaultLocale)
        {
            _promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text(BotStrings.Select_your_gender),
                Choices = ChoiceFactory.ToChoices(typeof(TEnum).GetEnumValues().Cast<TEnum>()
                    .Select(g => g.GetDescription()).ToArray())
            };
            _title = title ?? dialogId;
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

        public string GetTitle()
        {
            return _title;
        }
    }
}
