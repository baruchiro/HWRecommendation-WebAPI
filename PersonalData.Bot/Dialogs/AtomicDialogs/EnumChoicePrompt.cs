using HW.Bot.Extensions;
using HW.Bot.Factories;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HW.Bot.Dialogs.AtomicDialogs
{
    class EnumChoicePrompt<ENUM> : ChoicePrompt
        where ENUM : struct
    {
        private readonly PromptOptions promptOptions;

        public EnumChoicePrompt(string dialogId, PromptValidator<FoundChoice> validator = null, string defaultLocale = null) : base(dialogId, validator, defaultLocale)
        {
            this.promptOptions = new PromptOptionsFactory()
                .CreatePromptOptions("Select your gender",
                typeof(ENUM).GetEnumValues().Cast<ENUM>().ToDictionary(g => g.ToString(), g => g.GetDescription()));
        }

        internal Task<DialogTurnResult> PromptAsync(WaterfallStepContext stepcontext, CancellationToken cancellationtoken)
        {
            return stepcontext.PromptAsync(Id, promptOptions, cancellationtoken);
        }
    }
}
