using System;
using System.Collections.Generic;
using System.Text;
using ch.thommenmedia.common.Interfaces;

namespace recipe.business.Security
{
    public class RecipeIdentity : IBaseIdentity
    {
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public string AuthenticationType { get; }

        /// <inheritdoc />
        /// <summary>
        ///     indicates that the user is authenticate
        /// </summary>
        public bool IsAuthenticated => Id != Guid.Empty;

        public string Name => UserName;

        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; } // the token
    }
}
