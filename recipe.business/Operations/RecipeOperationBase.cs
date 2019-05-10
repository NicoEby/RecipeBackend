using System;
using System.Collections.Generic;
using System.Text;
using ch.thommenmedia.common.Interfaces;
using ch.thommenmedia.common.Operation;

namespace recipe.business.Operations
{
    public class RecipeOperationBase<TInput, TOutput> : OperationBase<TInput, TOutput>
    {
        public RecipeOperationBase(ISecurityAccessor securityAccessor) : base(securityAccessor)
        {
        }

        protected override TOutput Execute(TInput input)
        {
            throw new NotImplementedException();
        }

        protected override TOutput Execute()
        {
            // fake executor (empty input) => input must support empty constructor
            return Execute(Activator.CreateInstance<TInput>());
        }

        protected override bool Authorize()
        {
            return SecurityAccessor.IsAuthenticated;
        }
    }
}
