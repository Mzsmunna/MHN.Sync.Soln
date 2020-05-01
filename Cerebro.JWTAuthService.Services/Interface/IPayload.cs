namespace Cerebro.JWTAuthService.Services.Interface
{
    public interface IPayload
    {
        string unique_name { get; set; }
        string clientId { get; set; }
        string company { get; set; }
    }
}
