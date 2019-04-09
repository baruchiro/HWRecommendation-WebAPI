using System;
using System.Threading;
using System.Threading.Tasks;
using HW.Bot.Interfaces;
using HW.Bot.Model;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace HW.Bot
{
    internal class StateManager : IPersonalDataStateManager
    {
        public StateManager(IStorage storage)
        {
            ConversationState = new ConversationState(storage);
            ConversationDataAccessor = ConversationState.CreateProperty<DialogState>(ConversationDataName);

            UserState = new UserState(storage);
            PersonalDataAccessor = UserState.CreateProperty<IPersonalData>(UserProfileName);
        }

        public static string ConversationDataName { get; } = "ConversationData";
        public static string UserProfileName { get; } = "UserProfile";

        public IStatePropertyAccessor<DialogState> ConversationDataAccessor { get; set; }

        public ConversationState ConversationState { get; }
        
        public IStatePropertyAccessor<IPersonalData> PersonalDataAccessor { get; set; }
        
        public UserState UserState { get; }
        public Task<IPersonalData> GetPersonalDataAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            return PersonalDataAccessor.GetAsync(turnContext, () => new PersonalData(), cancellationToken);
        }

        public async Task SavePersonalDataAsync(ITurnContext turnContext, IPersonalData personalData, CancellationToken cancellationToken)
        {
             await PersonalDataAccessor.SetAsync(turnContext, personalData, cancellationToken);
             await UserState.SaveChangesAsync(turnContext, cancellationToken: cancellationToken);
        }

        public async Task UpdatePersonalDataAsync(ITurnContext turnContext, Action<IPersonalData> updateAction, CancellationToken cancellationToken)
        {
            var personalData = await GetPersonalDataAsync(turnContext, cancellationToken);
            updateAction(personalData);
            await SavePersonalDataAsync(turnContext, personalData, cancellationToken);
        }
    }
}
