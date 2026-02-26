using Shared.DTOs.IdentityModule;
using Shared.DTOs.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.Contracts
{
    public interface IAuthenticationService
    {
        // Login => UserResultDto ( Email , DisplayName , Token)
        //       ==> Take Parameters [ Email , Password ]

        Task<UserResultDto> LoginAsync(LoginDTO loginDTO);

        // Register => UserResultDto ( Email , DisplayName , Token)
        //         ==> Take Parameters [ Email , Password , PhoneNumber , UserName , DisplayName ]

        Task<UserResultDto> RegisterAsync(RegisterDTO registerDTO);

        // Get Current User ==> Return <UserResultDto>(displayName , Email , Token)
        //                  ==> Take < string userEmail>
        Task<UserResultDto> GetCurrentUserAsync(string userEmail);

        // Check If Email Exist ==> Return <bool> ==> Take <string userEmail>
        Task<bool> CheckEmailExistAsync(string userEmail);

        // Get User Address ==> Return <ShippingAddressDto> -- Take ==> <string userEmail>
        Task<ShippingAddressDto> GetUserAddressAsync(string userEmail);

        // Update User Address Or Create ==> Return <ShippingAddressDto>  
                                      // Take ==> ( string userEmail , ShippingAddressDto addressDto )
        Task<ShippingAddressDto> UpdateUserAddressAsync(string userEmail , ShippingAddressDto addressDto);
    }
}
