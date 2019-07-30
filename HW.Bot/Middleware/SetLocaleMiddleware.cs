using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;

namespace HW.Bot.Middleware
{
    class SetLocaleMiddleware : IMiddleware
    {
        private readonly string defaultLocale;

        public SetLocaleMiddleware(string defaultDefaultLocale)
        {
            defaultLocale = defaultDefaultLocale;
        }

        public async Task OnTurnAsync(ITurnContext turnContext, NextDelegate next,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var cultureInfo = string.IsNullOrEmpty(turnContext.Activity.Locale) ? new CultureInfo(defaultLocale) : new CultureInfo(turnContext.Activity.Locale);

            CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture = cultureInfo;

            await next(cancellationToken).ConfigureAwait(false);
        }
    }
}
