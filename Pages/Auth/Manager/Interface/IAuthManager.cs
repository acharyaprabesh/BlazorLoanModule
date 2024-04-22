using BlazorLoanModule.Model;
using BlazorLoanModule.Pages.Auth.Model;

namespace BlazorLoanModule.Pages.Auth.Manager.Interface
{
    public interface IAuthManager
    {
        Task<LoginTokenModel> LoginAsync(LoginModel loginModel);
    }
}
