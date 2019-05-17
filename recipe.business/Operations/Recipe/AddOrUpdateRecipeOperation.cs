using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ch.thommenmedia.common.Interfaces;
using recipe.data.Models;

namespace recipe.business.Operations.Recipe
{
    public class AddOrUpdateRecipeOperation : RecipeOperationBase<AddOrUpdateRecipeOperationInput, data.Models.Recipe>
    {
        public RecipeContext Context = new RecipeContext();

        public AddOrUpdateRecipeOperation(ISecurityAccessor securityAccessor) : base(securityAccessor)
        {
        }

        protected override data.Models.Recipe Execute(AddOrUpdateRecipeOperationInput input)
        {
            if (input.Recipe.Id == Guid.Empty)
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
