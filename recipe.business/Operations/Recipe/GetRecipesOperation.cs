using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ch.thommenmedia.common.Interfaces;
using ch.thommenmedia.common.Operation;
using recipe.data.Models;

namespace recipe.business.Operations.Recipe
{
    public class GetRecipesOperation : RecipeOperationBase<GetRecipesOperationInput, IQueryable<data.Models.Recipe>>
    {
        public RecipeContext Context = new RecipeContext();

        protected override IQueryable<data.Models.Recipe> Execute(GetRecipesOperationInput input)
        {
            var query = Context.GetRecipes.AsQueryable();
            if (input.Id != null)
            {
                query = query.Where(q => q.Id == input.Id);
            }
            return query;
        }

        public GetRecipesOperation(ISecurityAccessor securityAccessor) : base(securityAccessor)
        {
        }
    }

    public class GetRecipesOperationInput
    {
        public Guid? Id { get; set; }


    }
}
