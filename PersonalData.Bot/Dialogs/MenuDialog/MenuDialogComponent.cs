using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HW.Bot.Factories;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;

namespace HW.Bot.Dialogs.MenuDialog
{
    internal class MenuDialogComponent : ComponentDialog
    {
        private readonly string _title;
        private readonly string _doneTitle;
        private readonly Dictionary<string, Dialog> _mapTitleToDialog;
        private readonly Dictionary<string, IMenuItemDialog> _mapDialogIdToMenuItem;
        private readonly ICollection<string> _menuItemTitles;
        
        private const string SELECTED_DIALOG_ID = nameof(MenuDialogComponent) + "_selectedDialog";
        private const string WATERFALL_DIALOG = nameof(MenuDialogComponent) + "_waterfall";
        private const string CHOICE_DIALOG = nameof(MenuDialogComponent) + "_choice";


        public MenuDialogComponent(string dialogId, string title, ICollection<IMenuItemDialog> dialogsMenuItems, string doneTitle = "Done") : base(dialogId)
        {
            _title = title;
            _doneTitle = doneTitle;
            _mapTitleToDialog = dialogsMenuItems.ToDictionary(d => d.GetTitle(), d => d.GetDialog());
            _mapDialogIdToMenuItem = dialogsMenuItems.ToDictionary(d => d.Id, d => d);

            _menuItemTitles = dialogsMenuItems.Select(d => d.GetTitle()).ToList();

            if (_doneTitle != null)
            {
                _menuItemTitles.Add(_doneTitle);
            }

            AddDialog(new WaterfallDialog(WATERFALL_DIALOG)
                .AddStep(MenuStepAsync)
                .AddStep(HandleChoiceAsync)
                .AddStep(MenuLoopAsync)
            );

            foreach (var d in dialogsMenuItems)
            {
                AddDialog(d.GetDialog());
            }

            AddDialog(new ChoicePrompt(CHOICE_DIALOG));
        }

        private async Task<DialogTurnResult> MenuStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var options = new PromptOptions
            {
                Prompt = MessageFactory.Text(_title),
                Choices = ChoiceFactory.ToChoices(_menuItemTitles.ToArray())
            };

            return await stepContext.PromptAsync(CHOICE_DIALOG, options, cancellationToken);
        }

        private async Task<DialogTurnResult> HandleChoiceAsync(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            switch (stepContext.Result)
            {
                case FoundChoice result
                    when _mapTitleToDialog.Keys.Contains(result.Value):

                    var selectedDialogId = _mapTitleToDialog[result.Value].Id;
                    stepContext.Values[SELECTED_DIALOG_ID] = selectedDialogId;
                    return await stepContext.BeginDialogAsync(selectedDialogId, cancellationToken: cancellationToken);

                case FoundChoice result
                    when result.Value.Equals(_doneTitle):
                    return await stepContext.EndDialogAsync(stepContext.Values, cancellationToken: cancellationToken);

                default:
                    await stepContext.Context.SendActivityAsync("Can't find what you have selected.",
                        cancellationToken: cancellationToken);
                    return await stepContext.ReplaceDialogAsync(WATERFALL_DIALOG, cancellationToken: cancellationToken);
            }
        }

        private async Task<DialogTurnResult> MenuLoopAsync(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            _mapDialogIdToMenuItem[stepContext.Values[SELECTED_DIALOG_ID].ToString()]
                .HandleResult?.Invoke(stepContext.Context, stepContext.Result, cancellationToken);
            return await stepContext.ReplaceDialogAsync(WATERFALL_DIALOG, cancellationToken: cancellationToken);
        }
    }
}
