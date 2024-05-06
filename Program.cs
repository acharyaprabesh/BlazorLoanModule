using BlazorLoanModule.Components;
using BlazorLoanModule.Configuration;
using BlazorLoanModule.Pages.Auth.Manager.Implementation;
using BlazorLoanModule.Pages.Auth.Manager.Interface;
using BlazorLoanModule.Shared.Manger.Implementatioin;
using BlazorLoanModule.Shared.Manger.Interface;
using BlazorLoanModule.Utility.CacheManager;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddStackExchangeRedisCache(r => { r.Configuration = builder.Configuration["redis:connectionString"]; });
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddTransient<IAuthManager, AuthManager>();
builder.Services.AddTransient<IBranchManager, BranchManager>();
builder.Services.AddTransient<IUtilityManager, UtilityManager>();
builder.Services.AddTransient<ProtectedSessionStorage>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

builder.Services.AddHttpContextAccessor();
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
//    options =>
//    {
//        options.Cookie.Name = "Auth_Token";
//        options.LoginPath = "/Login";
//        options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
//    });
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpClient("ApiEndpoint", client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetSection("ApiUrl:BaseApi").Value);
});
builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("Permission", policy => policy.RequireClaim("Permission", "ABC.add"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
