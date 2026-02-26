using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions.Contracts;
using Shared.DTOs.IdentityModule;
using Shared.DTOs.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        // Get => Check Email Exist
        [HttpGet("EmailExist")]
        public async Task<ActionResult<bool>> CheckEmailExistAsync(string email)
        {
            var useremail = await _serviceManager.AuthenticationService.CheckEmailExistAsync(email);
            return Ok(useremail);
        }

        // Get => Get Current User
        [Authorize] // عشا يوصل ل دي Token لازم يكون معاه 
        [HttpGet]
        public async Task<ActionResult<UserResultDto>> GetCurrentUserAsync()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _serviceManager.AuthenticationService.GetCurrentUserAsync(Email);

            return Ok(user);
        }

        // Get ==> Get User Address
        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<ShippingAddressDto>> GetUserAddressAsync()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _serviceManager.AuthenticationService.GetUserAddressAsync(email);
            return Ok(user);
        }

        // PUT ==> Update Or Create Address
        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<ShippingAddressDto>> UpdateUserAddressAsync(ShippingAddressDto addressDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var address = await _serviceManager.AuthenticationService.UpdateUserAddressAsync(email, addressDto);
            return Ok(address);
        }
        
    }
}
