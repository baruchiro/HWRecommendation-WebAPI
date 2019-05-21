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
    internal class EnumChoicePrompt<TEnum> : ListChoicePrompt
        where TEnum : struct
    {
        public EnumChoicePrompt(string dialogId, string title = null, PromptValidator<FoundChoice> validator = null,
            string defaultLocale = null) :
            base(dialogId,
                typeof(TEnum).GetEnumValues().Cast<TEnum>().Select(g => g.GetDescription()).ToArray(),
                title, validator, defaultLocale)
        {
        }
    }
}
