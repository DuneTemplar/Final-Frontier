using Content.Shared.Weapons.Ranged.Components;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared._FinalFrontier.Weapons.Hitscan.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(fieldDeltas: true)]
public sealed partial class HitscanManualAmmoProviderComponent : BatteryAmmoProviderComponent
{
    [DataField("proto", required: true)]
    public EntProtoId HitscanEntityProto;

    /// <summary>
    /// How much the firing cost is multiplied by per charge
    /// </summary>
    [DataField, AutoNetworkedField]
    public float CostModifier = 1;

    /// <summary>
    /// How much the gun damage is multiplied by per charge
    /// </summary>
    [DataField, AutoNetworkedField]
    public float DamageModifier = 1;

    /// <summary>
    /// The sound that should be made when charging the gun
    /// </summary>
    [DataField, AutoNetworkedField]
    public SoundSpecifier InteractSound = new SoundPathSpecifier("/Audio/_Mono/Weapons/Guns/SmallArms/MagIn/chemrail_magin.ogg");

    /// <summary>
    /// How many times the gun has been charged
    /// </summary>
    [DataField, AutoNetworkedField]
    public int Charges = 1;

    /// <summary>
    /// How many times the gun can be charged
    /// </summary>
    [DataField, AutoNetworkedField]
    public int MaxCharges = 3;

    /// <summary>
    /// When the gun was last charged
    /// </summary>
    [DataField(customTypeSerializer:typeof(TimeOffsetSerializer))]
    [AutoNetworkedField]
    public TimeSpan LastCharge = TimeSpan.Zero;
}
