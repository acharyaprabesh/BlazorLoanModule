using BlazorLoanModule.Model;
using BlazorLoanModule.Pages.Auth.Manager.Interface;
using BlazorLoanModule.Pages.Auth.Manager.Route;
using BlazorLoanModule.Pages.Auth.Model;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BlazorLoanModule.Pages.Auth.Manager.Implementation
{
    public class AuthManager : IAuthManager
    {
        private IHttpClientFactory _httpClientFactory;
        HttpClient _httpClient = new();
        public AuthManager(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("ApiEndpoint");
        }
        public async Task<LoginTokenModel> LoginAsync(LoginModel loginModel)
        {
            //_httpClient.DefaultRequestHeaders.Authorization =new AuthenticationHeaderValue("Bearer", "");
            var respose = await _httpClient.PostAsJsonAsync(AuthEndPoint.Login, loginModel);
            var responseString=await respose.Content.ReadAsStringAsync();   
            var responseObject=JsonSerializer.Deserialize<LoginTokenModel>(responseString,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            return responseObject;
        }
    }
}
