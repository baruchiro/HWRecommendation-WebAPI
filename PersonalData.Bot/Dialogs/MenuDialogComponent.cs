using HW.Bot.Factories;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HW.Bot.Dialogs
{
    class MenuDialogComponent : ComponentDialog
    {
        private readonly IDictionary<Dialog, string> _dialogsAndTitles;
        private readonly string title;
        private readonly Dictionary<string, string> menuDialogKeyToTitle;

        private const string WATERFALL_DIALOG = nameof(MenuDialogComponent) + "waterfall";
        private const string CHOICE_DIALOG = nameof(MenuDialogComponent) + "choice";


        public MenuDialogComponent(string dialogId, string title, IDictionary<Dialog, string> dialogsAndTitles) : base(dialogId)
        {
            this._dialogsAndTitles = dialogsAndTitles;
            this.title = title;
            this.menuDialogKeyToTitle = this._dialogsAndTitles.ToDictionary(d => d.Key.Id, d => d.Value);

            AddDialog(new WaterfallDialog(WATERFALL_DIALOG)
                .AddStep(MenuStepAsync)
                .AddStep(HandleChoiceAsync)
                .AddStep(MenuLoopAsync)
            );

            foreach (var d in this._dialogsAndTitles.Keys)
            {
                AddDialog(d);
            }
            AddDialog(new ChoicePrompt(CHOICE_DIALOG));
        }

        private async Task<DialogTurnResult> MenuStepAsync(WaterfallStepContext stepcontext, CancellationToken cancellationtoken)
        {
            var options =
                new PromptOptionsFactory()
                .CreateChoicesPromptOptions(title, menuDialogKeyToTitle);

            return await stepcontext.PromptAsync(CHOICE_DIALOG, options, cancellationtoken);
        }

        private async Task<DialogTurnResult> HandleChoiceAsync(WaterfallStepContext stepcontext,
            CancellationToken cancellationtoken)
        {
            switch (stepcontext.Result)
            {
                case FoundChoice result
                    when menuDialogKeyToTitle.Keys.Contains(result.Value):
                    return await stepcontext.BeginDialogAsync(result.Value, cancellationToken: cancellationtoken);

                default:
                    return await stepcontext.EndDialogAsync(cancellationToken: cancellationtoken);
            }
        }

        private async Task<DialogTurnResult> MenuLoopAsync(WaterfallStepContext stepcontext,
            CancellationToken cancellationtoken)
        {
            return await stepcontext.ReplaceDialogAsync(WATERFALL_DIALOG, cancellationToken: cancellationtoken);
        }
    }
}
