using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using ch.thommenmedia.common.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using recipe.api.DTO;
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
            app.UseMvc();
        }
    }
}
