using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using ch.thommenmedia.common.Interfaces;
using Microsoft.IdentityModel.Tokens;
using recipe.business.Security;

namespace recipe.api.Security
{
    public class TokenValidator : ISecurityTokenValidator
    {
        private readonly IServiceProvider _serviceProvider;
        public TokenValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public bool CanReadToken(string securityToken)
        {
            return true;
        }

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters,
            out SecurityToken validatedToken)
        {
            var validator = new JwtSecurityTokenHandler();
            var claim = validator.ValidateToken(securityToken, validationParameters, out validatedToken);
            var sa = _serviceProvider.GetService(typeof(ISecurityAccessor)) as ISecurityAccessor;
            sa.AuthenticateUser(securityToken); // authenticate request
            return claim;
        }

        public bool CanValidateToken => true;
        public int MaximumTokenSizeInBytes { get; set; }
    }
}
