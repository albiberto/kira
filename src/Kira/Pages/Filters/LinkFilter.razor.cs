namespace Kira.Pages.Filters;

using Builders;
using Infrastructure.Options;
using Infrastructure.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Workloads;

public partial class LinkFilter
{
    [Inject] IOptions<CompaniesOptions> CompaniesOptions { get; set; } = null!;
    
    [Inject] public IServiceProvider Provider { get; set; } = null!;
    [Inject] public IOptionsSnapshot<CompanyOptions> Options { get; set; } = null!;
    
    [Parameter] public JqlModel Query { get; set; } = new();
    [Parameter] public EventCallback<JqlModel> QueryChanged { get; set; }
    
    [Parameter] public string LeftKey { get; set; } = string.Empty;
    [Parameter] public EventCallback<string> LeftKeyChanged { get; set; }
    
    [Parameter] public string RightKey { get; set; } = string.Empty;
    [Parameter] public EventCallback<string> RightKeyChanged { get; set; }

    protected override async Task OnInitializedAsync()
    {
        LeftOptions = CompaniesOptions.Value.Companies
            .Where(company => !company.Value.Master ?? true)
            .Select((company, index) => new Option(company.Key, company.Value.Name, index != 0));
        
        RightOptions = CompaniesOptions.Value.Companies
            .Where(company => company.Value.Master ?? true)
            .Select((company, index) => new Option(company.Key, company.Value.Name, index != 0));
        
        SelectedLeftOption = LeftOptions.FirstOrDefault(option => !option.Disabled)?.Key;
        SelectedRightOption = RightOptions.FirstOrDefault(option => !option.Disabled)?.Key;

        await LeftKeyChanged.InvokeAsync(SelectedLeftOption);
        await RightKeyChanged.InvokeAsync(SelectedRightOption);
    }
}