using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LJGHistoryService.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace LJGHistoryService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        readonly string MyAllowSpecificOrigins = "CorsPolicy";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Set up AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "LJG API",
                    Version = "v1",
                    Description = "LJG Technical Experience API",
                    Contact = new OpenApiContact
                    {
                        Name = "Aron Gamble",
                        Email = "aron.gamble@gmail.com"
                    }
                });
            });


            services.AddCors(options =>
            {
                options.AddPolicy(
                    MyAllowSpecificOrigins,
                    builder => builder
                                    .AllowAnyOrigin()
                                    .AllowAnyHeader()
                                    .AllowAnyMethod());

                options.DefaultPolicyName = MyAllowSpecificOrigins;
            });

            /* ReturnHttpNotAcceptable
            * 
            * Gets or sets the flag which decides whether an HTTP 406 Not Acceptable response
            * will be returned if no formatter has been selected to format the response.             
            * false by default
            * 
            * AddXmlDataContractSerializerFormatters
            * 
            * Allow Accept: Application/xml withou returning '406 Not Acceptable' status
            */
            services.AddControllers(setup => { setup.ReturnHttpNotAcceptable = true; }).AddXmlDataContractSerializerFormatters();

            services.AddScoped(typeof(IContractRepository), typeof(ContractRepository));
            services.AddScoped(typeof(IAuthenticationRepository), typeof(AuthenticationRepository));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "LJG API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var supportedCultures = new[]
            {
                new CultureInfo("en-GB")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-GB"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });


            //app.UseCors(options => options.WithOrigins("http://themancave.studio/login?returnUrl=%2Fvip", "http://themancave.studio", "http://localhost:4200", "http://localhost:8069").AllowAnyMethod());

            app.UseCors(MyAllowSpecificOrigins);


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
