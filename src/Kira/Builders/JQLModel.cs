namespace Kira.Builders;

using Extensions;
using Pages;
using Pages.Filters;

public record JqlModel(IEnumerable<string>? Projects = default,
    IEnumerable<(string ProjectId, string Id)>? IncludedComponents = default, IEnumerable<(string ProjectId, string Id)>? ExcludedComponents = default, bool? EmptyComponents = true,
    IEnumerable<(string ProjectId, string Id)>? IncludedTypes = default, IEnumerable<(string ProjectId, string Id)>? ExcludedTypes = default,
    IEnumerable<(string ProjectId, string Id)>? IncludedStatues = default, IEnumerable<(string ProjectId, string Id)>? ExcludedStatues = default,
    IEnumerable<(string Field, string Order)>? Order = default)
{
    public IEnumerable<string> Projects { get; init; } = Projects ?? Enumerable.Empty<string>();

    public IEnumerable<(string ProjectId, string Id)> IncludedComponents { get; init; } = IncludedComponents ?? Enumerable.Empty<(string ProjectId, string Id)>();
    public IEnumerable<(string ProjectId, string Id)> ExcludedComponents { get; init; } = ExcludedComponents ?? Enumerable.Empty<(string ProjectId, string Id)>();

    public IEnumerable<(string ProjectId, string Id)> IncludedTypes { get; init; } = IncludedTypes ?? Enumerable.Empty<(string ProjectId, string Id)>();
    public IEnumerable<(string ProjectId, string Id)> ExcludedTypes { get; init; } = ExcludedTypes ?? Enumerable.Empty<(string ProjectId, string Id)>();

    public IEnumerable<(string ProjectId, string Id)> IncludedStatues { get; init; } = IncludedStatues ?? Enumerable.Empty<(string ProjectId, string Id)>();
    public IEnumerable<(string ProjectId, string Id)> ExcludedStatues { get; init; } = ExcludedStatues ?? Enumerable.Empty<(string ProjectId, string Id)>();

    public IEnumerable<(string Field, string Order)> Order { get; init; } = Order ?? Enumerable.Empty<(string Field, string Order)>();

    public bool Empty => !Projects.Any();

    public static implicit operator JqlModel(Filter.FormModel formModel) =>
        new(
            formModel.SelectedProjects?.Select(project => project.Id),
            formModel.SelectedIncludedComponents.ToTuple(),
            formModel.SelectedExcludedComponents.ToTuple(),
            formModel.EmptyComponents,
            formModel.SelectedIncludedTypes.ToTuple(),
            formModel.SelectedExcludedTypes.ToTuple(),
            formModel.SelectedIncludedStatues.ToTuple(),
            formModel.SelectedExcludedStatues.ToTuple()
        );
}