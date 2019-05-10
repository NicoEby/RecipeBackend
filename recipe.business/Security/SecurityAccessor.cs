using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using ch.thommenmedia.common.Interfaces;

namespace recipe.business.Security
{

    /// <summary>
    /// DUMMY Accessor => Need to be implemented
    /// </summary>
    public class SecurityAccessor : ISecurityAccessor
    {
        public IBaseIdentity CurrentIdentity { get; }
        public Guid CurrentUserid { get; }
        public IPrincipal CurrentPrincipal { get; }
        // fake authenticated to be able to use our operations
        public bool IsAuthenticated { get => true; }
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

        public void Impersonate(Guid userid, string reason = "")
        {
            throw new NotImplementedException();
        }
    }
}
