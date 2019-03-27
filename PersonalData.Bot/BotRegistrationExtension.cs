using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Configuration;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PersonalData.Bot.Interfaces;
using System;
using System.Linq;

namespace PersonalData.Bot
{
    public static class BotRegistrationExtension
    {
        public static void AddPersonalDataBot<T>(this IServiceCollection services, IConfiguration configuration, IHostingEnvironment env)
            where T : IDbContext
        {
            services.AddBot<RecommendationBot>(options =>
            {
                var secretKey = configuration.GetSection("botFileSecret")?.Value;
                var botFilePath = configuration.GetSection("botFilePath")?.Value;

                // Loads .bot configuration file and adds a singleton that your Bot can access through dependency injection.
                var botConfig = BotConfiguration.Load(botFilePath ?? @".\HWRecommendationBot.bot", secretKey);
                services.AddSingleton(sp =>
                    botConfig ??
                    throw new InvalidOperationException(
                        $"The .bot configuration file could not be loaded. ({botFilePath ?? @".\HWRecommendationBot.bot"})"));

                // Retrieve current endpoint.
                var environment = env.IsProduction() ? "production" : "development";
                var service = botConfig.Services.FirstOrDefault(s => s.Type == "endpoint" && s.Name == environment);
                if (!(service is EndpointService endpointService))
                {
                    throw new InvalidOperationException($"The .bot file does not contain an endpoint.");
                }

                options.CredentialProvider =
                    new SimpleCredentialProvider(endpointService.AppId, endpointService.AppPassword);

                // Creates a logger for the application to use.
                ILogger logger = new LoggerFactory().CreateLogger<RecommendationBot>();

                // Catches any errors that occur during a conversation turn and logs them.
                options.OnTurnError = async (context, exception) =>
                {
                    logger.LogError($"Exception caught : {exception}");
                    await context.SendActivityAsync("Sorry, it looks like something went wrong.");
                };
            });

            services.AddScoped(typeof(IDbContext), typeof(T));
            services.AddSingleton<IStorage>(new MemoryStorage());
            services.AddSingleton<StateManager>();
        }
    }
}
