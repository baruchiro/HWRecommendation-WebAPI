using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Models;

namespace HW.Bot.Interfaces
{
    internal interface IPersonalDataStateManager
    {
        Task<Person> GetPersonalDataAsync(ITurnContext turnContext, CancellationToken cancellationToken);
        Task SavePersonalDataAsync(ITurnContext turnContext, Person personalData, CancellationToken cancellationToken);
        Task UpdatePersonalDataAsync(ITurnContext turnContext, Action<Person> updateAction, CancellationToken cancellationToken);
    }
}
