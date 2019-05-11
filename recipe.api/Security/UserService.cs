using ch.thommenmedia.common.Interfaces;
using ch.thommenmedia.common.Security;

namespace recipe.api.Security
{
    public interface IUserService
    {
        IBaseIdentity Authenticate(string username, string password);
    }

    public class UserService : IUserService
    {
        private readonly ISecurityAccessor _securityAccessor;

        public UserService(ISecurityAccessor securityAccessor)
        {
            _securityAccessor = securityAccessor;
        }

        
        public IBaseIdentity Authenticate(string username, string password)
        {
            return _securityAccessor.AuthenticateUser(new AuthenticateUserRequest()
            {
                Username = username,
                Password = password
            });
        }

    }
}
