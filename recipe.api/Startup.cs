using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ch.thommenmedia.common.Extensions;
using ch.thommenmedia.common.Interfaces;
using ch.thommenmedia.common.Security;
using ch.thommenmedia.common.Setting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using recipe.api.DTO;
using recipe.api.Security;
using recipe.business.Helper;
using recipe.business.Security;
using recipe.data.Models;

namespace recipe.api
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddAutoMapper(
                // dto place
                Assembly.GetAssembly(typeof(Startup)),
                // entity place
                Assembly.GetAssembly(typeof(data.Models.RecipeContext))
                );
            
            
            // global dependency injection is done in the business layer (please add business stuff only there!)
            DependencyInjectionHelper.BuildServiceProvider(services);

            var sa = DependencyInjectionHelper.ServiceProvider.GetService(typeof(ISecurityAccessor)) as ISecurityAccessor;

            // enable jwt authentication
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(sa.Secret),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                    x.SecurityTokenValidators.Clear();
                    x.SecurityTokenValidators.Add(new TokenValidator(DependencyInjectionHelper.ServiceProvider));
                });

            // user service
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // configure the entity/dto automapper
            ConfigureAutoMapper();

        }

        private void ConfigureAutoMapper()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Recipe, RecipeDto>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
