using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Model;
using PersonalData.Bot.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PersonalData.Bot.Dialogs
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
            this._dbContext = dbContext;
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
            var promptOptions = new PromptOptions
            {
                Choices = ChoiceFactory.ToChoices(Enum.GetNames(typeof(Gender))),
                Prompt = MessageFactory.Text("Select your gender")
            };
            return await stepcontext.PromptAsync(CHOICE_DIALOG, promptOptions, cancellationtoken);
        }

        private async Task<DialogTurnResult> RequestAgeStep(WaterfallStepContext stepcontext,
            CancellationToken cancellationtoken)
        {
            var personalData = stepcontext.Values[dataID] as Model.PersonalData ?? new Model.PersonalData();
            personalData.Gender = Enum.TryParse<Gender>(stepcontext.Result as string, true, out var result)
                ? result
                : Gender.NotDefine;
            stepcontext.Values[dataID] = personalData;

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("Enter your age"),
                RetryPrompt = MessageFactory.Text("Please, Enter your age!!")
            };
            return await stepcontext.PromptAsync(NUMBER_DIALOG, promptOptions, cancellationtoken);
        }

        private async Task<DialogTurnResult> RequestWorkStep(WaterfallStepContext stepcontext, CancellationToken cancellationtoken)
        {
            ((Model.PersonalData)stepcontext.Values[dataID]).Age = stepcontext.Result is int age ? age : -1;

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
            ((Model.PersonalData)stepcontext.Values[dataID]).Work = stepcontext.Result as string;

            var userDetails = (Model.PersonalData)stepcontext.Values[dataID];

            await stepcontext.Context.SendActivityAsync(
                MessageFactory.Text($"Saving your data:\n" +
                                    $"Gender- {userDetails.Gender.ToString()}\n" +
                                    $"Age- {userDetails.Age}\n" +
                                    $"Work- {userDetails.Work}"),
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