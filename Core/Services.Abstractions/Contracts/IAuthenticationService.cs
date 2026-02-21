using Shared.DTOs.IdentityModule;
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
    }
}
