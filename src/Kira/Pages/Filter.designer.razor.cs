namespace Kira.Pages;

public partial class Filter
{
    FormModel formModel = new();
    bool isLoading = true;
    bool popup;

    bool LeftBound() => (formModel.SelectedProjects?.Count ?? 0) > 0;
    bool RightBound() => (formModel.SelectedProjects?.Count ?? 0) <= 3;
}