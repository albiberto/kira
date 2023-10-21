namespace Kira.Pages;

using Domain;
using Extensions;
using Radzen;

public partial class Rosetta
{
    public class LeftModel(Issue leftIssue, IEnumerable<Issue> rightIssue)
    {
        public string Key { get; } = leftIssue.Key;
        public string Summary { get; } = leftIssue.Fields.Summary;
        public string Assignee { get; } = leftIssue.Fields.Assignee?.Name ?? "Unassigned";
        public string Reporter { get; } = leftIssue.Fields.Reporter?.Name ?? "Unassigned";
        public string Status { get; } = leftIssue.Fields.Status.Name;
        public BadgeStyle StatusColor { get; } = leftIssue.Fields.Status.StatusCategory.Id.ToBadgeStyle();

        public IEnumerable<RightModel> Issues { get; } = rightIssue.Select(issue => new RightModel( leftIssue, issue));
    }

    public class RightModel(Issue leftIssue, Issue rightIssue)
    {
        public string Key { get; } = rightIssue.Key;
        public string ParentKey { get; } = rightIssue.Fields.Parent?.Key ?? string.Empty;

        public bool IsSubType { get; } = rightIssue.Fields.Type.Subtask;
        
        public string Summary { get; set; } = rightIssue.Fields.Summary
            .Replace(leftIssue.Fields.Summary, string.Empty)
            .Replace(leftIssue.Key, string.Empty)
            .Replace("|", string.Empty)
            .Trim();
        
        public string StartDate { get; } = rightIssue.Fields.StartDate ?? string.Empty;
        public string DueDate { get; } = rightIssue.Fields.DueDate ?? string.Empty;

        public int? TimeOriginalEstimate { get; } = rightIssue.Fields.TimeOriginalEstimate;
        public int? TimeSpent { get; } = rightIssue.Fields.TimeSpent;
        public int? RemainingEstimate { get; } = rightIssue.Fields.RemainingEstimate;
        
        public string Assignee { get; } = rightIssue.Fields.Assignee?.DisplayName ?? "Unassigned";
        public string? AssigneeAvatar { get; } = rightIssue.Fields.Assignee?.AvatarUrls._16x16;

        public string Reporter { get; } = rightIssue.Fields.Reporter?.DisplayName ?? "Unassigned";
        public string? ReporterAvatar { get; } = rightIssue.Fields.Reporter?.AvatarUrls._16x16;

        public string Status { get; } = rightIssue.Fields.Status.Name;
        public BadgeStyle StatusColor { get; } = rightIssue.Fields.Status.StatusCategory.Id.ToBadgeStyle();
    }
}