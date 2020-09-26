using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAPI_Catalogo.Context;
using WebAPI_Catalogo.DTOs.Mappings;
using WebAPI_Catalogo.Extensions;
using WebAPI_Catalogo.Filters;
using WebAPI_Catalogo.Logging;
using WebAPI_Catalogo.Repository;
using WebAPI_Catalogo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using System;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using System.Collections.Generic;

namespace WebAPI_Catalogo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("PermitirApiRequest",
                    builder => builder.WithOrigins
                    ("https://www.apirequest.io/").WithMethods("GET"));
            });

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme).
                AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidAudience = Configuration["TokenConfiguration:Audience"],
                    ValidIssuer = Configuration["TokenConfiguration:Issuer"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["Jwt:key"]))
                });

            //Swagger
            services.AddSwaggerGen(c =>
           {
               c.SwaggerDoc("v1", new OpenApiInfo
               {
                   Version = "v1",
                   Title = "Web Api Catalogo",
                   Description = "Catálogo de produtos e categorias",
                   TermsOfService = new Uri("https://addamreis.net/terms"),
                   Contact = new OpenApiContact
                   {
                       Name = "Addam",
                       Email = "addam@gmail.com",
                       Url = new Uri("https://addamreis.net/terms"),
                   },
                   License = new OpenApiLicense
                   {
                       Name = "Usar sobre LICX",
                       Url = new Uri("https://addamreis.net/license")
                   }
               });

               //configurações para exibir os <summary's> das api's no Swagger
               var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
               var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
               c.IncludeXmlComments(xmlPath);

               //configurações para debug utilizando swagger
               var security = new Dictionary<string, IEnumerable<string>>
               {
                   {"Bearer", new string[] { }},
               };

               c.AddSecurityDefinition(
                   "Bearer",
                   new OpenApiSecurityScheme
                   {
                       In = ParameterLocation.Header,
                       Description = @"Copiar 'bearer' + token'",
                       Name = "Authorization",
                       Type = SecuritySchemeType.ApiKey,
                       Scheme = "Bearer"
                   });

               c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                      {
                          new OpenApiSecurityScheme
                              {
                                Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                     },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,
                                },
                          new List<string>()
                      }
                  });
           });

            services.AddApiVersioning(opt =>
            {
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
            });

            services.AddTransient<IMeuServico, MeuServico>(); //AddTransient é criado toda vez que for solicitado, AddScoped é criado uma única vez

            services.AddScoped<ApiLoggingFilter>();

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; //vai ignorar a referência ciclica na serialização na resposta json no método action
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //loggerFactory.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
            //{
            //    LogLevel = LogLevel.Information
            //})); ;

            app.ConfigureExceptionHandler();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            //Swagger
            app.UseSwagger();

            //SwaggerUI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catálogo de Produtos e Categorias"); //definindo endpoint do swagger
            });

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
