using Content.Shared.Containers.ItemSlots;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared._FinalFrontier.PowerArmor;

/// <summary>
/// Used for modular power armor.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(PowerArmorSlotsSystem))]
public sealed partial class PowerArmorSlotsComponent : Component
{
    /// <summary>
    /// Chestplate slot name
    /// </summary>
    [DataField]
    public string SlotChestplate = "Chestplate";
	
	/// <summary>
    /// Right arm slot name
    /// </summary>
    [DataField]
    public string SlotRightArm = "RightArm";
	
	/// <summary>
    /// Left arm slot name
    /// </summary>
    [DataField]
    public string SlotLeftArm = "LeftArm";
	
	/// <summary>
    /// Right leg slot name
    /// </summary>
    [DataField]
    public string SlotRightLeg = "RightLeg";
	
	/// <summary>
    /// Left leg slot name
    /// </summary>
    [DataField]
    public string SlotLeftLeg = "LeftLeg";
}

[RegisterComponent, NetworkedComponent, Access(typeof(PowerArmorSlotsSystem))]
public sealed partial class PowerArmorPieceComponent : Component
{
	/// <summary>
    /// RSI Path
    /// </summary>
    [DataField]
    public string Path = " ";
}

[Serializable, NetSerializable]
public enum PowerArmorSlotsVisuals : byte
{
    ContainsChestplate,
	ContainsRightArm,
	ContainsLeftArm,
	ContainsRightLeg,
	ContainsLeftLeg,
    LayerChestplate,
	LayerRightArm,
	LayerLeftArm,
	LayerRightLeg,
	LayerLeftLeg
}
