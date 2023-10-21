namespace Kira.Pages.Filters;

using Microsoft.AspNetCore.Components;
using Radzen;

public partial class Filter
{
    [Inject] NotificationService NotificationService { get; set; } = null!;

    FormModel formModel = new();
    bool isLoading = true;
    bool popup;

    bool LeftBound() => (formModel.SelectedProjects?.Count ?? 0) > 0;
    bool RightBound() => (formModel.SelectedProjects?.Count ?? 0) <= 3;
}