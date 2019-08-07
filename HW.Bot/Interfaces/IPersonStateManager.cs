using Microsoft.Bot.Builder;
using Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HW.Bot.Interfaces
{
    internal interface IPersonStateManager
    {
        Task<Person> GetPersonAsync(ITurnContext turnContext, CancellationToken cancellationToken);
        Task SavePersonAsync(ITurnContext turnContext, Person person, CancellationToken cancellationToken);
        Task UpdatePersonAsync(ITurnContext turnContext, Action<Person> updateAction, CancellationToken cancellationToken);
    }
}
