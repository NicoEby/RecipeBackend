using System;

namespace ch.thommenmedia.common.Security
{
    /// <summary>
    /// our main user (can be extended...)
    /// </summary>
    public class AuthenticateUserRequest
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
