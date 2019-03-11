using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace HWWebApi.Bot
{
    public class RecommendationBot : IBot
    {
        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = new CancellationToken())
        {
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
               await turnContext.SendActivityAsync($"You say:{turnContext.Activity.Text}");
            }
            else
            {
                await turnContext.SendActivityAsync($"{turnContext.Activity.Type} event detected");
            }
        }
    }
}
