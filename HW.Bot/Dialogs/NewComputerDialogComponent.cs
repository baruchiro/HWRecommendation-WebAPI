using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HW.Bot.Dialogs.MenuDialog;
using HW.Bot.Dialogs.Steps;
using HW.Bot.Interfaces;
using HW.Bot.Resources;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace HW.Bot.Dialogs
{
    [Localizable(true)]
    internal class NewComputerDialogComponent : ComponentDialog, IMenuItemDialog
    {
        private IDbContext _dbContext;
        private IPersonStateManager _accessors;
        private readonly IRecommender _recommender;
        private const string NEW_USER_DIALOG = nameof(NewComputerDialogComponent) + "_" + nameof(NEW_USER_DIALOG);
        private const string MAIN_WATERFALL = nameof(NewComputerDialogComponent) + "_" + nameof(MAIN_WATERFALL);

        public NewComputerDialogComponent(string dialogId, IPersonStateManager accessors, IDbContext dbContext,
            IRecommender recommender,
            string menuItemOptionText = null)
        :base(dialogId)
        {
            _dbContext = dbContext;
            _accessors = accessors;
            _recommender = recommender;
            MenuItemOptionText = menuItemOptionText ?? dialogId;

            AddDialog(new WaterfallDialog(MAIN_WATERFALL)
                .AddStep(DecideIfNewOrExistUser.Step(_dbContext, NEW_USER_DIALOG))
                .AddStep(GetComputerRecommendations));

            AddDialog(new PersonalDataDialogComponent(NEW_USER_DIALOG, _accessors, _dbContext));
        }

        private async Task<DialogTurnResult> GetComputerRecommendations(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(
                BotStrings.We_taking_personal_info_retrieve_recommendations,
                cancellationToken: cancellationToken);

            var person = _accessors.GetPersonAsync(stepContext.Context, cancellationToken);

            if (_recommender.IsReadyToGiveRecommendation())
            {
                _recommender.GetNewComputerRecommendations(await person)
                    .AsParallel()
                    .ForAll(async r =>
                        await stepContext.Context.SendActivityAsync(r.RecommendMessage(),
                            cancellationToken: cancellationToken));
            }
            else
            {
                await stepContext.Context.SendActivityAsync(BotStrings.No_Recommendation_engine,
                    cancellationToken: cancellationToken);
            }

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        public string MenuItemOptionText { get; }
        public Func<ITurnContext, object, CancellationToken, Task> HandleResult { get; }
        public Dialog GetDialog()
        {
            return this;
        }
    }
}