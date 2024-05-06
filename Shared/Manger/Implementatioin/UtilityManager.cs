using BlazorLoanModule.Pages.Auth.Manager.Route;
using BlazorLoanModule.Shared.Manger.Interface;
using BlazorLoanModule.Shared.Model;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using BlazorLoanModule.Utility.CacheManager;
using BlazorLoanModule.Shared.Manger.Route;

namespace BlazorLoanModule.Shared.Manger.Implementatioin
{
    public class UtilityManager : IUtilityManager
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        private readonly ICacheService _cacheService;
        public UtilityManager(IHttpClientFactory httpClientFactory, ICacheService cacheService)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("ApiEndpoint");
            _cacheService = cacheService;
        }
        public async Task<List<DropDownModel>> GetDropDownListAsync(string DropDownName, string? FilterValue)
        {
            var token = await _cacheService.GetData<string>("LoginToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.ToString());
            var respose = await _httpClient.GetAsync( UtilityApiEndPoint.DropDownApiEndPoint+ "?DropDownName="+DropDownName+ "&FilterValue=" + FilterValue);
            var responseString = await respose.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<List<DropDownModel>>(responseString,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            return responseObject;
        }
    }
}
