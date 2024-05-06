using BlazorLoanModule.Pages.Auth.Manager.Interface;
using BlazorLoanModule.Pages.Auth.Manager.Route;
using BlazorLoanModule.Pages.Auth.Model;
using BlazorLoanModule.Utility.CacheManager;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BlazorLoanModule.Pages.Auth.Manager.Implementation
{
    public class BranchManager : IBranchManager
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        private readonly ICacheService _cacheService;
        public BranchManager(IHttpClientFactory httpClientFactory, ICacheService cacheService)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("ApiEndpoint");
            _cacheService = cacheService;
        }

        public async Task<List<BranchModel>> GetBranchesAsync()
        {
            var token = await _cacheService.GetData<string>("LoginToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.ToString());
            var respose = await _httpClient.GetAsync(BranchEndPoint.BranchList);
            var responseString = await respose.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<List<BranchModel>>(responseString,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            return responseObject;
        }
    }
}
