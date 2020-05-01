using System.Collections.Generic;

namespace Cerebro.JWTAuthService.Services.Models
{
    public class EmailConfig
    {
        string _userName;
        string _password;

        public string Host { get; set; }
        public int Port { get; set; }
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
        public bool EnableSSL { get; set; }
        public List<Field> OptionalContent { get; set; }
    }
}
