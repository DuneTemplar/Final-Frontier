using Robust.Shared.GameStates;

namespace Content.Shared._FinalFrontier.Weapons.Ranged.Components;

/// <summary>
/// Indicates that this gun requires certain equipment to be useable.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class GunRequiresClothingComponent : Component
{
    [DataField]
    public string RequiredClothingName = " ";
}
