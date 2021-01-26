using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ZjkBlog.Common;
using ZjkBlog.Model;

namespace ZjkBlog.WebApi
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
            services.AddMvc();
            services.AddSession();

            #region swagger
            services.AddSwaggerGen(a =>
            {
                a.SwaggerDoc("V1", new OpenApiInfo
                {
                    Version = "",
                    Title = "",
                    Description = "说明文档"
                });

                //将xml注释展示于界面
                var xmlPath = AppDomain.CurrentDomain.BaseDirectory + "ZjkBlog.WebApi.xml";
                a.IncludeXmlComments(xmlPath);

                #region 向swagger中添加JWT验证
                //JWT相关配置
                a.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "在下方输入框中请求头需要添加JWT授权Token，格式为 ：Bearer Token",
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                a.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                     {
                         Reference=new OpenApiReference
                         {
                             Type=ReferenceType.SecurityScheme,
                             Id="Bearer"
                         }
                     }, new string[]{}
                    }

                });

                #endregion
            });
            #region 注册JwT验证
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                //获取appsettings配置值 第一种方式：建立对应Model
                var jwtmodel = Configuration.GetSection(nameof(JwtIssuerOptions));
                var iss = jwtmodel[nameof(JwtIssuerOptions.Issuer)];
                var key = jwtmodel[nameof(JwtIssuerOptions.SecurityKey)];
                var audience = jwtmodel[nameof(JwtIssuerOptions.Audience)];

                //第二种：通过节点地址
                //var iss = Configuration["JwtIssuerOptions:Issuer"];
                //var key = Configuration["JwtIssuerOptions:SecurityKey"];
                //var audience = Configuration["JwtIssuerOptions:Audience"];

                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,//是否验证Issuer
                    ValidateAudience = true,//是否验证Audience
                    ValidateLifetime = true,//是否验证失效时间
                    ClockSkew = TimeSpan.FromSeconds(30),
                    ValidateIssuerSigningKey = true,//是否验证SecurityKey
                    ValidAudience = audience,//Audience
                    ValidIssuer = iss,//Issuer，这两项和前面签发jwt的设置一致
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))//拿到SecurityKey
                };
            });
            #endregion
            //添加数据库的相关服务
            //string connectionString = Configuration.GetConnectionString("DbConnectionString");
            //services.Add(new ServiceDescriptor(typeof(DBContext), new DBContext(connectionString)));
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //身份认证中间件(踩坑：授权中间件必须在认证中间件之前)
            app.UseAuthentication();
            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/V1/swagger.json", "Web Api V1");
                // c.RoutePrefix = "";
            });
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
