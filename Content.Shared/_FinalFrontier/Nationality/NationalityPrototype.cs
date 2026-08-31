using Robust.Shared.Prototypes;

namespace Content.Shared._FinalFrontier.Nationality;

/// <summary>
/// Prototype for a nation players can be aligned with.
/// </summary>
[Prototype("nationality")]
public sealed class NationalityPrototype : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField("name", required: true)]
    public string Name { get; private set; } = default!;

    [DataField("description", required: false)]
    public string Description { get; private set; } = string.Empty;

    [DataField("color")]
    public Color Color { get; private set; } = Color.White;

    [DataField("image")]
    public string? Image { get; private set; }
}
