using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Ecom.Catalog.Service.Entities;
using Ecom.Api.Settings;
using Ecom.Api.MongoDB;
using MassTransit;
using Ecom.Api.MassTransit;

namespace Ecom.Catalog.Service
{
    public class Startup
    {
        private const string AllowedOriginSettings = "AllowedOrigin";
        private ServiceSettings serviceSettings;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            serviceSettings = Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

            services.AddMongo()
                .AddMongoRepository<Item>("items")
                .AddMassTransitWithRabbitMq();

                
            services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            }
            );
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Ecom.Catalog.Service",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Shayne Boyer",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/spboyer"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
           {
               c.SerializeAsV2 = true;
           });
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.)
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                // c.RoutePrefix = string.Empty;
            });

// Uncomment in dev environment
            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();

            //     // Conguring CORS middleware to prevent errors from front end requests
            //     app.UseCors(builder =>
            //     {
            //         builder.WithOrigins(Configuration[AllowedOriginSettings]).AllowAnyHeader().AllowAnyMethod();
            //     });
            // }
            app.UseCors(builder =>
                {
                    builder.WithOrigins(Configuration[AllowedOriginSettings]).AllowAnyHeader().AllowAnyMethod();
                });

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
