using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using System.Threading.Tasks;

namespace HW.Bot.Dialogs.AtomicDialogs
{
    class AgeNumberPrompt : NumberPrompt<int>
    {
        private readonly PromptOptions promptOptions;

        public AgeNumberPrompt(string dialogId, PromptValidator<int> validator = null, string defaultLocale = null) : base(dialogId, validator, defaultLocale)
        {
            this.promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("Enter your age"),
                RetryPrompt = MessageFactory.Text("Please, Enter your age!!")
            };
        }

        internal Task<DialogTurnResult> PromptAsync(WaterfallStepContext stepcontext, CancellationToken cancellationtoken)
        {
            return stepcontext.PromptAsync(Id, promptOptions, cancellationtoken);
        }

        public override async Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.BeginDialogAsync(dc, options ?? promptOptions, cancellationToken);
        }
    }
}
