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
    internal class StateManager : IPersonStateManager
    {
        public StateManager(IStorage storage)
        {
            ConversationState = new ConversationState(storage);
            ConversationDataAccessor = ConversationState.CreateProperty<DialogState>(ConversationDataName);

            UserState = new UserState(storage);
            PersonAccessor = UserState.CreateProperty<Person>(UserProfileName);
        }

        public static string ConversationDataName { get; } = "ConversationData";
        public static string UserProfileName { get; } = "UserProfile";

        public IStatePropertyAccessor<DialogState> ConversationDataAccessor { get; set; }

        public ConversationState ConversationState { get; }
        
        public IStatePropertyAccessor<Person> PersonAccessor { get; set; }
        
        public UserState UserState { get; }
        public Task<Person> GetPersonAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            return PersonAccessor.GetAsync(turnContext, () => new Person(), cancellationToken);
        }

        public async Task SavePersonAsync(ITurnContext turnContext, Person person, CancellationToken cancellationToken)
        {
             await PersonAccessor.SetAsync(turnContext, person, cancellationToken);
             await UserState.SaveChangesAsync(turnContext, cancellationToken: cancellationToken);
        }

        public async Task UpdatePersonAsync(ITurnContext turnContext, Action<Person> updateAction, CancellationToken cancellationToken)
        {
            var person = await GetPersonAsync(turnContext, cancellationToken);
            updateAction(person);
            await SavePersonAsync(turnContext, person, cancellationToken);
        }
    }
}
