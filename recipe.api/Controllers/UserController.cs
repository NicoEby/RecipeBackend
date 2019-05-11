using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ch.thommenmedia.common.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using recipe.api.Security;
using recipe.business.Security;

namespace recipe.api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[Controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("[Action]")]
        public IActionResult Authenticate([FromBody]AuthenticateUserRequest userParam)
        {
            var user = _userService.Authenticate(userParam.Username, userParam.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok("You are logged in");
        }
    }
}
