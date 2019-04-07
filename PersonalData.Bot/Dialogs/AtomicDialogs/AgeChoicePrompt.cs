using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HW.Bot.Dialogs.AtomicDialogs
{
    class AgeNumberPrompt : NumberPrompt<int>
    {
        private readonly string dialogId;
        private readonly PromptOptions promptOptions;

        public AgeNumberPrompt(string dialogId, PromptValidator<int> validator = null, string defaultLocale = null) : base(dialogId, validator, defaultLocale)
        {
            this.dialogId = dialogId;
            this.promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("Enter your age"),
                RetryPrompt = MessageFactory.Text("Please, Enter your age!!")
            };
        }

        internal Task<DialogTurnResult> PromptAsync(WaterfallStepContext stepcontext, CancellationToken cancellationtoken)
        {
            return stepcontext.PromptAsync(this.dialogId, promptOptions, cancellationtoken);
        }
    }
}
