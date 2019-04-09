using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HW.Bot.Dialogs.MenuDialog;
using HW.Bot.Extensions;
using HW.Bot.Factories;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;

namespace HW.Bot.Dialogs.AtomicDialogs
{
    internal class EnumChoicePrompt<TEnum> : ChoicePrompt, IMenuItemDialog
        where TEnum : struct
    {
        private readonly PromptOptions _promptOptions;
        public Func<ITurnContext, object, CancellationToken, Task> HandleResult { get; set; }

        public EnumChoicePrompt(string dialogId, PromptValidator<FoundChoice> validator = null, string defaultLocale = null) : base(dialogId, validator, defaultLocale)
        {
            _promptOptions = new PromptOptionsFactory()
                .CreateChoicesPromptOptions("Select your gender",
                typeof(TEnum).GetEnumValues().Cast<TEnum>().ToDictionary(g => g.ToString(), g => g.GetDescription()));
        }

        internal async Task<DialogTurnResult> PromptAsync(WaterfallStepContext stepcontext, CancellationToken cancellationtoken)
        {
            return await stepcontext.PromptAsync(Id, _promptOptions, cancellationtoken);
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
