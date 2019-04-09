using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;

namespace HW.Bot.Interfaces
{
    internal interface IPersonalDataStateManager
    {
        Task<IPersonalData> GetPersonalDataAsync(ITurnContext turnContext, CancellationToken cancellationToken);
        Task SavePersonalDataAsync(ITurnContext turnContext, IPersonalData personalData, CancellationToken cancellationToken);
        Task UpdatePersonalDataAsync(ITurnContext turnContext, Action<IPersonalData> updateAction, CancellationToken cancellationToken);
    }
}
