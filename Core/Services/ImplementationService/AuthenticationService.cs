using AutoMapper;
using Domain.Entities.IdentityModule;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Services.Abstractions.Contracts;
using Shared.Common;
using Shared.DTOs.IdentityModule;
using Shared.DTOs.OrderModule;
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
        private readonly IOptions<JwtOptions> _options;
        private readonly IMapper _mapper;

        public AuthenticationService( UserManager<User> userManager , IOptions<JwtOptions> options , IMapper mapper)
        {
            _userManager = userManager;
            _options = options;
            _mapper = mapper;
        }
        // Check Email Exist
        public async Task<bool> CheckEmailExistAsync(string userEmail)
        {
           var user = await _userManager.FindByEmailAsync(userEmail);
            if( user is null)
                return false;
            else
                return true;
        }
        // Get Current User
        public async Task<UserResultDto> GetCurrentUserAsync(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if( user is null)
                throw new UserNotFoundExceptions(userEmail);
            
            return new UserResultDto(user.DisplayName , await CreateTokenAsync(user) , user.Email);
            
        }
        // Get User Address
        public async Task<ShippingAddressDto> GetUserAddressAsync(string userEmail)
        {
            //  specifications عشان انا معنديش Include عن طريق ال user مع ال Address احنا هنا رجعنا ال
            //  اللي هو باعتو ونرجعهولو userEmail من ال FirstOrDefaultAsync عادي و هنجيب ال Include ف روحنا عملنا
            var user = await _userManager.Users.Include( user => user.Address)
                             .FirstOrDefaultAsync( u => u.Email == userEmail);

            // Email يكون معندوش ال user هنعمل اتشك ممكن ال
            if( user is null )
                throw new UserNotFoundExceptions(userEmail);

            return _mapper.Map<ShippingAddressDto>(user.Address);
        }
        // Update User Address
        public async Task<ShippingAddressDto> UpdateUserAddressAsync(string userEmail, ShippingAddressDto addressDto)
        {
            //  specifications عشان انا معنديش Include عن طريق ال user مع ال Address احنا هنا رجعنا ال
            //  اللي هو باعتو ونرجعهولو userEmail من ال FirstOrDefaultAsync عادي و هنجيب ال Include ف روحنا عملنا
            var user = await _userManager.Users.Include(user => user.Address)
                             .FirstOrDefaultAsync(u => u.Email == userEmail);

            // Email يكون معندوش ال user هنعمل اتشك ممكن ال
            if (user is null)
                throw new UserNotFoundExceptions(userEmail);

            //  Update هنعملو null لو مش ب
            if (user.Address != null)
            {
                user.Address.FirstName = addressDto.FirstName;
                user.Address.LastName = addressDto.LastName;
                user.Address.City = addressDto.City;
                user.Address.Country = addressDto.Country;
                user.Address.Street = addressDto.Street;
            }
            // Create اصلا انا هروح اعملو Address هنا بقا لو معندوش
            else
            {
                // map 
                var address = _mapper.Map<Address>(addressDto);
                // Create 
                user.Address = address;

            }
            // Create هروح بقا اضيف او اعمل
            await _userManager.UpdateAsync(user);
            // user  اللي جوا ال navigation Property من ال Map عشان دي هنعوز نعمل  OrderProfile بتاعها جوا ال MapProfile هروح اعمل ال
            return _mapper.Map<ShippingAddressDto>(user.Address);
        }

        // Login
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
        // Register
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
            // Options 
            var jwtOptions = _options.Value;

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

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));

            // Algorithm Or signInCreds => اللي هنشتغل بيه Algorithm و ال Key  دا بياخد مننا ال
            // SignIngCredentials من الكلاس دا Create Instance لازم عشان نعملو نعمل
            //  اللي هنشتغل بيه Algorithm ال SecurityAlgorithms.HmacSha256 دا
            var signInCreds = new SigningCredentials( Key , SecurityAlgorithms.HmacSha256 );

            var token = new JwtSecurityToken(issuer: jwtOptions.Issuer, audience: jwtOptions.Audience, claims: claims, expires: DateTime.UtcNow.AddDays(jwtOptions.ExpirationInDays), signingCredentials: signInCreds);
            // JwtSecurityTokenHandler من الكلاس دا Create Instance دي عشان اوصل ليها لازم اعمل WriteToken كمان اسمهاMethod محتاج اوصل ل Token عشان اعرف ارجع ال JwtSecurityToken بترجع token و دي Token انا عايز ارجع 

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
