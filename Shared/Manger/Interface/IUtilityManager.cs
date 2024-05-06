using BlazorLoanModule.Shared.Model;

namespace BlazorLoanModule.Shared.Manger.Interface
{
    public interface IUtilityManager
    {
        Task<List<DropDownModel>> GetDropDownListAsync(string DropDownName, string? FilterValue);
    }
}
