using Microsoft.AspNetCore.Mvc;
using Services.Abstractions.Contracts;
using Shared.DTOs.IdentityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class AuthenticationController : ApiController
    {
        private readonly IServiceManager _serviceManager;

        public AuthenticationController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // Post ==> Login
        [HttpPost("Login")]
        public async Task<ActionResult<UserResultDto>> LoginAsync(LoginDTO loginDTO)
        {
            var login = await _serviceManager.AuthenticationService.LoginAsync(loginDTO);
            return Ok(login);
        }

        // Post => Register
        [HttpPost("Register")]
        public async Task<ActionResult<UserResultDto>> RegisterAsync (RegisterDTO registerDTO)
        {
            var register = await _serviceManager.AuthenticationService.RegisterAsync(registerDTO);
            return Ok(register);
        }
    }
}
