namespace Cerebro.JWTAuthService.Services.Models
{
    public class FTPCredential
    {
        string _userName;
        string _password;

        public string Host { get; set; }
        public string Port { get; set; }
        public string UserName
        {
            get => Cryptography.Cryptography.Decrypt(_userName);
            set => _userName = value;
        }
        public string Password
        {
            get => Cryptography.Cryptography.Decrypt(_password);
            set => _password = value;
        }
    }
}
