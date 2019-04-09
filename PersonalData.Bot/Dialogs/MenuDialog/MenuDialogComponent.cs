using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HW.Bot.Factories;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;

namespace HW.Bot.Dialogs.MenuDialog
{
    internal class MenuDialogComponent : ComponentDialog
    {
        private readonly IDictionary<IMenuItemDialog, string> _dialogsAndTitles;
        private readonly string _title;
        private readonly Dictionary<string, string> _menuDialogKeyToTitle;

        private const string SELECTED_DIALOG = nameof(MenuDialogComponent) + "selectedDialog";
        private const string WATERFALL_DIALOG = nameof(MenuDialogComponent) + "waterfall";
        private const string CHOICE_DIALOG = nameof(MenuDialogComponent) + "choice";


        public MenuDialogComponent(string dialogId, string title, IDictionary<IMenuItemDialog, string> dialogsAndTitles, string doneTitle = "Done") : base(dialogId)
        {
            _dialogsAndTitles = dialogsAndTitles;
            this._title = title;
            _menuDialogKeyToTitle = _dialogsAndTitles.ToDictionary(d => d.Key.Id, d => d.Value);
            if (!string.IsNullOrEmpty(doneTitle))
            {
                _menuDialogKeyToTitle.Add(doneTitle, doneTitle);
            }

            AddDialog(new WaterfallDialog(WATERFALL_DIALOG)
                .AddStep(MenuStepAsync)
                .AddStep(HandleChoiceAsync)
                .AddStep(MenuLoopAsync)
            );

            foreach (var d in _dialogsAndTitles.Keys)
            {
                AddDialog(d.GetDialog());
            }

            AddDialog(new ChoicePrompt(CHOICE_DIALOG));
        }

        private async Task<DialogTurnResult> MenuStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var options =
                new PromptOptionsFactory()
                .CreateChoicesPromptOptions(_title, _menuDialogKeyToTitle);

            return await stepContext.PromptAsync(CHOICE_DIALOG, options, cancellationToken);
        }

        private async Task<DialogTurnResult> HandleChoiceAsync(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            switch (stepContext.Result)
            {
                case FoundChoice result
                    when _dialogsAndTitles.Keys.Select(d => d.Id).Contains(result.Value):

                    stepContext.Values[SELECTED_DIALOG] = result.Value;
                    return await stepContext.BeginDialogAsync(result.Value, cancellationToken: cancellationToken);

                default:
                    return await stepContext.EndDialogAsync(stepContext.Values, cancellationToken: cancellationToken);
            }
        }

        private async Task<DialogTurnResult> MenuLoopAsync(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            _dialogsAndTitles.Keys.First(d => d.Id.Equals(stepContext.Values[SELECTED_DIALOG]))
                .HandleResult?.Invoke(stepContext.Context, stepContext.Result, cancellationToken);
            return await stepContext.ReplaceDialogAsync(WATERFALL_DIALOG, cancellationToken: cancellationToken);
        }
    }
}
