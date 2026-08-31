using Robust.Shared.GameStates;

namespace Content.Shared._FinalFrontier.Nationality;

/// <summary>
/// Component for a player's nation alignment.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class NationalityComponent : Component
{
    /// <summary>
    /// The name of the nation the player belongs to.
    /// </summary>
    [DataField, AutoNetworkedField]
    public string NationName = string.Empty;
}
