using System;
using System.Threading;
using System.Threading.Tasks;
using HW.Bot.Dialogs.MenuDialog;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace HW.Bot.Dialogs.AtomicDialogs
{
    internal class AgeNumberPrompt : NumberPrompt<int>, IMenuItemDialog
    {
        private readonly PromptOptions _promptOptions;
        private string _title;
        public Func<ITurnContext, object, CancellationToken, Task> HandleResult { get; set; }

        public AgeNumberPrompt(string dialogId, string title = null, PromptValidator<int> validator = null, string defaultLocale = null) : base(dialogId, validator, defaultLocale)
        {
            _promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("Enter your age"),
                RetryPrompt = MessageFactory.Text("Please, Enter your age!!")
            };
            _title = title ?? dialogId;
        }

        internal Task<DialogTurnResult> PromptAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return stepContext.PromptAsync(Id, _promptOptions, cancellationToken);
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
