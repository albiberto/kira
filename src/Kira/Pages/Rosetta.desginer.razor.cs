namespace Kira.Pages;

using Infrastructure.Clients;
using Infrastructure.Options;
using Radzen.Blazor;

public partial class Rosetta
{
    static readonly IEnumerable<Switch> Options = new List<Switch>
    {
        new(false, "From My to Customer"),
        new(true, "From Customer to My")
    };
    
    Switch SelectedOption { get; set; } = Options.First();
    bool selectedValue;
    
    RadzenDropDown<Switch> ddl = null!;

    public void DropDownChange(object value) => selectedValue = ((Switch)value).Value;
    public Task SwitchChange(bool value) => ddl.SelectItem(Options.FirstOrDefault(@switch => @switch.Value == value));

    public class Switch(bool value, string name)
    {
        public bool Value { get; } = value;
        public string Name { get; } = name;
    }
}