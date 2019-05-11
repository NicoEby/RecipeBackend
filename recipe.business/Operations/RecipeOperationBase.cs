using System;
using System.Collections.Generic;
using System.Text;
using ch.thommenmedia.common.Interfaces;
using ch.thommenmedia.common.Operation;

namespace recipe.business.Operations
{
    public abstract class RecipeOperationBase<TInput, TOutput> : OperationBase<TInput, TOutput>
    {
        protected RecipeOperationBase(ISecurityAccessor securityAccessor) : base(securityAccessor)
        {
        }

        /// <summary>
        /// must be implemented from the consuming class
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected abstract override TOutput Execute(TInput input);

        /// <summary>
        /// can be overwritten but normally handels the inputless requests
        /// </summary>
        /// <returns></returns>
        protected override TOutput Execute()
        {
            // fake executor (empty input) => input must support empty constructor
            return Execute(Activator.CreateInstance<TInput>());
        }

        /// <summary>
        /// checks the basic if a user is authenticated
        /// </summary>
        /// <returns></returns>
        protected override bool Authorize()
        {
            return SecurityAccessor.IsAuthenticated;
        }
    }
}
