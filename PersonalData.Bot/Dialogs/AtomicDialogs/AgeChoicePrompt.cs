using System;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using System.Threading.Tasks;
using HW.Bot.Dialogs.MenuDialog;

namespace HW.Bot.Dialogs.AtomicDialogs
{
    class AgeNumberPrompt : NumberPrompt<int>, IMenuItemDialog
    {
        private readonly PromptOptions promptOptions;
        public Func<ITurnContext, object, CancellationToken, Task> HandleResult { get; set; }

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

        public Dialog GetDialog()
        {
            return this;
        }
    }
}
