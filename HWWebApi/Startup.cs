using System.IO;
using AlgorithmManager.Extensions;
using AutoML;
using HW.Bot;
using HWWebApi.Bot;
using HWWebApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;

namespace HWWebApi
{
    public class Startup
    {
        private IHostingEnvironment _environment;
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _environment = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var connection = Configuration.GetConnectionString("HWConnectionString");
            services.AddDbContext<HardwareContext>(options => options.UseSqlServer(connection));

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "HWRecommendation - Web API", Version = "v1" });
                if (File.Exists("HWWebApi.xml"))
                {
                    c.IncludeXmlComments("HWWebApi.xml");
                }
                else if (File.Exists(Path.Combine("..","HWWebApi.xml")))
                {
                    c.IncludeXmlComments(Path.Combine("..", "HWWebApi.xml"));

                }
                
            });

            services.RegisterRecommendationAlgorithm();
            services.AddRecommendationBot<BotDbContextAdapter, AutoMLRecommender>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HWRecommendation - Web API V1");
                    c.DocumentTitle = "HWRecommendation - Web API";
                });
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseHsts();
            }

            app.UseMvc()
                .UseBotFramework();
        }
    }
}
