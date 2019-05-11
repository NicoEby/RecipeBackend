using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace ch.thommenmedia.common.Interfaces
{
    public interface IBaseIdentity : IIdentity
    {
        /// <summary>
        /// mostly the userId
        /// </summary>
        Guid Id { get; set; }
        string UserName { get; set; }
        string Token { get; set; }
    }
}
