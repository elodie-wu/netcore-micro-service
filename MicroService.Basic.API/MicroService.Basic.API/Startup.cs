using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using MicroService.Basic.Abstraction;
using MicroService.Basic.Application;
using MicroService.Basic.Data.DBContext;
using MicroService.Basic.Domain.IRepository;
using MicroService.Basic.Repository;
using MicroService.Common.JWT;
using MicroService.Common.Operator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySql.Data.EntityFrameworkCore.Extensions;
using Swashbuckle.AspNetCore.Filters;

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

            #region 注入

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

            #region jwt官方认证 
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

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
