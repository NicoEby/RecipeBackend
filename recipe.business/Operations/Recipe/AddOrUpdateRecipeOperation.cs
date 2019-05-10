using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using recipe.data.Models;

namespace recipe.business.Operations.Recipe
{
    public class AddOrUpdateRecipeOperation
    {
        public RecipeContext Context = new RecipeContext();
      public  data.Models.Recipe Execute(AddOrUpdateRecipeOperationInput input)
        {
            if(input.Recipe.Id == Guid.Empty)
            {
                //Add
            }
            else
            {
                //Update
            }
            return input.Recipe;
        }
    }

    public class AddOrUpdateRecipeOperationInput
    {
        public data.Models.Recipe Recipe { get; set; }

    }
}
