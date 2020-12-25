using AutoMapper;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.MySql;
using MicroService.Basic.Abstraction;
using MicroService.Basic.Application;
using MicroService.Basic.Data.DBContext;
using MicroService.Basic.Domain.IRepository;
using MicroService.Basic.DTO.AutoMapper;
using MicroService.Basic.Repository;
using MicroService.Common.Operator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySql.Data.EntityFrameworkCore.Extensions;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Basic.API
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
            services.AddMvc();

            #region di

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();
            services.AddSingleton<IOperatorProvider, OperatorProvider>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #endregion

            #region efcore

            services.AddEntityFrameworkMySQL().AddDbContext<BasicDBContext>((serviceProvider, options) =>
            {
                options.UseMySql(Configuration.GetSection("DbConfig:Mysql:ConnectionString").Value);
            });
            #endregion

            #region swagger 

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                var filePath = Path.Combine(System.AppContext.BaseDirectory, "MicroService.Basic.API.xml");
                c.IncludeXmlComments(filePath);

                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                c.OperationFilter<SecurityRequirementsOperationFilter>();

                #region 开启授权
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT授权Bearer",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                #endregion
            });
            #endregion

            #region jwt
            var audienceConfig = Configuration.GetSection("Audience");
            var symmetricKeyAsBase64 = "asjdhfjkasdhkflhkashd";
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = audienceConfig["Issuer"],//发行人
                ValidateAudience = true,
                ValidAudience = audienceConfig["Audience"],//订阅人
                ValidateLifetime = true,
                RequireExpirationTime = true,
            };

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(o =>
             {
                 o.TokenValidationParameters = tokenValidationParameters;
                 o.Events = new JwtBearerEvents
                 {
                     OnAuthenticationFailed = context =>
                     {
                         if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                         {
                             context.Response.Headers.Add("Token-Expired", "true");
                         }
                         return Task.CompletedTask;
                     }
                 };
             });

            #endregion

            #region automapper
            services.AddAutoMapper(typeof(AutoMapperConfig));
            #endregion

            #region cors
            services.AddCors(options =>
            {
                options.AddPolicy("LimitRequests", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
            #endregion

            #region hangfire
            // Add Hangfire services. 
            services.AddHangfire(x => x.UseStorage(
                new MySqlStorage(
                    Configuration["DbConfig:Mysql:ConnectionString"],
                    new MySqlStorageOptions
                    {
                        TransactionIsolationLevel = (System.Transactions.IsolationLevel?)IsolationLevel.ReadCommitted,                           // 事务隔离级别。默认是读取已提交。
                        QueuePollInterval = TimeSpan.FromSeconds(15),             // 作业队列轮询间隔。默认值为15秒。
                        JobExpirationCheckInterval = TimeSpan.FromHours(1),       // 作业到期检查间隔（管理过期记录）。默认值为1小时。
                        CountersAggregateInterval = TimeSpan.FromMinutes(5),      // 聚合计数器的间隔。默认为5分钟。
                        PrepareSchemaIfNecessary = true,                          // 如果设置为true，则创建数据库表。默认是true。
                        DashboardJobListLimit = 50000,                            // 仪表板作业列表限制。默认值为50000。
                        TransactionTimeout = TimeSpan.FromMinutes(1),             // 交易超时。默认为1分钟。
                        TablesPrefix = "Hangfire"                                 // 数据库中表的前缀。默认为none
                    }
            )));
            // Add the processing server as IHostedService
            //services.AddHangfireServer();

            #endregion

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IBackgroundJobClient backgroundJobs, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region swagger

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            #endregion

            #region hangfire
            //You can limit worker count by setting WorkerCount property value in BackgroundJobServerOptions
            app.UseHangfireServer(
               new BackgroundJobServerOptions
               {
                   WorkerCount = 1
               }); 
            app.UseHangfireDashboard();
            backgroundJobs.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));

            #endregion 

            app.UseHttpsRedirection();

            app.UseRouting();

            #region cors 
            app.UseCors("LimitRequests");
            #endregion

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });
        }
    }
}
