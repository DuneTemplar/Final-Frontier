using Content.Shared.Audio; // Final Frontier
using Content.Shared.Examine;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Events;
using Robust.Shared.GameStates;
using Robust.Shared.Map;
using Robust.Shared.Serialization;
using Content.Shared._FinalFrontier.Weapons.Hitscan.Components;
using Content.Shared.Interaction.Events;
using Robust.Shared.Audio; // Final Frontier

namespace Content.Shared.Weapons.Ranged.Systems;

public abstract partial class SharedGunSystem
{
    protected virtual void InitializeBattery()
    {
        // Trying to dump comp references hence the below
        // Hitscan
        SubscribeLocalEvent<HitscanBatteryAmmoProviderComponent, ComponentGetState>(OnBatteryGetState);
        SubscribeLocalEvent<HitscanBatteryAmmoProviderComponent, ComponentHandleState>(OnBatteryHandleState);
        SubscribeLocalEvent<HitscanBatteryAmmoProviderComponent, TakeAmmoEvent>(OnBatteryTakeAmmo);
        SubscribeLocalEvent<HitscanBatteryAmmoProviderComponent, CheckShootPrototypeEvent>(OnBatteryCheckProto); // Mono
        SubscribeLocalEvent<HitscanBatteryAmmoProviderComponent, GetAmmoCountEvent>(OnBatteryAmmoCount);
        SubscribeLocalEvent<HitscanBatteryAmmoProviderComponent, ExaminedEvent>(OnBatteryExamine);
        SubscribeLocalEvent<HitscanManualAmmoProviderComponent, UseInHandEvent>(OnManualCharge); // Final Frontier
        SubscribeLocalEvent<HitscanManualAmmoProviderComponent, CheckShootPrototypeEvent>(OnBatteryCheckProto); // Final Frontier
        SubscribeLocalEvent<HitscanManualAmmoProviderComponent, TakeAmmoEvent>(OnBatteryTakeAmmo); // Final Frontier

        // Projectile
        SubscribeLocalEvent<ProjectileBatteryAmmoProviderComponent, ComponentGetState>(OnBatteryGetState);
        SubscribeLocalEvent<ProjectileBatteryAmmoProviderComponent, ComponentHandleState>(OnBatteryHandleState);
        SubscribeLocalEvent<ProjectileBatteryAmmoProviderComponent, TakeAmmoEvent>(OnBatteryTakeAmmo);
        SubscribeLocalEvent<ProjectileBatteryAmmoProviderComponent, CheckShootPrototypeEvent>(OnBatteryCheckProto); // Mono
        SubscribeLocalEvent<ProjectileBatteryAmmoProviderComponent, GetAmmoCountEvent>(OnBatteryAmmoCount);
        SubscribeLocalEvent<ProjectileBatteryAmmoProviderComponent, ExaminedEvent>(OnBatteryExamine);
    }

    private void OnManualCharge(EntityUid uid, HitscanManualAmmoProviderComponent component, UseInHandEvent args)
    {
        if (args.Handled)
            return;
        if (component.Charges < component.MaxCharges)
        {
            if (TryComp<GunComponent>(uid, out var gun))
            {
                if ((Timing.CurTime - component.LastCharge).TotalSeconds < 1.25)
                    return;
                component.Charges++;
                component.LastCharge = Timing.CurTime;
                Audio.PlayPvs(component.InteractSound, uid, AudioParams.Default.WithVariation(SharedContentAudioSystem.DefaultVariation).WithVolume(-1f));
            }
        }
        args.Handled = true;
    }

    private void OnBatteryHandleState(EntityUid uid, BatteryAmmoProviderComponent component, ref ComponentHandleState args)
    {
        if (args.Current is not BatteryAmmoProviderComponentState state)
            return;

        component.Shots = state.Shots;
        component.Capacity = state.MaxShots;
        component.FireCost = state.FireCost;

        if (component is HitscanBatteryAmmoProviderComponent hitscan && state.Prototype != null) // Shitmed Change
            hitscan.HitscanEntityProto = state.Prototype; // Mono - Changed to HitscanEntityProto
    }

    private void OnBatteryGetState(EntityUid uid, BatteryAmmoProviderComponent component, ref ComponentGetState args)
    {
        var state = new BatteryAmmoProviderComponentState() // Shitmed Change
        {
            Shots = component.Shots,
            MaxShots = component.Capacity,
            FireCost = component.FireCost,
        };

        if (TryComp<HitscanBatteryAmmoProviderComponent>(uid, out var hitscan)) // Shitmed Change
            state.Prototype = hitscan.HitscanEntityProto; // Mono - Changed to HitscanEntityProto

        args.State = state; // Shitmed Change
    }

    private void OnBatteryExamine(EntityUid uid, BatteryAmmoProviderComponent component, ExaminedEvent args)
    {
        args.PushMarkup(Loc.GetString("gun-battery-examine", ("color", AmmoExamineColor), ("count", component.Shots)));
    }

    private void OnBatteryTakeAmmo(EntityUid uid, BatteryAmmoProviderComponent component, TakeAmmoEvent args)
    {
        var shots = Math.Min(args.Shots, component.Shots);

        // Don't dirty if it's an empty fire.
        if (shots == 0)
            return;

        for (var i = 0; i < shots; i++)
        {
            args.Ammo.Add(GetShootable(component, args.Coordinates));
            component.Shots--;
        }
        TakeCharge(uid, component);
        UpdateBatteryAppearance(uid, component);
        Dirty(uid, component);
    }

    // Mono
    private void OnBatteryCheckProto(EntityUid uid, BatteryAmmoProviderComponent comp, ref CheckShootPrototypeEvent args)
    {
        switch (comp)
        {
            case ProjectileBatteryAmmoProviderComponent proj:
                ProtoManager.TryIndex(proj.Prototype, out var proto);
                args.ShootPrototype = proto;
                break;
            case HitscanBatteryAmmoProviderComponent hitscan:
                ProtoManager.TryIndex(hitscan.HitscanEntityProto, out var hitProto);
                args.ShootPrototype = hitProto;
                break;
            case HitscanManualAmmoProviderComponent manual:
                ProtoManager.TryIndex(manual.HitscanEntityProto, out var manualProto);
                args.ShootPrototype = manualProto;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnBatteryAmmoCount(EntityUid uid, BatteryAmmoProviderComponent component, ref GetAmmoCountEvent args)
    {
        args.Count = component.Shots;
        args.Capacity = component.Capacity;
    }

    /// <summary>
    /// Update the battery (server-only) whenever fired.
    /// </summary>
    protected virtual void TakeCharge(EntityUid uid, BatteryAmmoProviderComponent component)
    {
        UpdateAmmoCount(uid, prediction: false);
        if (TryComp<HitscanManualAmmoProviderComponent>(uid, out var manual))
            manual.Charges = 1;
    }

    protected void UpdateBatteryAppearance(EntityUid uid, BatteryAmmoProviderComponent component)
    {
        if (!TryComp<AppearanceComponent>(uid, out var appearance))
            return;

        Appearance.SetData(uid, AmmoVisuals.HasAmmo, component.Shots != 0, appearance);
        Appearance.SetData(uid, AmmoVisuals.AmmoCount, component.Shots, appearance);
        Appearance.SetData(uid, AmmoVisuals.AmmoMax, component.Capacity, appearance);
    }

    private (EntityUid? Entity, IShootable) GetShootable(BatteryAmmoProviderComponent component, EntityCoordinates coordinates)
    {
        switch (component)
        {
            case ProjectileBatteryAmmoProviderComponent proj:
                var ent = Spawn(proj.Prototype, coordinates);
                return (ent, EnsureShootable(ent));
            case HitscanBatteryAmmoProviderComponent hitscan:
                var hitscanEnt = Spawn(hitscan.HitscanEntityProto);
                return (hitscanEnt, EnsureShootable(hitscanEnt));
            case HitscanManualAmmoProviderComponent manual:
                var manualEnt = Spawn(manual.HitscanEntityProto);
                return (manualEnt, EnsureShootable(manualEnt));
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    [Serializable, NetSerializable]
    private sealed class BatteryAmmoProviderComponentState : ComponentState
    {
        public int Shots;
        public int MaxShots;
        public float FireCost;
        public string? Prototype;
    }
}
