namespace BlazorLoanModule.Pages.Auth.Model
{
    public class BranchModel
    {
        public int BranchId { get; set; }
        public string BranchCode { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? PhoneNo { get; set; }
        public bool status { get; set; }
        public string CreatedDate { get; set; }
    }
}
