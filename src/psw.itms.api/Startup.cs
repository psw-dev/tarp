using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using AutoMapper;


// using PSW.ITMS.Resources;
using PSW.ITMS.Service;
using PSW.ITMS.Service.AutoMapper;

using PSW.ITMS.Data;
using PSW.ITMS.Data.Sql;

using PSW.Lib.Consul;
// using PSW.RabbitMq;
// using PSW.ITMS.RabbitMq;



namespace PSW.ITMS.Api
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
            services.AddControllers();
            services.AddTransient<IItmsService, ItmsService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();            
            
            // Auto Mapper Profiles 
            services.AddAutoMapper(
                typeof(DTOToEntityMappingProfile), 
                typeof(EntityToDTOMappingProfile)
            );

            
            services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.IgnoreNullValues = true;
                    });;
           
           
            // services.AddSingleton<IEventBus, RabbitMqBus>(s => {
            //         var lifetime = s.GetRequiredService<IHostApplicationLifetime>();
            //         return new RabbitMqBus(lifetime, Configuration);
            //     });

            services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy",
                        builder => builder.SetIsOriginAllowed(_ => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
                });

            services.AddConsul(Configuration);
            services.AddHealthChecks();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IUnitOfWork unitOfWork, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            string UseConsulDev = Configuration.GetSection("UseConsulDev").Value;

            if(UseConsulDev.ToLower() == "true")
            {
                app.UseConsul(lifetime);
            }

            app.UseCors("CorsPolicy");


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            // var component = app.ApplicationServices.GetRequiredService<IEventBus>();
            // var upsRabbitMqListener = new UPSRabbitMqListener(OgaService, unitOfWork, Configuration);
            // component.Subscribe(MessageQueues.UPSQueue, upsRabbitMqListener, Configuration, eventBus);
        }
    }
}
