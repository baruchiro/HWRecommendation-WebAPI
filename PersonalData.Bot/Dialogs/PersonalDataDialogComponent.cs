using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Model;
using PersonalData.Bot.Interfaces;

namespace PersonalData.Bot.Dialogs
{
    public class PersonalDataDialogComponent : ComponentDialog
    {
        private const string WATERFALL_DIALOG = nameof(PersonalDataDialogComponent) + "waterfall";
        private const string NUMBER_DIALOG = nameof(PersonalDataDialogComponent) + "number";
        private const string CHOICE_DIALOG = nameof(PersonalDataDialogComponent) + "choice";
        private const string TEXT_PROMPT = nameof(PersonalDataDialogComponent) + "text";
        private const string dataID = nameof(PersonalDataDialogComponent)+"data";

        public PersonalDataDialogComponent(string dialogId, IDbContext dbContext) : base(dialogId)
        {
            AddDialog(new WaterfallDialog(WATERFALL_DIALOG)
                .AddStep(RequestGenderStep)
                .AddStep(RequestAgeStep)
                .AddStep(RequestWorkStep)
                .AddStep(PrintAllDetailsStep)
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
            ((Model.PersonalData) stepcontext.Values[dataID]).Age = stepcontext.Result is int age ? age : -1;

            var activityPrompt = MessageFactory.Text("Select one of this works or write your own.");
            activityPrompt.SuggestedActions = new SuggestedActions()
            {
                //Actions = 
            };
            var options = new PromptOptions()
            { Prompt = activityPrompt
            };

            return await stepcontext.PromptAsync(TEXT_PROMPT, options, cancellationtoken);
        }

        private Task<DialogTurnResult> PrintAllDetailsStep(WaterfallStepContext stepcontext, CancellationToken cancellationtoken)
        {
            throw new NotImplementedException();
        }
    }
}