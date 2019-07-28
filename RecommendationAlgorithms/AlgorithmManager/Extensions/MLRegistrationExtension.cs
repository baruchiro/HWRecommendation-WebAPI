using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;
using AlgorithmManager.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML;

namespace AlgorithmManager.Extensions
{
    public static class MLRegistrationExtension
    {
        public static void RegisterRecommendationAlgorithm(this IServiceCollection services)
        {
            services.AddSingleton(provider => new MLContext(0));
            services.AddSingleton<AlgorithmManagerFactory>();
            
            services.AddSingleton<ModelSaver>();
        }
    }
}
