using System;
using Microsoft.IdentityModel.Tokens;

namespace recipe.api.Security
{
    public class RecipeSecurityToken : SecurityToken
    {
        public RecipeSecurityToken(string id, string issuer, SecurityKey securityKey, SecurityKey signingKey, DateTime validFrom, DateTime validTo)
        {
            this.Id = id;
            this.Issuer = issuer;
            this.SecurityKey = securityKey;
            this.SigningKey = signingKey;
            this.ValidFrom = validFrom;
            this.ValidTo = validTo;

        }

        public override string Id { get; }
        public override string Issuer { get; }
        public override SecurityKey SecurityKey { get; }
        public override SecurityKey SigningKey { get; set; }
        public override DateTime ValidFrom { get; }
        public override DateTime ValidTo { get; }
    }
}
