using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HW.Bot.Dialogs.MenuDialog;
using HW.Bot.Dialogs.Steps;
using HW.Bot.Interfaces;
using HW.Bot.Resources;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;

namespace HW.Bot.Dialogs
{
    class ExistedComputerDialogComponent : ComponentDialog, IMenuItemDialog
    {
        private readonly IDbContext _dbContext;

        public ExistedComputerDialogComponent(string dialogId, IPersonStateManager personStateManager, IDbContext dbContext, string menuItemOptionText) : base(dialogId)
        {
            _dbContext = dbContext;
            MenuItemOptionText = menuItemOptionText ?? dialogId;
            AddDialog(new WaterfallDialog(MAIN_WATERFALL)
                .AddStep(DecideIfNewOrExistUser.Step(dbContext, PERSONAL_DATA_DIALOG))
                .AddStep(RequestForScanId)
                .AddStep(HandleScanIdResult));

            AddDialog(new PersonalDataDialogComponent(PERSONAL_DATA_DIALOG, personStateManager, dbContext));

            AddDialog(new TextPrompt(SCAN_ID_PROMPT, Validator));
        }

        private Task<bool> Validator(PromptValidatorContext<string> promptcontext, CancellationToken cancellationtoken)
        {
            var exitSynonyms = new[] {"exit", "done", "return"};
            switch (promptcontext.Recognized.Value)
            {
                case var s1 when exitSynonyms.Contains(s1.ToLower()):
                    promptcontext.Recognized.Value = "exit";
                    return Task.FromResult(true);

                case var s2 when Guid.TryParse(s2, out _):
                    return Task.FromResult(true);

                default:
                    return Task.FromResult(false);
            }
        }

        private async Task<DialogTurnResult> RequestForScanId(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(
                BotStrings.DownloadOurSoftware_windows_withoutLink + BotStrings.LinkToLateasSoftware_windows,
                cancellationToken: cancellationToken);

            return await stepContext.PromptAsync(SCAN_ID_PROMPT,
                new PromptOptions
                {
                    Prompt = MessageFactory.Text(BotStrings.ScanIdOrExit),
                    RetryPrompt = MessageFactory.Text(BotStrings.ScanIdExample)
                }, cancellationToken);
        }

        private async Task<DialogTurnResult> HandleScanIdResult(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            if (Guid.TryParse(stepContext.Result.ToString(), out var guid))
            {
                var recommendations = _dbContext.GetRecommendationsForScan(guid);
                if (recommendations == null)
                {
                    await stepContext.Context.SendActivityAsync(BotStrings.There_is_no_such_scan,
                        cancellationToken: cancellationToken);
                }
                else
                {
                    var isHave = false;
                    foreach (var recommendation in recommendations)
                    {
                        if (!isHave)
                        {
                            await stepContext.Context.SendActivityAsync(BotStrings.Here_our_recommendations_for_you,
                                cancellationToken: cancellationToken);
                            isHave = true;
                        }
                        await stepContext.Context.SendActivityAsync(recommendation,
                            cancellationToken: cancellationToken);
                    }

                    if (!isHave)
                    {
                        await stepContext.Context.SendActivityAsync(BotStrings.NoRecommendationsForYou,
                            cancellationToken: cancellationToken);
                        await stepContext.Context.SendActivityAsync(BotStrings.Github_issues,
                            cancellationToken: cancellationToken);
                    }
                }

            }

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        public const string SCAN_ID_PROMPT = nameof(ExistedComputerDialogComponent) + "_" + nameof(SCAN_ID_PROMPT);
        public const string PERSONAL_DATA_DIALOG = nameof(ExistedComputerDialogComponent) + "_" + nameof(PERSONAL_DATA_DIALOG);
        public const string EXIST_USER_MENU = nameof(ExistedComputerDialogComponent) + "_" + nameof(EXIST_USER_MENU);
        public const string NEW_USER_DIALOG  = nameof(ExistedComputerDialogComponent) + "_" + nameof(NEW_USER_DIALOG);
        public const string MAIN_WATERFALL = nameof(ExistedComputerDialogComponent) + "_" + nameof(MAIN_WATERFALL);
        public string MenuItemOptionText { get; }
        public Func<ITurnContext, object, CancellationToken, Task> HandleResult { get; }
        public Dialog GetDialog()
        {
            return this;
        }
    }
}
