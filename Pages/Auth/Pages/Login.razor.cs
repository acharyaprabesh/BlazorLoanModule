using BlazorLoanModule.Model;
using BlazorLoanModule.Pages.Auth.Manager.Interface;
using Microsoft.AspNetCore.Components;

namespace BlazorLoanModule.Pages.Auth.Pages
{
    public partial class Login
    {
        public LoginModel LoginModel { get; set; } = new();
        [Inject]
        private IAuthManager _authManager { get; set; }
        protected override async Task OnInitializedAsync()
        {
            
        }

        private async Task LoginClick()
        {
           var response=  await _authManager.LoginAsync(LoginModel);
            await _customAuthStateProvider.UpadteAuthenticationState(response.Message);
            _navigationManager.NavigateTo("/");
        }
    }
}