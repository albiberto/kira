namespace Kira.Pages;

using System.Collections.Immutable;
using Domain;
using Infrastructure.Options;

public partial class FilterForm
{
    FormModel formModel = new();
    bool isLoading = true;
    bool popup;
    
    bool LeftBound() => (formModel.SelectedProjects?.Count ?? 0) > 0;
    bool RightBound() => (formModel.SelectedProjects?.Count ?? 0) <= 3;
}