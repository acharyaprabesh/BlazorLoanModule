using BlazorLoanModule.Components;
using BlazorLoanModule.Pages.Auth.Manager.Implementation;
using BlazorLoanModule.Pages.Auth.Manager.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddTransient<IAuthManager, AuthManager>();
builder.Services.AddHttpClient("ApiEndpoint", client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetSection("ApiUrl:BaseApi").Value);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
