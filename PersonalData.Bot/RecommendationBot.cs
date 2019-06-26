using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HW.Bot.Dialogs;
using HW.Bot.Dialogs.MenuDialog;
using HW.Bot.Interfaces;
using HW.Bot.Resources;
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
                new PersonalDataDialogComponent(PERSONAL_DATA_DIALOG, _accessors, dbContext,
                    BotStrings.Manage_your_personal_information));
            _menuDialogs.Add(
                new ExistedComputerDialogComponent(EXISTED_COMPUTER_DIALOG, _accessors, dbContext,
                    BotStrings.RecommendationMenuItemTitle));


            _dialogSet = new DialogSet(accessors.ConversationDataAccessor);

            _dialogSet.Add(
                new MenuDialogComponent(MENU_DIALOG, BotStrings.MainMenuTitle, _menuDialogs, doneTitle: null));
        }

        public const string PERSONAL_DATA_DIALOG = nameof(RecommendationBot) + "_" + nameof(PERSONAL_DATA_DIALOG);
        public const string EXISTED_COMPUTER_DIALOG = nameof(RecommendationBot) + "_" + nameof(EXISTED_COMPUTER_DIALOG);

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
                await turnContext.SendActivityAsync(string.Format(BotStrings.event_detected, turnContext.Activity.Type),
                    cancellationToken: cancellationToken);
            }
        }
    }
}
