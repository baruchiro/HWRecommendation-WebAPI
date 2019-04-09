using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HW.Bot.Factories;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;

namespace HW.Bot.Dialogs.MenuDialog
{
    class MenuDialogComponent : ComponentDialog
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

        private async Task<DialogTurnResult> MenuStepAsync(WaterfallStepContext stepcontext, CancellationToken cancellationtoken)
        {
            var options =
                new PromptOptionsFactory()
                .CreateChoicesPromptOptions(_title, _menuDialogKeyToTitle);

            return await stepcontext.PromptAsync(CHOICE_DIALOG, options, cancellationtoken);
        }

        private async Task<DialogTurnResult> HandleChoiceAsync(WaterfallStepContext stepcontext,
            CancellationToken cancellationtoken)
        {
            switch (stepcontext.Result)
            {
                case FoundChoice result
                    when _dialogsAndTitles.Keys.Select(d => d.Id).Contains(result.Value):

                    stepcontext.Values[SELECTED_DIALOG] = result.Value;
                    return await stepcontext.BeginDialogAsync(result.Value, cancellationToken: cancellationtoken);

                default:
                    return await stepcontext.EndDialogAsync(stepcontext.Values, cancellationToken: cancellationtoken);
            }
        }

        private async Task<DialogTurnResult> MenuLoopAsync(WaterfallStepContext stepcontext,
            CancellationToken cancellationtoken)
        {
            _dialogsAndTitles.Keys.First(d => d.Id.Equals(stepcontext.Values[SELECTED_DIALOG]))
                .HandleResult?.Invoke(stepcontext.Context, stepcontext.Result, cancellationtoken);
            return await stepcontext.ReplaceDialogAsync(WATERFALL_DIALOG, cancellationToken: cancellationtoken);
        }
    }
}
