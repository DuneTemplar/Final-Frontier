using Content.Shared.Containers.ItemSlots;
using Content.Shared.Interaction;
using Content.Shared.Nutrition.Components;
using Content.Shared.Nutrition.EntitySystems;
using Robust.Shared.Containers;
using System.Diagnostics.CodeAnalysis;
using Content.Shared.Clothing.EntitySystems;
using Content.Shared.Clothing.Components;
using Content.Shared.Mobs;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Shared._FinalFrontier.PowerArmor;

/// <summary>
/// Controls PowerArmorSlots visuals
/// </summary>
public abstract class PowerArmorSlotsSystem : EntitySystem
{
    [Dependency] private readonly ItemSlotsSystem _slots = default!;
	[Dependency] private readonly ClothingSystem _clothing = default!;
    [Dependency] private readonly OpenableSystem _openable = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private   readonly INetManager _netManager = default!;
    [Dependency] private   readonly ISharedPlayerManager _playerManager = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PowerArmorSlotsComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<PowerArmorSlotsComponent, EntInsertedIntoContainerMessage>(OnContainerModified);
        SubscribeLocalEvent<PowerArmorSlotsComponent, EntRemovedFromContainerMessage>(OnContainerModified);
    }

    private void OnStartup(Entity<PowerArmorSlotsComponent> ent, ref ComponentStartup args)
    {
        UpdateAppearance(ent);
    }

    public virtual void UpdateAppearance(Entity<PowerArmorSlotsComponent> ent)
    {
        _appearance.SetData(ent, PowerArmorSlotsVisuals.ContainsChestplate, HasItem(ent, ent.Comp.SlotChestplate));
		_appearance.SetData(ent, PowerArmorSlotsVisuals.ContainsRightArm, HasItem(ent, ent.Comp.SlotRightArm));
		_appearance.SetData(ent, PowerArmorSlotsVisuals.ContainsLeftArm, HasItem(ent, ent.Comp.SlotLeftArm));
		_appearance.SetData(ent, PowerArmorSlotsVisuals.ContainsRightLeg, HasItem(ent, ent.Comp.SlotRightLeg));
		_appearance.SetData(ent, PowerArmorSlotsVisuals.ContainsLeftLeg, HasItem(ent, ent.Comp.SlotLeftLeg));
		if (TryComp<ClothingComponent>(ent, out var clothingComp))
		{
			foreach (var layer in clothingComp.ClothingVisuals["outerClothing"])
			{
                if (layer.State == "test-chestplate")
                {
                    layer.Visible = HasItem(ent, ent.Comp.SlotChestplate);
                    var slotGot = TryGetSlot(ent, "Chestplate", out var slot);
                    if (slotGot && slot != null && slot.HasItem)
                    {
                        if (TryComp<PowerArmorPieceComponent>(slot.Item, out var pieceComp))
                        {
                            layer.RsiPath = pieceComp.Path;
                        }
                    }
                }
				else if (layer.State == "test-right-arm")
				{
					layer.Visible = HasItem(ent, ent.Comp.SlotRightArm);
					var slotGot = TryGetSlot(ent, "RightArm", out var slot);
                    if (slotGot && slot != null && slot.HasItem)
                    {
                        if (TryComp<PowerArmorPieceComponent>(slot.Item, out var pieceComp))
                        {
                            layer.RsiPath = pieceComp.Path;
                        }
                    }
				}
				else if (layer.State == "test-left-arm")
				{
					layer.Visible = HasItem(ent, ent.Comp.SlotLeftArm);
					var slotGot = TryGetSlot(ent, "LeftArm", out var slot);
                    if (slotGot && slot != null && slot.HasItem)
                    {
                        if (TryComp<PowerArmorPieceComponent>(slot.Item, out var pieceComp))
                        {
                            layer.RsiPath = pieceComp.Path;
                        }
                    }
				}
				else if (layer.State == "test-right-leg")
				{
					layer.Visible = HasItem(ent, ent.Comp.SlotRightLeg);
					var slotGot = TryGetSlot(ent, "RightLeg", out var slot);
                    if (slotGot && slot != null && slot.HasItem)
                    {
                        if (TryComp<PowerArmorPieceComponent>(slot.Item, out var pieceComp))
                        {
                            layer.RsiPath = pieceComp.Path;
                        }
                    }
				}
				else if (layer.State == "test-left-leg")
				{
					layer.Visible = HasItem(ent, ent.Comp.SlotLeftLeg);
					var slotGot = TryGetSlot(ent, "LeftLeg", out var slot);
                    if (slotGot && slot != null && slot.HasItem)
                    {
                        if (TryComp<PowerArmorPieceComponent>(slot.Item, out var pieceComp))
                        {
                            layer.RsiPath = pieceComp.Path;
                        }
                    }
				}
                if (_netManager.IsClient && _playerManager.LocalEntity != null)
                {
                    if (TryComp<AppearanceComponent>(_playerManager.LocalEntity.Value, out var appearanceComp))
                        Dirty(_playerManager.LocalEntity.Value, appearanceComp);
                }
            }

		}
    }

    private void OnContainerModified(EntityUid uid, PowerArmorSlotsComponent component, ContainerModifiedMessage args)
    {

        if (args.Container.ID == component.SlotChestplate)
            UpdateAppearance((uid, component));
		if (args.Container.ID == component.SlotRightArm)
            UpdateAppearance((uid, component));
		if (args.Container.ID == component.SlotLeftArm)
            UpdateAppearance((uid, component));
		if (args.Container.ID == component.SlotRightLeg)
            UpdateAppearance((uid, component));
		if (args.Container.ID == component.SlotLeftLeg)
            UpdateAppearance((uid, component));
    }

    /// <summary>
    /// Tries to get the cabinet's item slot.
    /// </summary>
    public bool TryGetSlot(Entity<PowerArmorSlotsComponent> ent, string limb, [NotNullWhen(true)] out ItemSlot? slot)
    {
        slot = null;
        if (!TryComp<ItemSlotsComponent>(ent, out var slots))
            return false;
        if (limb == "Chestplate")
		{
			return _slots.TryGetSlot(ent, ent.Comp.SlotChestplate, out slot, slots);
		}
		else if (limb == "RightArm")
		{
			return _slots.TryGetSlot(ent, ent.Comp.SlotRightArm, out slot, slots);
		}
		else if (limb == "LeftArm")
		{
			return _slots.TryGetSlot(ent, ent.Comp.SlotLeftArm, out slot, slots);
		}
		else if (limb == "RightLeg")
		{
			return _slots.TryGetSlot(ent, ent.Comp.SlotRightLeg, out slot, slots);
		}
		else if (limb == "LeftLeg")
		{
			return _slots.TryGetSlot(ent, ent.Comp.SlotLeftLeg, out slot, slots);
		}
		return false;
    }

    /// <summary>
    /// Returns true if the cabinet contains an item.
    /// </summary>
    public bool HasItem(Entity<PowerArmorSlotsComponent> ent, string limb)
    {
        return TryGetSlot(ent, limb, out var slot) && slot.HasItem;
    }
}
