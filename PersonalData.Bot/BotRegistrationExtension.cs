using System.ComponentModel;
using HW.Bot.Interfaces;
using HW.Bot.Middleware;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.BotFramework;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HW.Bot
{
    public static class BotRegistrationExtension
    {
        [Localizable(false)]
        public static void AddRecommendationBot<T>(this IServiceCollection services)
            where T : class, IDbContext
        {
            services.AddSingleton<ICredentialProvider, ConfigurationCredentialProvider>();
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();
            services.AddScoped<IDbContext, T>();
            services.AddSingleton<IStorage>(new MemoryStorage());
            services.AddSingleton<StateManager>();

            services.AddBot<RecommendationBot>(options => 
                options.Middleware.Add(new SetLocaleMiddleware("he-il")));
        }

        public static IBot GetBotForTest(IDbContext dbContext)
        {
            return new RecommendationBot(new StateManager(new MemoryStorage()), dbContext);
        }
    }
}
