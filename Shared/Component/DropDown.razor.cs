using BlazorLoanModule.Shared.Manger.Interface;
using BlazorLoanModule.Shared.Model;
using Microsoft.AspNetCore.Components;

namespace BlazorLoanModule.Shared.Component
{
    public partial class DropDown
    {
        [Parameter]
        public string DropDownName { get; set; }

        [Parameter]
        public string Filter { get; set; }=string.Empty;

        [Parameter]
        public string Value { get; set; } = string.Empty;

        [Parameter]
        public int All { get; set; }

        [Inject]
        private IUtilityManager _utilityManager  { get; set; }=default!;

        public List<DropDownModel> DropDownModels { get; set; } = new();

        [Parameter]
        public EventCallback<string> ValueChanged { get; set; }

        private string _value=string.Empty;

        public string BindValue
        {
            get => _value;
            set
            {
                if (_value == value) return;
                _value = value;
                ValueChanged.InvokeAsync(value);   
            }
        }
        protected override async Task OnInitializedAsync()
        {
            await LoanDrodown();
        }

        protected override async Task OnParametersSetAsync()
        {
            await LoanDrodown();
            if (!string.IsNullOrEmpty(Value))
            {
                BindValue = Value;
            }
        }
        private async Task LoanDrodown()
        {
            DropDownModels=await _utilityManager.GetDropDownListAsync(DropDownName, Filter);
        }
    }
}