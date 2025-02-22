using CustomerManagerWeb.Controllers;
using CustomerManagerWeb.Models;
using CustomerManagerWeb.Services.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using System.Reflection;

namespace CustomerManagerWeb.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _BaseUrl;
        public AuthService(IOptions<ServiceSettings> BaseUrl, ILogger<HomeController> logger)
        {
            _logger = logger;
            _BaseUrl = $"{BaseUrl.Value.BaseUrl}/api";
        }

        public async Task<string> LoginAsync(LoginViewModel request)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var content = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");
                    var response = await client.PostAsync($"{_BaseUrl}/Auth/Login", content);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

                        return tokenResponse.Token;
                    }
                    else
                        return string.Empty ;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Login ex {ex.Message}");
                return string.Empty;
            }
        }
    }
}
