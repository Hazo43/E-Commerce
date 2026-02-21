using Domain.Entities.IdentityModule;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Services.Abstractions.Contracts;
using Shared.DTOs.IdentityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ImplementationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;

        public AuthenticationService( UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<UserResultDto> LoginAsync(LoginDTO loginDTO)
        {
           // Check Email Already Exist Or No
           var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user is null) // موجود هروح اكمل بقا user دي معناها ان ال if لو مدخلش جوا ال
                throw new UnauthorizedException();
           // Check Password
           var checkPassword = await _userManager.CheckPasswordAsync( user , loginDTO.Password );
           if(!checkPassword) // مش صح ف انت كدا مدخل باسورد غلط checkPassword لو ال
                throw new UnauthorizedException();
            // لو عدا من كل دول يبقي روح رجعلو الداتا دي بقا كدا هو تمام معندوش حاجه غلط
            return new UserResultDto(user.DisplayName, "Token", user.Email);
        }

        public async Task<UserResultDto> RegisterAsync(RegisterDTO registerDTO)
        {
           var user = new User()
           {
               DisplayName = registerDTO.DisplayName,
               Email = registerDTO.Email,
               UserName = registerDTO.DisplayName,
               PhoneNumber = registerDTO.PhoneNumber,
           };  
           var result = await _userManager.CreateAsync(user , registerDTO.Password);
            // Validation 
            if(result.Succeeded == false)
            {
                // و يرجع الايرور errors هيخش هنا ف ال result لل CreateAsync لو معرفش يعمل
                var errors = result.Errors.Select( e => e.Description).ToList();
                throw new VlaidationException(errors);
            }
            return new UserResultDto(user.DisplayName, "Token", user.Email);
        }
    }
}
