using HW.Bot.Dialogs;
using HW.Bot.Factories;
using HW.Bot.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HW.Bot
{

    public class RecommendationBot : IBot
    {
        private readonly StateManager _accessors;
        private readonly DialogSet _dialogSet;

        private readonly IDictionary<string, string> menuDialogKeyToTitle =
            new Dictionary<string, string>
            {
                {DETAILS_DIALOG, "Get hardware recommendations for your current computer"}
            };

        private const string DETAILS_DIALOG = "recommendations";
        private const string MENU_DIALOG = "menu";
        private const string CHOICE_DIALOG = "choice";

        public RecommendationBot(StateManager accessors, IDbContext dbContext)
        {
            _accessors = accessors ?? throw new ArgumentNullException(nameof(accessors));

            _dialogSet = new DialogSet(accessors.ConversationDataAccessor);

            _dialogSet.Add(new WaterfallDialog(MENU_DIALOG)
                .AddStep(MenuStepAsync)
                .AddStep(HandleChoiceAsync)
                .AddStep(MenuLoopAsync)
            );

            _dialogSet.Add(new ChoicePrompt(CHOICE_DIALOG));

            _dialogSet.Add(new PersonalDataDialogComponent(DETAILS_DIALOG, dbContext));

        }

        public async Task OnTurnAsync(ITurnContext turnContext,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                var dialogContext = await _dialogSet.CreateContextAsync(turnContext, cancellationToken);
                var results = await dialogContext.ContinueDialogAsync(cancellationToken);

                // If the DialogTurnStatus is Empty we should start a new dialog.
                if (results.Status == DialogTurnStatus.Empty)
                {
                    await dialogContext.BeginDialogAsync(MENU_DIALOG, cancellationToken: cancellationToken);
                }

                await _accessors.ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            }
            else
            {
                await turnContext.SendActivityAsync($"{turnContext.Activity.Type} event detected",
                    cancellationToken: cancellationToken);
            }
        }

        private async Task<DialogTurnResult> MenuStepAsync(WaterfallStepContext stepcontext, CancellationToken cancellationtoken)
        {
            var options =
                new PromptOptionsFactory()
                .CreatePromptOptions("Hi, select what you want", menuDialogKeyToTitle);

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
            return await stepcontext.ReplaceDialogAsync(MENU_DIALOG, cancellationToken: cancellationtoken);
        }
    }
}
