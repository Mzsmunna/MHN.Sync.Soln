using Microsoft.IdentityModel.Tokens;
using Cerebro.JWTAuthService.Services.Interface;
using System.Security.Claims;

namespace Cerebro.JWTAuthService.Services.Models
{
    internal class AuthContainerModel : IAuthContainerModel
    {
        public string SecretKey { get; set; }
        public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;
        public int ExpireMinutes { get; set; } = 60 * 24 * 30;
        public Claim[] Claims { get; set; }
    }
}
