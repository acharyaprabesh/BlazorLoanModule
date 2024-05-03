using BlazorLoanModule.Pages.Auth.Model;
using BlazorLoanModule.Utility.CacheManager;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;
using System.Text.Json;

namespace BlazorLoanModule.Configuration
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
        private readonly ProtectedSessionStorage _sessionStorage;
        //private readonly ProtectedLocalStorage _LocalStorage;
        private readonly ICacheService _cacheService;
        public CustomAuthenticationStateProvider(ProtectedSessionStorage protectedSessionStorage, ICacheService cacheService)
        {
            _sessionStorage = protectedSessionStorage;
            _cacheService = cacheService;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var TokenStorageResult = await _sessionStorage.GetAsync<string>("LoanModuleToken");
                var userSession = TokenStorageResult.Success ? TokenStorageResult.Value : null;
                if (userSession == null)
                {
                    return await Task.FromResult(new AuthenticationState(_anonymous));
                }

                var claims = GetClaimsFromJwt(TokenStorageResult.ToString());

                var ClaimPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new(ClaimTypes.Email, "abc@uranutech.com"),
                }.Union(claims)
                , "CustomAuth"));
                return await Task.FromResult(new AuthenticationState(ClaimPrincipal));
            }
            catch
            {
                return await Task.FromResult(new AuthenticationState(_anonymous));
            }
        }

        public  async Task UpadteAuthenticationState(string Token)
        {
            ClaimsPrincipal claimsPrincipal = new();
            if (Token != null)
            {
                
                var claims = GetClaimsFromJwt(Token);
                //string? SecurityStamp = claims.Where(c => c.Type == "SecurityStamp").Select(c => c.Value).SingleOrDefault();
                await _sessionStorage.SetAsync("LoanModuleToken", Token);
                await _cacheService.SetData("LoginToken", Token);
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new(ClaimTypes.Email, "abc@uranutech.com")
                }.Union(claims)
                ));
            }
            else
            {
                claimsPrincipal = _anonymous;
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
        internal static IEnumerable<Claim> GetClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            if (keyValuePairs != null)
            {
                keyValuePairs.TryGetValue(ClaimTypes.Role, out var roles);

                if (roles != null)
                {
                    if (roles.ToString()!.Trim().StartsWith("["))
                    {
                        var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString()!);

                        claims.AddRange(parsedRoles!.Select(role => new Claim(ClaimTypes.Role, role)));
                    }
                    else
                    {
                        claims.Add(new Claim(ClaimTypes.Role, roles.ToString()!));
                    }

                    keyValuePairs.Remove(ClaimTypes.Role);
                }

                keyValuePairs.TryGetValue("Permission", out var permissions);
                if (permissions != null)
                {
                    if (permissions.ToString()!.Trim().StartsWith("["))
                    {
                        var parsedPermissions = JsonSerializer.Deserialize<string[]>(permissions.ToString()!);
                        claims.AddRange(parsedPermissions!.Select(permission => new Claim("Permission", permission)));
                    }
                    else
                    {
                        claims.Add(new Claim("Permission", permissions.ToString()!));
                    }
                    keyValuePairs.Remove("Permission");
                }

                claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!)));
            }
            return claims;
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            return Convert.FromBase64String(base64);
        }
    }
}
