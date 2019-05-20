using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HW.Bot.Dialogs;
using HW.Bot.Dialogs.MenuDialog;
using HW.Bot.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace HW.Bot
{
    internal class RecommendationBot : IBot
    {
        private readonly StateManager _accessors;
        private readonly DialogSet _dialogSet;

        private readonly ICollection<IMenuItemDialog> _menuDialogs = new List<IMenuItemDialog>();

        private const string MENU_DIALOG = "menu";

        public RecommendationBot(StateManager accessors, IDbContext dbContext)
        {
            _accessors = accessors ?? throw new ArgumentNullException(nameof(accessors));

            _menuDialogs.Add(
                new PersonalDataDialogComponent("recommendations", _accessors, dbContext,
                    "Get hardware recommendations for your current computer"));


            _dialogSet = new DialogSet(accessors.ConversationDataAccessor);

            _dialogSet.Add(new MenuDialogComponent(MENU_DIALOG, "Hi, select what you want", _menuDialogs, doneTitle: null));
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
    }
}
