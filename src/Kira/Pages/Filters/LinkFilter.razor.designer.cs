namespace Kira.Pages.Filters;

using Radzen.Blazor;

public partial class LinkFilter
{
    RadzenDropDown<string> left;
    RadzenDropDown<string> right;

    IEnumerable<LinkFilter.Option> LeftOptions { get; set; }
    IEnumerable<LinkFilter.Option> RightOptions { get; set; }
    
    string SelectedLeftOption { get; set; }
    string SelectedRightOption { get; set; }
    
    public Task SelectedLeftOptionChanged(object value)
    {
        var key = (string)value;
        
        LeftKey = key;
        return LeftKeyChanged.InvokeAsync(key);
    }
    
    public Task SelectedRightOptionChanged(object value)
    {
        var key = (string)value;

        RightKey = key;
        return RightKeyChanged.InvokeAsync(key);
    }
}