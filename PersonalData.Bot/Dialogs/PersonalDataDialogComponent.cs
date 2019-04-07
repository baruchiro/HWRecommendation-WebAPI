using HW.Bot.Dialogs.AtomicDialogs;
using HW.Bot.Extensions;
using HW.Bot.Interfaces;
using HW.Bot.Model;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HW.Bot.Dialogs
{
    public class PersonalDataDialogComponent : ComponentDialog
    {
        private readonly EnumChoicePrompt<Gender> _genderPrompt = new EnumChoicePrompt<Gender>(GENDER_CHOICE_DIALOG);
        private readonly AgeNumberPrompt _agePrompt = new AgeNumberPrompt(AGE_NUMBER_DIALOG);
        private readonly WorkTextPrompt _workPrompt = new WorkTextPrompt(WORK_TEXT_DIALOG);

        private readonly IDbContext _dbContext;
        private const string WATERFALL_DIALOG = nameof(PersonalDataDialogComponent) + nameof(WaterfallDialog) + "newuser";
        private const string GENDER_CHOICE_DIALOG = nameof(PersonalDataDialogComponent) + nameof(EnumChoicePrompt<Gender>) + "gender";
        private const string AGE_NUMBER_DIALOG = nameof(PersonalDataDialogComponent) + nameof(AgeNumberPrompt) + "age";
        private const string WORK_TEXT_DIALOG = nameof(PersonalDataDialogComponent) + nameof(WorkTextPrompt) + "work";
        private const string DETAILS_EXIST_WATERFALL = nameof(PersonalDataDialogComponent) + nameof(WaterfallDialog) + "existuser";
        private const string dataID = nameof(PersonalDataDialogComponent) + "data";

        public PersonalDataDialogComponent(string dialogId, IDbContext dbContext) : base(dialogId)
        {
            _dbContext = dbContext;

            AddDialog(new WaterfallDialog(WATERFALL_DIALOG)
                .AddStep(NewUserStartStep)
                .AddStep(_genderPrompt.PromptAsync)
                .AddStep(HandleGenderStep)
                .AddStep(_agePrompt.PromptAsync)
                .AddStep(HandleAgeStep)
                .AddStep(RequestWorkStep)
                .AddStep(HandleWorkStep)
                .AddStep(SaveDetailsStep)
            );

            AddDialog(_genderPrompt);
            AddDialog(_agePrompt);
            AddDialog(_workPrompt);
        }

        private async Task<DialogTurnResult> NewUserStartStep(WaterfallStepContext stepcontext, CancellationToken cancellationtoken)
        {
            await stepcontext.Context.SendActivityAsync(
                "First, we need some of your personal data to give you specific recommendations and to upgrade our system.", cancellationToken: cancellationtoken);

            stepcontext.Values[dataID] = new PersonalData();

            return await stepcontext.NextAsync(cancellationToken: cancellationtoken);
        }

        private async Task<DialogTurnResult> HandleGenderStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (stepContext.Result is FoundChoice foundChoice)
            {
                var personalData = stepContext.Values[dataID] as IPersonalData ?? new PersonalData();
                Enum.TryParse<Gender>(foundChoice.Value, true, out var result);
                personalData.Gender = (int)result;

                stepContext.Values[dataID] = personalData;
            }

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> HandleAgeStep(WaterfallStepContext stepcontext, CancellationToken cancellationtoken)
        {
            ((IPersonalData)stepcontext.Values[dataID]).Age = stepcontext.Result is int age ? age : -1;

            return await stepcontext.NextAsync(cancellationToken: cancellationtoken);
        }

        private async Task<DialogTurnResult> RequestWorkStep(WaterfallStepContext stepcontext, CancellationToken cancellationtoken)
        {
            return await _workPrompt.PromptAsync(stepcontext, cancellationtoken, _dbContext.GetOrderedWorkList().Take(5));
        }

        private async Task<DialogTurnResult> HandleWorkStep(WaterfallStepContext stepcontext, CancellationToken cancellationtoken)
        {
            ((IPersonalData)stepcontext.Values[dataID]).WorkArea = stepcontext.Result as string;

            return await stepcontext.NextAsync(cancellationToken: cancellationtoken);
        }

        private async Task<DialogTurnResult> SaveDetailsStep(WaterfallStepContext stepcontext,
            CancellationToken cancellationtoken)
        {
            var userDetails = (IPersonalData)stepcontext.Values[dataID];

            var channelId = stepcontext.Context.Activity.ChannelId;
            var userId = stepcontext.Context.Activity.From.Id;

            if (_dbContext.SavePersonalDetails(channelId, userId, userDetails))
            {
                await stepcontext.Context.SendActivityAsync(MessageFactory.Text("Your data saved"),
                    cancellationtoken);

                await stepcontext.Context.SendActivityAsync(
                    $"Saving your data:\n" +
                    $"Gender- {userDetails.Gender.ToEnumDescription<Gender>()}\n" +
                    $"Age- {userDetails.Age}\n" + 
                    $"WorkArea- {userDetails.WorkArea}",
                    cancellationToken: cancellationtoken);
            }
            else
            {
                await stepcontext.Context.SendActivityAsync(MessageFactory.Text("We can't save your data"),
                    cancellationtoken);
            }
            return await stepcontext.EndDialogAsync(cancellationToken: cancellationtoken);
        }
    }
}