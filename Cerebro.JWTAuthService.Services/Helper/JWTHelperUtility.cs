 using Newtonsoft.Json;
using Cerebro.JWTAuthService.Services.Interface;
using Cerebro.JWTAuthService.Services.Models;
using Cerebro.JWTAuthService.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Net;

namespace Cerebro.JWTAuthService.Services.Helper
{
    public class JWTHelperUtility
    {
        readonly string _secretKey;
        readonly IPayload _payload;

        string GetToken<T>(string secretKey, T payload) where T : IPayload
        {
            var claims = new List<Claim>();
            typeof(T).GetProperties().ToList().ForEach(x =>
            {
                claims.Add(new Claim(x.Name, x.GetValue(payload, null)?.ToString()));
            });

            IAuthService authService = new AuthService(secretKey);
            var token = authService.GenerateToken(new AuthContainerModel()
            {
                Claims = claims.ToArray(),
                SecretKey = secretKey
            });
            return token;
        }

        static string SCDNEndpoint
        {
            get => "https://commonapi.insightincloud.com/api/External/GetConfigData?contentIdentifier=";
        }

        async Task<T> GetContent<T>(string contentIdentifier)
        {
            string token = GetToken(_secretKey, _payload);
            string SCDNEndpoint = $"{ JWTHelperUtility.SCDNEndpoint }{ contentIdentifier }";

            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var content = await client.GetStringAsync(SCDNEndpoint).ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<T>(content);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public JWTHelperUtility(string secretKey, IPayload payload)
        {
            _secretKey = secretKey;
            _payload = payload;
        }

        public FTPCredential GetFTPCred(string contentIdentifier)
        {
            return GetContent<FTPCredential>(contentIdentifier).Result;
        }

        public FTPCredential GetSFTPCred(string contentIdentifier)
        {
            return GetContent<FTPCredential>(contentIdentifier).Result;
        }

        public EmailList GetEmailRecients(string contentIdentifier)
        {
            return GetContent<EmailList>(contentIdentifier).Result;
        }

        public EmailConfig GetEmailConfig(string contentIdentifier)
        {
            return GetContent<EmailConfig>(contentIdentifier).Result;
        }
    }
}
