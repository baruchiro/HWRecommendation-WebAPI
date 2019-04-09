using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HW.Bot.Dialogs.AtomicDialogs;
using HW.Bot.Dialogs.MenuDialog;
using HW.Bot.Extensions;
using HW.Bot.Interfaces;
using HW.Bot.Model;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;

namespace HW.Bot.Dialogs
{
    internal class PersonalDataDialogComponent : ComponentDialog, IMenuItemDialog
    {
        private readonly EnumChoicePrompt<Gender> _genderPrompt = new EnumChoicePrompt<Gender>(GENDER_CHOICE_DIALOG);
        private readonly AgeNumberPrompt _agePrompt = new AgeNumberPrompt(AGE_NUMBER_DIALOG);
        private readonly WorkTextPrompt _workPrompt;

        private readonly IDbContext _dbContext;
        private readonly IPersonalDataStateManager _personalDataStateManager;
        private const string NEW_USER_WATERFALL = nameof(PersonalDataDialogComponent) + nameof(WaterfallDialog) + "newuser";
        private const string EXIST_USER_MENU = nameof(PersonalDataDialogComponent) + nameof(WaterfallDialog) + "existuser";
        private const string GENDER_CHOICE_DIALOG = "Gender";
        private const string AGE_NUMBER_DIALOG = "Age";
        private const string WORK_TEXT_DIALOG = "Work";
        private const string WELCOME_WATERFALL = nameof(PersonalDataDialogComponent) + nameof(WaterfallDialog) + "welcome";
        private const string DATA_ID = nameof(PersonalDataDialogComponent) + "data";

        public Func<ITurnContext, object, CancellationToken, Task> HandleResult { get; set; }

        public PersonalDataDialogComponent(string dialogId, IPersonalDataStateManager personalDataStateManager, IDbContext dbContext) : base(dialogId)
        {
            this._personalDataStateManager = personalDataStateManager;
            _dbContext = dbContext;
            _workPrompt = new WorkTextPrompt(WORK_TEXT_DIALOG, suggestedActions: _dbContext.GetOrderedWorkList().Take(5))
            {
                HandleResult = HandleWork
            };
            _agePrompt.HandleResult = HandleAge;
            _genderPrompt.HandleResult = HandleGender;

            AddDialog(new WaterfallDialog(WELCOME_WATERFALL)
                .AddStep(DecideIfNewOrExistUserStep)
                .AddStep(SaveDetailsStep)
            );

            AddDialog(new WaterfallDialog(NEW_USER_WATERFALL)
                .AddStep(HandlePersonalDataStep)
                .AddStep(_genderPrompt.PromptAsync)
                .AddStep(HandleGenderStep)
                .AddStep(_agePrompt.PromptAsync)
                .AddStep(HandleAgeStep)
                .AddStep(_workPrompt.PromptAsync)
                .AddStep(HandleWorkStep)
                .AddStep(EndWaterfallStep)
            );

            AddDialog(new MenuDialogComponent(EXIST_USER_MENU, "Select which data you want to change",
                new Dictionary<IMenuItemDialog, string>
                {
                    {_genderPrompt, "Change your Gender"},
                    {_agePrompt, "Change your age"},
                    {_workPrompt, "Change your work"}
                }));

            AddDialog(_genderPrompt);
            AddDialog(_agePrompt);
            AddDialog(_workPrompt);
        }

        private async Task<DialogTurnResult> DecideIfNewOrExistUserStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var channelId = stepContext.Context.Activity.ChannelId;
            var userId = stepContext.Context.Activity.From.Id;

            var personalInfo = _dbContext.GetPersonalDetails(channelId, userId);

            if (personalInfo == null)
            {
                await stepContext.Context.SendActivityAsync(
                    $"We see there is no information about {userId} from {channelId}.\n" +
                    "We need some information of you to give you a personal results",
                    cancellationToken: cancellationToken);

                return await stepContext.BeginDialogAsync(NEW_USER_WATERFALL, new PersonalData(), cancellationToken: cancellationToken);
            }

            stepContext.Values[DATA_ID] = personalInfo;
            return await stepContext.BeginDialogAsync(EXIST_USER_MENU, personalInfo, cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> SaveDetailsStep(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            var personalData = await _personalDataStateManager.GetPersonalDataAsync(stepContext.Context, cancellationToken);

            var channelId = stepContext.Context.Activity.ChannelId;
            var userId = stepContext.Context.Activity.From.Id;

            await stepContext.Context.SendActivityAsync($"Saving information of {userId} from {channelId}", cancellationToken: cancellationToken);

            if (_dbContext.SavePersonalDetails(channelId, userId, personalData))
            {
                await stepContext.Context.SendActivityAsync(
                    "Saving your data:\n" +
                    $"Gender- {personalData.Gender.GetDescription()}\n" +
                    $"Age- {personalData.Age}\n" +
                    $"WorkArea- {personalData.WorkArea}",
                    cancellationToken: cancellationToken);
            }
            else
            {
                await stepContext.Context.SendActivityAsync("We can't save your data", cancellationToken: cancellationToken);
            }

            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> HandlePersonalDataStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values[DATA_ID] = stepContext.Options as IPersonalData ?? new PersonalData();
            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> HandleGenderStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (stepContext.Result is FoundChoice foundChoice)
            {
                var personalData = stepContext.Values[DATA_ID] as IPersonalData ?? new PersonalData();
                Enum.TryParse<Gender>(foundChoice.Value, true, out var result);
                personalData.Gender = result;

                stepContext.Values[DATA_ID] = personalData;
            }

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> HandleAgeStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            ((IPersonalData)stepContext.Values[DATA_ID]).Age = stepContext.Result is int age ? age : -1;

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> HandleWorkStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            ((IPersonalData)stepContext.Values[DATA_ID]).WorkArea = stepContext.Result as string;

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> EndWaterfallStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (stepContext.Values[DATA_ID] is IPersonalData personalData)
            {
                await _personalDataStateManager.SavePersonalDataAsync(stepContext.Context, personalData, cancellationToken);
            }
            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        public Dialog GetDialog()
        {
            return this;
        }

        private async Task HandleWork(ITurnContext turnContext, object result, CancellationToken cancellationToken)
        {
            await _personalDataStateManager.UpdatePersonalDataAsync(turnContext,
                personalData =>
                {
                    if (result is string work)
                        personalData.WorkArea = work;
                }, cancellationToken);
        }

        private async Task HandleAge(ITurnContext turnContext, object result, CancellationToken cancellationToken)
        {
            await _personalDataStateManager.UpdatePersonalDataAsync(turnContext,
                personalData =>
                {
                    if (result is int age)
                        personalData.Age = age;
                }, cancellationToken);
        }

        private async Task HandleGender(ITurnContext turnContext, object result, CancellationToken cancellationToken)
        {
            await _personalDataStateManager.UpdatePersonalDataAsync(turnContext,
                personalData =>
                {
                    if (!(result is FoundChoice foundChoice)) return;
                    Enum.TryParse<Gender>(foundChoice.Value, true, out var gender);
                    personalData.Gender = gender;
                }, cancellationToken);
        }
    }
}