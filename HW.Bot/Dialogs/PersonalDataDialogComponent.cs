using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HW.Bot.Dialogs.AtomicDialogs;
using HW.Bot.Dialogs.MenuDialog;
using HW.Bot.Dialogs.Steps;
using HW.Bot.Extensions;
using HW.Bot.Interfaces;
using HW.Bot.Resources;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Models;

namespace HW.Bot.Dialogs
{
    internal class PersonalDataDialogComponent : ComponentDialog, IMenuItemDialog
    {
        private readonly EnumChoicePrompt<Gender> _genderPrompt = new EnumChoicePrompt<Gender>(GENDER_CHOICE_DIALOG, BotStrings.Select_your_gender, BotStrings.Change_your_Gender);
        private readonly AgeNumberPrompt _agePrompt = new AgeNumberPrompt(AGE_NUMBER_DIALOG, BotStrings.Change_your_age);
        private readonly WorkTextPrompt _workPrompt;

        private readonly IDbContext _dbContext;
        private readonly IPersonStateManager _personStateManager;
        private const string NEW_USER_WATERFALL = nameof(PersonalDataDialogComponent) + nameof(WaterfallDialog) + "newuser";
        private const string EXIST_USER_MENU = nameof(PersonalDataDialogComponent) + nameof(WaterfallDialog) + "existuser";
        private const string GENDER_CHOICE_DIALOG = "Gender";
        private const string AGE_NUMBER_DIALOG = "Age";
        private const string WORK_TEXT_DIALOG = "Work";
        private const string WELCOME_WATERFALL = nameof(PersonalDataDialogComponent) + nameof(WaterfallDialog) + "welcome";
        private const string DATA_ID = nameof(PersonalDataDialogComponent) + "data";

        public string MenuItemOptionText { get; }
        public Func<ITurnContext, object, CancellationToken, Task> HandleResult { get; set; }

        public PersonalDataDialogComponent([Localizable(false)] string dialogId, IPersonStateManager personStateManager,
            IDbContext dbContext, string menuItemOptionText = null) : base(dialogId)
        {
            MenuItemOptionText = menuItemOptionText ?? dialogId;
            _personStateManager = personStateManager;
            _dbContext = dbContext;
            _workPrompt =
                new WorkTextPrompt(WORK_TEXT_DIALOG, menuItemOptionText: BotStrings.Change_your_work,
                    suggestedActions: _dbContext.GetOrderedWorkList().Take(5), handleResult: HandleWork);
            _agePrompt.HandleResult = HandleAge;
            _genderPrompt.HandleResult = HandleGender;

            AddDialog(new WaterfallDialog(WELCOME_WATERFALL)
                .AddStep(DecideIfNewOrExistUser.Step(dbContext, NEW_USER_WATERFALL, EXIST_USER_MENU))
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

            AddDialog(new MenuDialogComponent(EXIST_USER_MENU, BotStrings.Select_which_data_you_want_to_change,
                new List<IMenuItemDialog>
                {
                    _genderPrompt,
                    _agePrompt,
                    _workPrompt
                }));

            AddDialog(_genderPrompt);
            AddDialog(_agePrompt);
            AddDialog(_workPrompt);
        }

        private async Task<DialogTurnResult> SaveDetailsStep(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            var person = await _personStateManager.GetPersonAsync(stepContext.Context, cancellationToken);

            var channelId = stepContext.Context.Activity.ChannelId;
            var userId = stepContext.Context.Activity.From.Id;

            await stepContext.Context.SendActivityAsync(string.Format(BotStrings.Saving_info_of_user, userId, channelId), cancellationToken: cancellationToken);

            if (_dbContext.SavePerson(channelId, userId, person))
            {
                await stepContext.Context.SendActivityAsync(
                    BotStrings.Saving_your_data +
                    string.Format(BotStrings.data_gender, person.Gender.GetDescription()) +
                    string.Format(BotStrings.data_age, person.Age) +
                    string.Format(BotStrings.date_workArea, person.WorkArea),
                    cancellationToken: cancellationToken);
            }
            else
            {
                await stepContext.Context.SendActivityAsync(BotStrings.cant_save_your_data, cancellationToken: cancellationToken);
            }

            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> HandlePersonalDataStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values[DATA_ID] = stepContext.Options as Person ?? new Person();
            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> HandleGenderStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (stepContext.Result is FoundChoice foundChoice)
            {
                var person = stepContext.Values[DATA_ID] as Person ?? new Person();
                Enum.TryParse<Gender>(foundChoice.Value, true, out var result);
                person.Gender = result;

                stepContext.Values[DATA_ID] = person;
            }

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> HandleAgeStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            ((Person)stepContext.Values[DATA_ID]).Age = stepContext.Result is int age ? age : -1;

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> HandleWorkStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            ((Person)stepContext.Values[DATA_ID]).WorkArea = stepContext.Result as string;

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> EndWaterfallStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (stepContext.Values[DATA_ID] is Person person)
            {
                await _personStateManager.SavePersonAsync(stepContext.Context, person, cancellationToken);
            }
            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        public Dialog GetDialog()
        {
            return this;
        }

        private async Task HandleWork(ITurnContext turnContext, object result, CancellationToken cancellationToken)
        {
            await _personStateManager.UpdatePersonAsync(turnContext,
                person =>
                {
                    if (result is string work)
                        person.WorkArea = work;
                }, cancellationToken);
        }

        private async Task HandleAge(ITurnContext turnContext, object result, CancellationToken cancellationToken)
        {
            await _personStateManager.UpdatePersonAsync(turnContext,
                person =>
                {
                    if (result is int age)
                        person.Age = age;
                }, cancellationToken);
        }

        private async Task HandleGender(ITurnContext turnContext, object result, CancellationToken cancellationToken)
        {
            await _personStateManager.UpdatePersonAsync(turnContext,
                person =>
                {
                    if (!(result is FoundChoice foundChoice)) return;
                    Enum.TryParse<Gender>(foundChoice.Value, true, out var gender);
                    person.Gender = gender;
                }, cancellationToken);
        }
    }
}