using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddResponseCaching(options =>
            {
                //�Ƿ���������·����Сд
                options.UseCaseSensitivePaths = false;
            });
            #region swagger
            services.AddSwaggerGen(a =>
            {
                a.SwaggerDoc("V1", new OpenApiInfo
                {
                    Version = "",
                    Title = "",
                    Description = "˵���ĵ�"
                });

                //��xmlע��չʾ�ڽ���
                var xmlPath = AppDomain.CurrentDomain.BaseDirectory + "ZjkBlog.WebApi.xml";
                a.IncludeXmlComments(xmlPath);

                #region ��swagger�����JWT��֤
                //JWT�������
                a.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "���·������������ͷ��Ҫ���JWT��ȨToken����ʽΪ ��Bearer {Token}",
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
            #region ע��JwT��֤
            // AddAuthorization ��ӻ�����Ȩ�ķ���
            // AddAuthentication ��֤��Ȩģʽ�Ƿ�ΪJwt Bearer   
            // AddJwtBearer Jwt��Ϣ��֤
            services
            //.AddAuthorization(Options =>
            //{
            //    Options.AddPolicy("permission",policy=> policy.Requirements.Add(new PolicyRequirement()))
            //}) 
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                //��ȡappsettings����ֵ ��һ�ַ�ʽ��������ӦModel
                var jwtmodel = Configuration.GetSection(nameof(JwtIssuerOptions));
                var iss = jwtmodel[nameof(JwtIssuerOptions.Issuer)];
                var key = jwtmodel[nameof(JwtIssuerOptions.SecurityKey)];
                var audience = jwtmodel[nameof(JwtIssuerOptions.Audience)];

                //�ڶ��֣�ͨ���ڵ��ַ
                //var iss = Configuration["JwtIssuerOptions:Issuer"];
                //var key = Configuration["JwtIssuerOptions:SecurityKey"];
                //var audience = Configuration["JwtIssuerOptions:Audience"];

                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,//�Ƿ���֤Issuer
                    ValidateAudience = true,//�Ƿ���֤Audience
                    ValidateLifetime = true,//�Ƿ���֤ʧЧʱ��
                    ClockSkew = TimeSpan.FromSeconds(30), //ע�����ǻ������ʱ�䣬�ܵ���Чʱ��������ʱ�����jwt�Ĺ���ʱ�䣬��������ã�Ĭ����5����
                    ValidateIssuerSigningKey = true,//�Ƿ���֤SecurityKey
                    ValidAudience = audience,//Audience
                    ValidIssuer = iss,//Issuer���������ǰ��ǩ��jwt������һ��
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))//�õ�SecurityKey
                };
            });
            #endregion

            services.AddCors(options =>
            {
                //ȫ��������
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                    });

                options.AddPolicy("AnotherPolicy",
                    builder =>
                    {
                        builder.WithOrigins("")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });

            });
            //������ݿ����ط���
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
            else
            {
                app.UseHsts();
            }
            //�����֤�м��(�ȿӣ���Ȩ�м����������֤�м��֮ǰ)
            app.UseAuthentication();
            //�����м����������Swagger��ΪJSON�ս��
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
            app.UseResponseCaching();
            app.UseAuthorization();
            //ʹ�� Cors
            app.UseCors();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
