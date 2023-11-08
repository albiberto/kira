namespace Kira.Pages.Filters;

public partial class LinkFilter
{
    public class Option(string key, string value, bool disabled)
    {
        public string Key { get; } = key;
        public string Value { get; } = value;
        public bool Disabled { get; } = disabled;
    }
}