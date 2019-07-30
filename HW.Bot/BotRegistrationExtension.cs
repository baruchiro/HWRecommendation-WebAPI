using System;
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
        private static void Register(this IServiceCollection services)
        {
            services.AddSingleton<ICredentialProvider, ConfigurationCredentialProvider>();
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();
            services.AddSingleton<IStorage>(new MemoryStorage());
            services.AddSingleton<StateManager>();

            services.AddBot<RecommendationBot>(options =>
                options.Middleware.Add(new SetLocaleMiddleware("he-il")));
        }

        [Localizable(false)]
        public static void AddRecommendationBot<TDbContext, TRecommender>(this IServiceCollection services)
            where TDbContext : class, IDbContext
            where TRecommender : class, IRecommender
        {
            services.Register();

            services.AddScoped<IDbContext, TDbContext>();
            services.AddSingleton<IRecommender, TRecommender>();
        }

        public static void AddRecommendationBot<TDbContext, TRecommender>(this IServiceCollection services,
            Func<IServiceProvider,TDbContext> dbContextImplementationFactory,
            Func<IServiceProvider, TRecommender> recommenderImplementationFactory)
            where TDbContext : class, IDbContext
            where TRecommender : class, IRecommender
        {
            services.Register();

            services.AddScoped<IDbContext>(dbContextImplementationFactory);
            services.AddSingleton<IRecommender>(recommenderImplementationFactory);
        }
    }
}
