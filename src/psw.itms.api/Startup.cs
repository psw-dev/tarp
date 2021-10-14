using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using PSW.ITMS.Data;
using PSW.ITMS.Data.Sql;
// using PSW.ITMS.Resources;
using PSW.ITMS.Service;
using PSW.ITMS.Service.AutoMapper;
using PSW.Lib.Consul;
using PSW.RabbitMq;
using PSW.ITMS.RabbitMq;
using PSW.Common.Crypto;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;


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
                    }); ;


            services.AddSingleton<IEventBus, RabbitMqBus>(s =>
            {
                var lifetime = s.GetRequiredService<IHostApplicationLifetime>();
                return new RabbitMqBus(lifetime, Configuration);
            });

            //--- This Section is for Securing API (via IdentityServer) ---------------------------------------------
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Environment.GetEnvironmentVariable("ASPNETCORE_IDENTITY_SERVER_ISSUER");
                    //options.Authority = "http://localhost:4000";
                    options.ApiName = "auth"; // will be IRMS
                    options.ApiSecret = "auth"; //will be IRMS
                    options.RequireHttpsMetadata = false;
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine("OnAuthenticationFailed: " +
                                              context.Exception.Message);
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            Console.WriteLine("OnTokenValidated: " +
                                              context.SecurityToken);
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("authorizedUserPolicy", policyAdmin =>
                {
                    policyAdmin.RequireClaim("client_id", "psw.client.spa");
                });
            });

            string salt = Environment.GetEnvironmentVariable("ENCRYPTION_SALT");
            string password = Environment.GetEnvironmentVariable("ENCRYPTION_PASSWORD");

            if (string.IsNullOrWhiteSpace(salt) || string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Please provide salt and password for Crypto Algorithm in Environment Variable");
            }

            services.AddSingleton<IAppSettingsProcessor>(_ => new AppSettingsDecrypter<AesManaged>(_.GetService<IConfiguration>(),
               password,
               salt));

            services.AddScoped<ICryptoAlgorithm>(x =>
             {
                 return new CryptoFactory().Create<AesManaged>(password, salt);
             });
             
            services.AddSingleton<IAppSettingsProcessor>(_ => new AppSettingsDecrypter<AesManaged>(_.GetService<IConfiguration>(),
                                                                         "pass",
                                                                         "random"));

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
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IItmsService itmsService, IUnitOfWork unitOfWork, IEventBus eventBus, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                IdentityModelEventSource.ShowPII = true;  //remove afterwards
            }



            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            string UseConsulDev = Configuration.GetSection("UseConsulDev").Value;

            if (UseConsulDev.ToLower() == "true")
            {
                app.UseConsul(lifetime);
            }

            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapHealthChecks("/health");
            });

            var component = app.ApplicationServices.GetRequiredService<IEventBus>();
            var ITMSRabbitMqListener = new ITMSRabbitMqListener(itmsService, unitOfWork, Configuration);
            component.Subscribe(MessageQueues.TARPQueue, ITMSRabbitMqListener, Configuration, eventBus);

        }
    }
}
