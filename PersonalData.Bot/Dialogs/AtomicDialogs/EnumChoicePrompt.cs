using HW.Bot.Extensions;
using HW.Bot.Factories;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;

namespace HW.Bot.Dialogs.AtomicDialogs
{
    class EnumChoicePrompt<ENUM> : ChoicePrompt
        where ENUM : struct
    {
        private readonly PromptOptions promptOptions;

        public EnumChoicePrompt(string dialogId, PromptValidator<FoundChoice> validator = null, string defaultLocale = null) : base(dialogId, validator, defaultLocale)
        {
            
            this.promptOptions = new PromptOptionsFactory()
                .CreateChoicesPromptOptions("Select your gender",
                typeof(ENUM).GetEnumValues().Cast<ENUM>().ToDictionary(g => g.ToString(), g => g.GetDescription()));
        }

        internal async Task<DialogTurnResult> PromptAsync(WaterfallStepContext stepcontext, CancellationToken cancellationtoken)
        {
            return await stepcontext.PromptAsync(Id, promptOptions, cancellationtoken);
        }

        public override async Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.BeginDialogAsync(dc, options ?? promptOptions, cancellationToken);
        }
    }
}
