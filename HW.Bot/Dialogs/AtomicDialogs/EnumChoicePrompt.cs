using HW.Bot.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using System.Linq;

namespace HW.Bot.Dialogs.AtomicDialogs
{
    internal class EnumChoicePrompt<TEnum> : ListChoicePrompt
        where TEnum : struct
    {
        public EnumChoicePrompt(string dialogId, string text, string menuItemOptionText = null, PromptValidator<FoundChoice> validator = null,
            string defaultLocale = null) :
            base(dialogId, text,
                typeof(TEnum).GetEnumValues().Cast<TEnum>().Select(g => g.GetDescription()).ToArray(),
                menuItemOptionText, validator, defaultLocale)
        {
        }
    }
}
