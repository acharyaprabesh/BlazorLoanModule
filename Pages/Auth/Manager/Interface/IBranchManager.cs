using BlazorLoanModule.Pages.Auth.Model;

namespace BlazorLoanModule.Pages.Auth.Manager.Interface
{
    public interface IBranchManager
    {
        Task<List<BranchModel>> GetBranchesAsync();
    }
}
