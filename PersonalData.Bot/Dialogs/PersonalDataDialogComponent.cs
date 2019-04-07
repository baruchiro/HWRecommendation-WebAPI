using HW.Bot.Dialogs.AtomicDialogs;
using HW.Bot.Extensions;
using HW.Bot.Interfaces;
using HW.Bot.Model;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
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
        private const string WATERFALL_DIALOG = nameof(PersonalDataDialogComponent) + "waterfall";
        private const string GENDER_CHOICE_DIALOG = nameof(PersonalDataDialogComponent) + nameof(EnumChoicePrompt<Gender>) + "gender";
        private const string AGE_NUMBER_DIALOG = nameof(PersonalDataDialogComponent) + nameof(AgeNumberPrompt) + "age";
        private const string WORK_TEXT_DIALOG = nameof(PersonalDataDialogComponent) + nameof(WorkTextPrompt) + "work";
        private const string dataID = nameof(PersonalDataDialogComponent) + "data";

        public PersonalDataDialogComponent(string dialogId, IDbContext dbContext, IPersonalData personalData = null) : base(dialogId)
        {
            _dbContext = dbContext;
            AddDialog(new WaterfallDialog(WATERFALL_DIALOG)
                .AddStep(RequestGenderStep)
                .AddStep(RequestAgeStep)
                .AddStep(RequestWorkStep)
                .AddStep(SaveAndPrintAllDetailsStep)
            );

            AddDialog(_genderPrompt);
            AddDialog(_agePrompt);
            AddDialog(_workPrompt);
        }

        private async Task<DialogTurnResult> RequestGenderStep(WaterfallStepContext stepcontext, CancellationToken cancellationtoken)
        {
            stepcontext.Context.Activity.CreateReply(
                "First, we need some of your personal data to give you specific recommendations and to upgrade our system.");

            stepcontext.Values[dataID] = new PersonalData();

            return await _genderPrompt.PromptAsync(stepcontext, cancellationtoken);// stepcontext.PromptAsync(CHOICE_DIALOG, promptOptions, cancellationtoken);
        }

        private async Task<DialogTurnResult> RequestAgeStep(WaterfallStepContext stepcontext,
            CancellationToken cancellationtoken)
        {
            if (stepcontext.Result is FoundChoice foundChoice)
            {
                var personalData = stepcontext.Values[dataID] as IPersonalData ?? new Model.PersonalData();
                Enum.TryParse<Gender>(foundChoice.Value, true, out var result);
                personalData.Gender = (int)result;

                stepcontext.Values[dataID] = personalData;
            }

            return await _agePrompt.PromptAsync(stepcontext, cancellationtoken);
        }

        private async Task<DialogTurnResult> RequestWorkStep(WaterfallStepContext stepcontext, CancellationToken cancellationtoken)
        {
            ((IPersonalData)stepcontext.Values[dataID]).Age = stepcontext.Result is int age ? age : -1;

            return await _workPrompt.PromptAsync(stepcontext, cancellationtoken, _dbContext.GetOrderedWorkList().Take(5));
        }

        private async Task<DialogTurnResult> SaveAndPrintAllDetailsStep(WaterfallStepContext stepcontext,
            CancellationToken cancellationtoken)
        {
            ((IPersonalData)stepcontext.Values[dataID]).WorkArea = stepcontext.Result as string;

            var userDetails = (IPersonalData)stepcontext.Values[dataID];

            await stepcontext.Context.SendActivityAsync(
                MessageFactory.Text($"Saving your data:\n" +
                                    $"Gender- {userDetails.Gender.ToEnumDescription<Gender>()}\n" +
                                    $"Age- {userDetails.Age}\n" +
                                    $"WorkArea- {userDetails.WorkArea}"),
                cancellationtoken);

            var channelId = stepcontext.Context.Activity.ChannelId;
            var userId = stepcontext.Context.Activity.From.Id;

            if (_dbContext.SavePersonalDetails(channelId, userId, userDetails))
            {
                await stepcontext.Context.SendActivityAsync(MessageFactory.Text("Your data saved"),
                    cancellationtoken);
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