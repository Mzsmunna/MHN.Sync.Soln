using MHN.Sync.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MHN.Sync.MongoRepository.Services
{
    public class BrowserStackService
    {
        private string _userName { get; set; }
        private string _accessKey { get; set; }
        private const string _baseurl = "https://www.browserstack.com/screenshots/";

        public BrowserStackService(string userName, string accessKey)
        {
            _userName = userName;
            _accessKey = accessKey;
        }

        public async Task<T> GetAllAvailableBrowsers<T>() where T : class
        {
            using (var httpClient = GenerateHttpClient())
            {
                string content = "";
                var response = await httpClient.GetAsync("browsers").ConfigureAwait(false);
                content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<T>(content);
            }
        }

        public async Task<T> GenerateScreenShotJob<T>(RootBrowser rootBrowser) where T : class
        {
            if (rootBrowser == null)
                throw new ArgumentNullException("rootBrowser", "Method Argument cannot be null");
            using (var httpClient = GenerateHttpClient())
            {
                string Content;
                var response = await httpClient.PostAsync("", new StringContent(JsonConvert.SerializeObject(rootBrowser), Encoding.UTF8, "application/json")).ConfigureAwait(false);
                Content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<T>(Content);
            }
        }

        public async Task<T> GenerateScreenShot<T>(string jobId) where T : class
        {
            if (string.IsNullOrWhiteSpace(jobId))
                throw new ArgumentNullException("jobId", "Method Argument cannot be null");
            using (var httpClient = GenerateHttpClient())
            {
                string content;
                var response = await httpClient.GetAsync(jobId).ConfigureAwait(false);
                content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<T>(content);
            }
        }

        public async void SaveImage(string screenShotUrl, string saveImagePath)
        {
            if (string.IsNullOrWhiteSpace(screenShotUrl) || string.IsNullOrWhiteSpace(saveImagePath))
                throw new ArgumentNullException("jobId", "Method Argument cannot be null");
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Clear();
                var response = await httpClient.GetStreamAsync(screenShotUrl).ConfigureAwait(false);
                using (System.IO.FileStream output = new FileStream(@"F:\BrowserStackScreenShot\template-preview.jpg", FileMode.Create))
                {
                    response.CopyTo(output);
                }
            }
        }

        private HttpClient GenerateHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.BaseAddress = new Uri(_baseurl);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
                                                 | SecurityProtocolType.Tls
                                                 | SecurityProtocolType.Tls11
                                                 | SecurityProtocolType.Tls12;
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                                                 Convert.ToBase64String(Encoding.UTF8.GetBytes($"{this._userName}:{this._accessKey}")));
            return httpClient;
        }
    }
}
