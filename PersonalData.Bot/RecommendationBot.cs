using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using PersonalData.Bot.Dialogs;
using PersonalData.Bot.Interfaces;

namespace PersonalData.Bot
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
                .AddStep(HandleResultAsync)
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
            var options = new PromptOptions
            {
                Prompt = MessageFactory.Text("Hi, select what you want"),
                Choices = menuDialogKeyToTitle.Select(v=>
                    new Choice(v.Key)
                    {
                        Action = new CardAction(ActionTypes.ImBack, v.Value, value:v.Key)
                    })
                    .ToList()
            };
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

                case 1:
                    return await stepcontext.BeginDialogAsync(DETAILS_DIALOG, cancellationToken: cancellationtoken);
                default:
                    return await stepcontext.EndDialogAsync(cancellationToken: cancellationtoken);
            }
        }

        private Task<DialogTurnResult> HandleResultAsync(WaterfallStepContext stepcontext, CancellationToken cancellationtoken)
        {
            throw new NotImplementedException();
        }
    }
}
