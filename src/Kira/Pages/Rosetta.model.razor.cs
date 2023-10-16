namespace Kira.Pages;

using Domain;

public partial class Rosetta
{
    public class Model(Issue myIssue, Issue customerIssue)
    {
        public string CustomerKey { get; } = customerIssue.Key;
        public string CustomerSummary { get; } = customerIssue.Fields.Summary;
        
        public string MyKey { get; } = myIssue.Key;
        
    }
}