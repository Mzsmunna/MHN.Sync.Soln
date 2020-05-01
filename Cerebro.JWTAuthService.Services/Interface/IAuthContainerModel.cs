using System.Security.Claims;

namespace Cerebro.JWTAuthService.Services.Interface
{
    internal interface IAuthContainerModel
    {
        string SecretKey { get; set; }
        string SecurityAlgorithm { get; set; }
        int ExpireMinutes { get; set; }

        Claim[] Claims { get; set; }
    }
}
