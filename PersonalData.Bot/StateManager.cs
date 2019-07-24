using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using HW.Bot.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Models;

namespace HW.Bot
{
    [Localizable(false)]
    internal class StateManager : IPersonalDataStateManager
    {
        public StateManager(IStorage storage)
        {
            ConversationState = new ConversationState(storage);
            ConversationDataAccessor = ConversationState.CreateProperty<DialogState>(ConversationDataName);

            UserState = new UserState(storage);
            PersonalDataAccessor = UserState.CreateProperty<Person>(UserProfileName);
        }

        public static string ConversationDataName { get; } = "ConversationData";
        public static string UserProfileName { get; } = "UserProfile";

        public IStatePropertyAccessor<DialogState> ConversationDataAccessor { get; set; }

        public ConversationState ConversationState { get; }
        
        public IStatePropertyAccessor<Person> PersonalDataAccessor { get; set; }
        
        public UserState UserState { get; }
        public Task<Person> GetPersonalDataAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            return PersonalDataAccessor.GetAsync(turnContext, () => new Person(), cancellationToken);
        }

        public async Task SavePersonalDataAsync(ITurnContext turnContext, Person personalData, CancellationToken cancellationToken)
        {
             await PersonalDataAccessor.SetAsync(turnContext, personalData, cancellationToken);
             await UserState.SaveChangesAsync(turnContext, cancellationToken: cancellationToken);
        }

        public async Task UpdatePersonalDataAsync(ITurnContext turnContext, Action<Person> updateAction, CancellationToken cancellationToken)
        {
            var personalData = await GetPersonalDataAsync(turnContext, cancellationToken);
            updateAction(personalData);
            await SavePersonalDataAsync(turnContext, personalData, cancellationToken);
        }
    }
}
