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

namespace LBHAddressesAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            TestStatus.IsRunningInTests = false;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //TODO: This shouldn't be here :) just for testing.
            //var connectionString = "server=LBHSQLT03\\MSSQL2017; database=ADDRESSES_API_TEST; Integrated Security=true;";
            var connectionString = Environment.GetEnvironmentVariable("LLPGConnectionString");

            services.ConfigureAddressSearch(connectionString);

            services.AddCors(option => {
                option.AddPolicy("AllowAny", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Hackney Addresses API", Version = "v1" });

                c.DescribeAllEnumsAsStrings();

                var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, @"HackneyAddressesAPI.xml");
                c.IncludeXmlComments(filePath);
            });

            services.AddCustomServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            // don't use preceding slash in endpoint path
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Hackney Addresses API v1");
                c.RoutePrefix = "swagger";
            });

            app.UseCors("AllowAny");

            app.UseMvc();   
        }
    }
}
