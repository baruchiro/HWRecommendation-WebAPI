using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HW.Bot
{
    public class StateManager
    {
        public StateManager(IStorage storage)
        {
            ConversationState = new ConversationState(storage);
            ConversationDataAccessor = ConversationState.CreateProperty<DialogState>(ConversationDataName);
        }

        public static string ConversationDataName { get; } = "ConversationData";

        public IStatePropertyAccessor<DialogState> ConversationDataAccessor { get; set; }

        public ConversationState ConversationState { get; }
    }
}
