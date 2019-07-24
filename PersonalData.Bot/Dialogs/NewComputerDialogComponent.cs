using System;
using System.Threading;
using System.Threading.Tasks;
using HW.Bot.Dialogs.MenuDialog;
using HW.Bot.Dialogs.Steps;
using HW.Bot.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace HW.Bot.Dialogs
{
    internal class NewComputerDialogComponent : ComponentDialog, IMenuItemDialog
    {
        private IDbContext _dbContext;
        private IPersonStateManager _accessors;
        private const string NEW_USER_DIALOG = nameof(NewComputerDialogComponent) + "_" + nameof(NEW_USER_DIALOG);
        private const string MAIN_WATERFALL = nameof(NewComputerDialogComponent) + "_" + nameof(MAIN_WATERFALL);

        public NewComputerDialogComponent(string dialogId, IPersonStateManager accessors, IDbContext dbContext,
            string menuItemOptionText = null)
        :base(dialogId)
        {
            _dbContext = dbContext;
            _accessors = accessors;
            MenuItemOptionText = menuItemOptionText ?? dialogId;

            AddDialog(new WaterfallDialog(MAIN_WATERFALL)
                .AddStep(DecideIfNewOrExistUser.Step(_dbContext, NEW_USER_DIALOG))
                .AddStep(GetComputerRecommendations));

            AddDialog(new PersonalDataDialogComponent(NEW_USER_DIALOG, _accessors, _dbContext));
        }

        private Task<DialogTurnResult> GetComputerRecommendations(WaterfallStepContext stepcontext, CancellationToken cancellationtoken)
        {
            throw new NotImplementedException();
        }

        public string MenuItemOptionText { get; }
        public Func<ITurnContext, object, CancellationToken, Task> HandleResult { get; }
        public Dialog GetDialog()
        {
            return this;
        }
    }
}