using Robust.Shared.GameStates;

namespace Content.Shared._Goobstation.Weapons.SmartGun;

[RegisterComponent, NetworkedComponent]
public sealed partial class SmartGunComponent : Component
{
    [DataField]
    public bool RequiresWield;

    [DataField]
    public bool UseFactionIff = false;

    [DataField]
    public string FactionIff = " ";
}
