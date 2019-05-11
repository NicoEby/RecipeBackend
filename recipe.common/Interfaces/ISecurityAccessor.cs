using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using ch.thommenmedia.common.Security;

namespace ch.thommenmedia.common.Interfaces
{
    /// <summary>
    /// Interface for the Securitity Accesor Component
    /// </summary>
    public interface ISecurityAccessor
    {
        IBaseIdentity CurrentIdentity { get; }
        Guid CurrentUserid { get; }
        IPrincipal CurrentPrincipal { get; }
        bool IsAuthenticated { get; }
        byte[] Secret { get; }

        string ObjectPrivilegeName(Type entityType);
        string ObjectPrivilegeName(string objectName);
        bool HasAccess(string privilegeName, IAccessRight r = null);
        bool HasAccess(Type t, IAccessRight r);
        bool HasAccess(IPrivilege p, IAccessRight r = null);
        bool HasAccess(IPrivilegeBase p, IAccessRight r = null);

        IQueryable<TEntity> FilterQuery<TEntity>(IQueryable<TEntity> query)
            where TEntity : class, IEntityBase;

        bool CheckEncodedPassword(string password, string encodedPassword);
        string EncodePassword(string password);

        /// <summary>
        /// maps the userID to the username
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        string MapUser(Guid? userId);

        /// <summary>
        /// use this to impersonate a request
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="reason">the reason why you want to impersonate</param>
        void Impersonate(Guid userid, string reason = "");


        /// <summary>
        /// is used to authenticate a user
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token">the token to save if the authentication was successful</param>
        /// <returns></returns>
        IBaseIdentity AuthenticateUser(AuthenticateUserRequest request);

        /// <summary>
        /// is used to login with an access token
        /// </summary>
        /// <param name="token">the token as generated previously</param>
        /// <returns></returns>
        IBaseIdentity AuthenticateUser(string token);

    }
}