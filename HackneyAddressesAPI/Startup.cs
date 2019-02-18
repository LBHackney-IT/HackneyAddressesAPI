using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LBHAddressesAPI.Actions;
using LBHAddressesAPI.DB;
using LBHAddressesAPI.Helpers;
using LBHAddressesAPI.Interfaces;
using LBHAddressesAPI.Logging;
using LBHAddressesAPI.Tests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using LBHAddressesAPI.Infrastructure.V1.Services;
using LBHAddressesAPI.Infrastructure.V1.Middleware;
using LBHAddressesAPI.Settings;

namespace LBHAddressesAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //TODO: THis needs to be changed before putting in to prod. 
            var connectionString = Environment.GetEnvironmentVariable("LLPGConnectionString");
            //var connectionString = Environment.GetEnvironmentVariable("LLPGConnectionStringLive");
            //var connectionString = Environment.GetEnvironmentVariable("LLPGConnectionStringDev");
            //var connectionString = Environment.GetEnvironmentVariable("LLPGConnectionStringTest");
            var settings = Configuration.Get<ConfigurationSettings>();

            services.ConfigureAddressSearch(connectionString);

            //services.AddCors(option => {
            //    option.AddPolicy("AllowAny", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            //});

            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Token",
                    new ApiKeyScheme
                    {
                        In = "header",
                        Description = "Your Hackney API Key",
                        Name = "X-Api-Key",
                        Type = "apiKey"
                    });
                
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {"Token", Enumerable.Empty<string>()}
                });

                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Hackney Addresses API", Version = "v1" });

                c.DescribeAllEnumsAsStrings();

                var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, @"HackneyAddressesAPI.xml");
                c.IncludeXmlComments(filePath);
            });

            services.AddCustomServices();

            services.ConfigureLogging(Configuration, settings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Register exception handling middleware early so exceptions are handled and formatted
            app.UseMiddleware<CustomExceptionHandlerMiddleware>();

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            // don't use preceding slash in endpoint path
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Hackney Addresses API v1");
                c.RoutePrefix = "swagger";
            });

            //app.UseCors("AllowAny");

            app.UseMvc();   
        }
    }
}
