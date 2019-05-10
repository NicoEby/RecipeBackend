using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using recipe.data.Models;

namespace recipe.business.Operations.Recipe
{
    public class GetRecipesOperation
    {
        public RecipeContext Context = new RecipeContext();
      public  IQueryable<data.Models.Recipe> Execute(GetRecipesOperationInput input)
        {
            var query = Context.GetRecipes.AsQueryable();
            if(input.Id != null)
            {
                query = query.Where(q => q.Id == input.Id);
            }
            return query;
        }
    }

    public class GetRecipesOperationInput
    {
        public Guid? Id { get; set; }

    }
}
