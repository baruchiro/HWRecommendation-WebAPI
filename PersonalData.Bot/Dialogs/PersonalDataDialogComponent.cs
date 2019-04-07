using HW.Bot.Extensions;
using HW.Bot.Interfaces;
using HW.Bot.Model;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HW.Bot.Dialogs
{
    public class PersonalDataDialogComponent : ComponentDialog
    {
        private readonly IDbContext _dbContext;
        private const string WATERFALL_DIALOG = nameof(PersonalDataDialogComponent) + "waterfall";
        private const string NUMBER_DIALOG = nameof(PersonalDataDialogComponent) + "number";
        private const string CHOICE_DIALOG = nameof(PersonalDataDialogComponent) + "choice";
        private const string TEXT_PROMPT = nameof(PersonalDataDialogComponent) + "text";
        private const string dataID = nameof(PersonalDataDialogComponent) + "data";

        public PersonalDataDialogComponent(string dialogId, IDbContext dbContext) : base(dialogId)
        {
            _dbContext = dbContext;
            AddDialog(new WaterfallDialog(WATERFALL_DIALOG)
                .AddStep(RequestGenderStep)
                .AddStep(RequestAgeStep)
                .AddStep(RequestWorkStep)
                .AddStep(SaveAndPrintAllDetailsStep)
            );

            AddDialog(new NumberPrompt<int>(NUMBER_DIALOG));
            AddDialog(new ChoicePrompt(CHOICE_DIALOG));
            AddDialog(new TextPrompt(TEXT_PROMPT));

        }

        private async Task<DialogTurnResult> RequestGenderStep(WaterfallStepContext stepcontext, CancellationToken cancellationtoken)
        {
            stepcontext.Context.Activity.CreateReply(
                "First, we need some of your personal data to give you specific recommendations and to upgrade our system.");

            stepcontext.Values[dataID] = new Model.PersonalData();

            var choices = typeof(Gender).GetEnumValues().Cast<Gender>().Select(g =>
                new Choice(g.ToString())
                {
                    Action = new CardAction(ActionTypes.ImBack,
                        g.GetDescription(),
                        text: g.GetDescription(),
                        displayText: g.GetDescription(),
                        value: g.ToString())
                })
                .ToArray();

            var promptOptions = new PromptOptions
            {
                Choices = choices,// ChoiceFactory.ToChoices(typeof(Gender).GetDescriptions().ToArray()),
                Prompt = MessageFactory.Text("Select your gender")
            };
            return await stepcontext.PromptAsync(CHOICE_DIALOG, promptOptions, cancellationtoken);
        }

        private async Task<DialogTurnResult> RequestAgeStep(WaterfallStepContext stepcontext,
            CancellationToken cancellationtoken)
        {
            if (stepcontext.Result is FoundChoice foundChoice)
            {
                var personalData = stepcontext.Values[dataID] as IPersonalData ?? new Model.PersonalData();
                personalData.Gender = (int)
                    (Enum.TryParse<Gender>(foundChoice.Value, true, out var result)
                        ? result
                        : Gender.NOT_DEFINE);

                stepcontext.Values[dataID] = personalData;
            }

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("Enter your age"),
                RetryPrompt = MessageFactory.Text("Please, Enter your age!!")
            };
            return await stepcontext.PromptAsync(NUMBER_DIALOG, promptOptions, cancellationtoken);
        }

        private async Task<DialogTurnResult> RequestWorkStep(WaterfallStepContext stepcontext, CancellationToken cancellationtoken)
        {
            ((IPersonalData)stepcontext.Values[dataID]).Age = stepcontext.Result is int age ? age : -1;

            var activityPrompt = MessageFactory.Text("Select one of this works or write your own.");
            activityPrompt.SuggestedActions = new SuggestedActions
            {
                Actions = _dbContext.GetOrderedWorkList().Take(5)
                    .Select(w => new CardAction(ActionTypes.PostBack, w, value: w)).ToList()
            };

            var options = new PromptOptions
            {
                Prompt = activityPrompt
            };

            return await stepcontext.PromptAsync(TEXT_PROMPT, options, cancellationtoken);
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