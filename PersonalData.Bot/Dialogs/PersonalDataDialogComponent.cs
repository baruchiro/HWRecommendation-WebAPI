using HW.Bot.Dialogs.AtomicDialogs;
using HW.Bot.Extensions;
using HW.Bot.Interfaces;
using HW.Bot.Model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HW.Bot.Dialogs.MenuDialog;

namespace HW.Bot.Dialogs
{
    public class PersonalDataDialogComponent : ComponentDialog
    {
        private readonly EnumChoicePrompt<Gender> _genderPrompt = new EnumChoicePrompt<Gender>(GENDER_CHOICE_DIALOG);
        private readonly AgeNumberPrompt _agePrompt = new AgeNumberPrompt(AGE_NUMBER_DIALOG);
        private readonly WorkTextPrompt _workPrompt;

        private readonly IDbContext _dbContext;
        private const string NEW_USER_WATERFALL = nameof(PersonalDataDialogComponent) + nameof(WaterfallDialog) + "newuser";
        private const string EXIST_USER_WATERFALL = nameof(PersonalDataDialogComponent) + nameof(WaterfallDialog) + "existuser";
        private const string GENDER_CHOICE_DIALOG = "Gender";
        private const string AGE_NUMBER_DIALOG = "Age";
        private const string WORK_TEXT_DIALOG = "Work";
        private const string WELCOME_WATERFALL = nameof(PersonalDataDialogComponent) + nameof(WaterfallDialog) + "welcome";
        private const string dataID = nameof(PersonalDataDialogComponent) + "data";

        public PersonalDataDialogComponent(string dialogId, IDbContext dbContext) : base(dialogId)
        {
            _dbContext = dbContext;
            _workPrompt = new WorkTextPrompt(WORK_TEXT_DIALOG, suggestedActions: _dbContext.GetOrderedWorkList().Take(5));

            AddDialog(new WaterfallDialog(WELCOME_WATERFALL)
                .AddStep(DecideIfNewOrExistUserStep)
                .AddStep(SaveDetailsLoopStep)
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

            AddDialog(new MenuDialogComponent(EXIST_USER_WATERFALL, "Select which data you want to change",
                new Dictionary<Dialog, string>
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

            stepContext.Values[dataID] = personalInfo;
            return await stepContext.BeginDialogAsync(EXIST_USER_WATERFALL, personalInfo, cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> SaveDetailsLoopStep(WaterfallStepContext stepcontext,
            CancellationToken cancellationtoken)
        {
            var userDetails = stepcontext.Result as IPersonalData ??
                              throw new ArgumentNullException(nameof(stepcontext.Result), "dont have an IPersonalData");

            var channelId = stepcontext.Context.Activity.ChannelId;
            var userId = stepcontext.Context.Activity.From.Id;

            await stepcontext.Context.SendActivityAsync($"Saving information of {userId} from {channelId}", cancellationToken: cancellationtoken);

            if (_dbContext.SavePersonalDetails(channelId, userId, userDetails))
            {
                await stepcontext.Context.SendActivityAsync(
                    $"Saving your data:\n" +
                    $"Gender- {userDetails.Gender.GetDescription()}\n" +
                    $"Age- {userDetails.Age}\n" +
                    $"WorkArea- {userDetails.WorkArea}",
                    cancellationToken: cancellationtoken);
            }
            else
            {
                await stepcontext.Context.SendActivityAsync("We can't save your data", cancellationToken: cancellationtoken);
            }
            return await stepcontext.ReplaceDialogAsync(WELCOME_WATERFALL, cancellationToken: cancellationtoken);
        }

        private async Task<DialogTurnResult> HandlePersonalDataStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values[dataID] = stepContext.Options as IPersonalData ?? new PersonalData();
            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> HandleGenderStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (stepContext.Result is FoundChoice foundChoice)
            {
                var personalData = stepContext.Values[dataID] as IPersonalData ?? new PersonalData();
                Enum.TryParse<Gender>(foundChoice.Value, true, out var result);
                personalData.Gender = result;

                stepContext.Values[dataID] = personalData;
            }

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> HandleAgeStep(WaterfallStepContext stepcontext, CancellationToken cancellationtoken)
        {
            ((IPersonalData)stepcontext.Values[dataID]).Age = stepcontext.Result is int age ? age : -1;

            return await stepcontext.NextAsync(cancellationToken: cancellationtoken);
        }

        private async Task<DialogTurnResult> HandleWorkStep(WaterfallStepContext stepcontext, CancellationToken cancellationtoken)
        {
            ((IPersonalData)stepcontext.Values[dataID]).WorkArea = stepcontext.Result as string;

            return await stepcontext.NextAsync(cancellationToken: cancellationtoken);
        }

        private async Task<DialogTurnResult> EndWaterfallStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.NextAsync(((IPersonalData)stepContext.Values[dataID]), cancellationToken);
        }
    }
}