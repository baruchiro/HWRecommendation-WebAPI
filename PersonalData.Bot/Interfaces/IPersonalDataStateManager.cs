﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;

namespace HW.Bot.Interfaces
{
    interface IPersonalDataStateManager
    {
        Task<IPersonalData> GetPersonalDataAsync(ITurnContext turnContext, CancellationToken cancellationToken);
        Task SavePersonalDataAsync(ITurnContext turnContext, IPersonalData personalData, CancellationToken cancellationToken);
        Task UpdatePersonalDataAsync(ITurnContext turnContext, Action<IPersonalData> updateAction, CancellationToken cancellationToken);
    }
}
