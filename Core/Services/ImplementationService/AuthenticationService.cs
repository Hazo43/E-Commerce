using Domain.Entities.IdentityModule;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Services.Abstractions.Contracts;
using Shared.DTOs.IdentityModule;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

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
            return new UserResultDto(user.DisplayName, await CreateTokenAsync(user), user.Email);
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
            return new UserResultDto(user.DisplayName, await CreateTokenAsync(user) , user.Email);
        }

        // Token ==> Encrypted string ==> function return string 
        // Helper Method 
        private async Task<string> CreateTokenAsync(User user)
        {
            // Claims 
            // Name  , Email , Roles 
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name , user.DisplayName),
                new Claim(ClaimTypes.Email , user.Email),
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            // Secret Key ==> SymmetricSecuritykey من الكلاس دا  Create Instance هنعمل  Secret Key عشان نعمل ال
            // Key ==> b0592a14c6e372883397b6dee7f1f4f7535563de481a9941eeb2d63658328533 
            // Array of bytes دا اللي Key هنروح نحول ال

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("b0592a14c6e372883397b6dee7f1f4f7535563de481a9941eeb2d63658328533"));

            // Algorithm Or signInCreds => اللي هنشتغل بيه Algorithm و ال Key  دا بياخد مننا ال
            // SignIngCredentials من الكلاس دا Create Instance لازم عشان نعملو نعمل
            //  اللي هنشتغل بيه Algorithm ال SecurityAlgorithms.HmacSha256 دا
            var signInCreds = new SigningCredentials( Key , SecurityAlgorithms.HmacSha256 );

            var token = new JwtSecurityToken(issuer: "https://localhost:7067", audience: "AngularProject", claims: claims, expires: DateTime.UtcNow.AddDays(30), signingCredentials: signInCreds);
            // JwtSecurityTokenHandler من الكلاس دا Create Instance دي عشان اوصل ليها لازم اعمل WriteToken كمان اسمهاMethod محتاج اوصل ل Token عشان اعرف ارجع ال JwtSecurityToken بترجع token و دي Token انا عايز ارجع 

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
