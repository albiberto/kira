namespace Kira.Pages;

using Domain;
using Extensions;
using Radzen;

public partial class Workloads
{
    public class Model(Issue issue)
    {
        public string Key { get; } = issue.Key;
        public string Summary { get; } = issue.Fields.Summary;

        public string Status { get; } = issue.Fields.Status.Name;
        public BadgeStyle StatusColor { get; } = issue.Fields.Status.StatusCategory.Id.ToBadgeStyle();

        public string StartDate { get; } = issue.Fields.StartDate ?? string.Empty;
        public string DueDate { get; } = issue.Fields.DueDate ?? string.Empty;

        public int? TimeOriginalEstimate { get; } = issue.Fields.TimeOriginalEstimate;
        public int? TimeSpent { get; } = issue.Fields.TimeSpent;
        public int? RemainingEstimate { get; } = issue.Fields.RemainingEstimate;
        public double Progress { get; } = issue.Fields.Progress.Percent.ToDouble();

        public string Type { get; } = new(issue.Fields.Type.Name);
        public string TypeIcon { get; } = new(issue.Fields.Type.IconUrl);

        public string Priority { get; } = new(issue.Fields.Priority.Name);
        public string PriorityIcon { get; } = new(issue.Fields.Priority.IconUrl);

        public string Assignee { get; } = issue.Fields.Assignee?.DisplayName ?? "Unassigned";
        public string? AssigneeAvatar { get; } = issue.Fields.Assignee?.AvatarUrls._16x16;

        public string Reporter { get; } = issue.Fields.Reporter?.DisplayName ?? "Unassigned";
        public string? ReporterAvatar { get; } = issue.Fields.Reporter?.AvatarUrls._16x16;

        public string? ParentKey { get; } = issue.Fields.Parent?.Key;

        public string? ParentType { get; } = issue.Fields.Parent?.Fields.Type.Name;
        public string? ParentTypeIcon { get; } = issue.Fields.Parent?.Fields.Type.IconUrl;

        public string? ParentPriority { get; } = issue.Fields.Parent?.Fields.Priority.Name;
        public string? ParentPriorityIcon { get; } = issue.Fields.Parent?.Fields.Priority.IconUrl;
    }
}