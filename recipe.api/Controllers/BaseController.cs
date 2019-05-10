using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ch.thommenmedia.common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace recipe.api.Controllers
{
    // will be activated later
    //[Authorize]
    public abstract class BaseController : ControllerBase
    {
        protected readonly ISecurityAccessor SecurityAccesor;
        protected readonly IServiceProvider ServiceProvider;

        protected BaseController(ISecurityAccessor securityAccessor, IServiceProvider serviceProvider)
        {
            SecurityAccesor = securityAccessor;
            ServiceProvider = serviceProvider;
        }

    }
}
