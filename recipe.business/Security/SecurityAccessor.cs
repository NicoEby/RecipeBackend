using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Security;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using ch.thommenmedia.common.Extensions;
using ch.thommenmedia.common.Interfaces;
using ch.thommenmedia.common.Security;
using ch.thommenmedia.common.Setting;
using ch.thommenmedia.common.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace recipe.business.Security
{

    /// <summary>
    /// DUMMY Accessor => Need to be implemented
    /// </summary>
    public class SecurityAccessor : ISecurityAccessor
    {

        /// <summary>
        ///  fake token store until we have a database
        /// </summary>
        private static List<RecipeIdentity> TokenStore = new List<RecipeIdentity>();

        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private static List<AuthenticateUserRequest> _users = new List<AuthenticateUserRequest>
        {
            new AuthenticateUserRequest { Id = Guid.NewGuid(), Username = "test", Password = "test" },
            new AuthenticateUserRequest { Id = Guid.NewGuid(), Username = "admin", Password = "admin" }
        };


        private readonly IApplicationSettingDbProvider _settings;
        private readonly IServiceProvider _serviceProvider;
        private readonly Guid _instanceId;
        public SecurityAccessor(IServiceProvider provider)
        {
            _serviceProvider = provider;
            _settings = provider.GetService(typeof(IApplicationSettingDbProvider)) as IApplicationSettingDbProvider;
            _instanceId = Guid.NewGuid();

            CheckHttpContextAuthenticated();
        }

        public byte[] Secret => string.IsNullOrEmpty(_settings?.GetConfigStringValue("System.Secret", ignoreError: true)) ?
            Encoding.ASCII.GetBytes("39sdfjäakjwtliauwefaishflashf") : _settings.GetConfigStringValue("System.Secret", ignoreError: true).ToByteArray();

        /// <summary>
        /// if logged in there is an identity stored here
        /// </summary>
        public IBaseIdentity CurrentIdentity { get; set; }
        public Guid CurrentUserid => CurrentIdentity?.Id ?? Guid.Empty;
        public bool IsAuthenticated => CurrentIdentity != null;

        /// <summary>
        /// authenticates a user by name and password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IBaseIdentity AuthenticateUser(AuthenticateUserRequest request)
        {
            var user = GetUsers().FirstOrDefault(q => q.Username == request.Username && q.Password == request.Password);
            if (user == null)
            {
                throw new Exception("Authentication failed");
            }
            else
            {
                return Authenticated(user);
            }
        }

        /// <summary>
        ///  fake get users method because we need thie more than once
        /// </summary>
        /// <returns></returns>
        private List<AuthenticateUserRequest>  GetUsers()
        {
            return _users;
        }

        /// <summary>
        /// authenticates a user by token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public IBaseIdentity AuthenticateUser(string token)
        {
            var user = TokenStore.FirstOrDefault(q => q.Token == token);
            if (user == null)
            {
                throw new Exception("TokenAuthentication failed");
            }
            else
            {
                return Authenticated(user);
            }
        }

        /// <summary>
        /// set current request as authorized
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private IBaseIdentity Authenticated(AuthenticateUserRequest user)
        {
            Authenticated(new RecipeIdentity()
            {
                Id = user.Id,
                UserName = user.Username,
                Token = new JwtSecurityTokenHandler().WriteToken(CreateToken(user.Id.ToString()))
            });
            // add to fake tokenstore
            TokenStore.Add((RecipeIdentity) CurrentIdentity);
            return CurrentIdentity;
        }

        /// <summary>
        /// set current request as authorized
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private IBaseIdentity Authenticated(RecipeIdentity user)
        {
            CurrentIdentity = user;
            return CurrentIdentity;
        }

        /// <summary>
        /// creates a jwt token
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal JwtSecurityToken CreateToken(string userId)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            };

            var validUntil =
                DateTimeOffset.Now.AddDays(1);

            return new JwtSecurityToken(
                //audience: AppSettingsHelper.GetConfigStringValue("System.Auth.Audience"),
                "System",
                expires: validUntil.DateTime,
                claims: claims,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Secret), SecurityAlgorithms.HmacSha512Signature)
            );
        }

        /// <summary>
        /// logs in the user as the provided userid
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="reason"></param>
        public void Impersonate(Guid userid, string reason = "")
        {
            Authenticated(GetUsers().First(q => q.Id == userid));
        }


        /// <summary>
        /// checks if the user is logged in on the http context
        /// if true we login the user
        /// </summary>
        private void CheckHttpContextAuthenticated()
        {
            // we use here the impersonate because when we have the httpcontext, the  login procedure allready is done
            if (_serviceProvider.GetService(typeof(IHttpContextAccessor)) is IHttpContextAccessor context 
                    && context.HttpContext.User.Identity != null
                    && context.HttpContext.User.Claims.Any(q => q.Type == ClaimTypes.NameIdentifier)
                )
                Impersonate(Guid.Parse(context.HttpContext.User.Claims.SingleOrDefault(q => q.Type == ClaimTypes.NameIdentifier)?.Value), "HttpContext Authenticated");
        }


        #region TOBEIMPLEMENTED

        public IPrincipal CurrentPrincipal { get; }
        // fake authenticated to be able to use our operations
        
        public string ObjectPrivilegeName(Type entityType)
        {
            throw new NotImplementedException();
        }

        public string ObjectPrivilegeName(string objectName)
        {
            throw new NotImplementedException();
        }

        public bool HasAccess(string privilegeName, IAccessRight r = null)
        {
            throw new NotImplementedException();
        }

        public bool HasAccess(Type t, IAccessRight r)
        {
            throw new NotImplementedException();
        }

        public bool HasAccess(IPrivilege p, IAccessRight r = null)
        {
            throw new NotImplementedException();
        }

        public bool HasAccess(IPrivilegeBase p, IAccessRight r = null)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> FilterQuery<TEntity>(IQueryable<TEntity> query) where TEntity : class, IEntityBase
        {
            throw new NotImplementedException();
        }

        public bool CheckEncodedPassword(string password, string encodedPassword)
        {
            throw new NotImplementedException();
        }

        public string EncodePassword(string password)
        {
            throw new NotImplementedException();
        }

        public string MapUser(Guid? userId)
        {
            throw new NotImplementedException();
        }

        

      

        #endregion
    }
}
