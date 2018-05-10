using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HackneyAddressesAPI.Actions;
using HackneyAddressesAPI.DB;
using HackneyAddressesAPI.Helpers;
using HackneyAddressesAPI.Interfaces;
using HackneyAddressesAPI.Logging;
using HackneyAddressesAPI.Tests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;

namespace HackneyAddressesAPI
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
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hackney Addresses API v1");
                c.RoutePrefix = "";
            });

            app.UseCors("AllowAny");

            app.UseMvc();   
        }
    }
}
