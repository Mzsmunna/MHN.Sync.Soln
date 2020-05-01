using System.Collections.Generic;
using System.Security.Claims;

namespace Cerebro.JWTAuthService.Services.Interface
{
    internal interface IAuthService
    {
        string SecretKey { get; set; }

        bool IsTokenValid(string token);
        string GenerateToken(IAuthContainerModel model);
        IEnumerable<Claim> GetTokenClaims(string token);
    }
}
