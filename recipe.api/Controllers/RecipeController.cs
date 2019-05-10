using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using recipe.business.Operations.Recipe;

namespace recipe.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        public List<data.Models.Recipe> Get()
        {
            return new GetRecipesOperation().Execute(new GetRecipesOperationInput()).ToList();
        }
    }
}