using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using ch.thommenmedia.common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using recipe.api.DTO;
using recipe.business.Operations.Recipe;

namespace recipe.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : BaseController
    {
        public RecipeController(ISecurityAccessor securityAccessor, IServiceProvider serviceProvider) : base(securityAccessor, serviceProvider)
        {
            
        }

        public List<RecipeDto> Get()
        {
            var getter = ActivatorUtilities.CreateInstance<GetRecipesOperation>(ServiceProvider);
            return getter.Start().ProjectTo<RecipeDto>().ToList();
        }

        
    }
}