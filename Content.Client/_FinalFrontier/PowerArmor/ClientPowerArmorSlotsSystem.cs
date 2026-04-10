using Content.Shared.Containers.ItemSlots;
using Content.Shared.Interaction;
using Content.Shared.Nutrition.Components;
using Content.Shared.Nutrition.EntitySystems;
using Robust.Shared.Containers;
using System.Diagnostics.CodeAnalysis;
using Content.Client.Sprite;
using Content.Shared.Clothing.EntitySystems;
using Content.Shared.Clothing.Components;
using Content.Shared.Mobs;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Client.GameObjects;
using Content.Shared._FinalFrontier.PowerArmor;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Shared.ContentPack;
using Robust.Shared.Utility;

namespace Content.Client._FinalFrontier.PowerArmor;

/// <summary>
/// Controls PowerArmorSlots visuals on the client.
/// </summary>
public sealed partial class ClientPowerArmorSlotsSystem : PowerArmorSlotsSystem
{
	[Dependency] private   readonly INetManager _netManager = default!;
    [Dependency] private   readonly ISharedPlayerManager _playerManager = default!;

	public override void UpdateAppearance(Entity<PowerArmorSlotsComponent> ent)
	{
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
		
		
		if (TryComp<SpriteComponent>(ent, out var spriteComp))
		{
			foreach (var layer in spriteComp.AllLayers)
			{
                if (layer.RsiState == "test-chestplate")
                {
                    layer.Visible = HasItem(ent, ent.Comp.SlotChestplate);
                    var slotGot = TryGetSlot(ent, "Chestplate", out var slot);
                    if (slotGot && slot != null && slot.HasItem)
                    {
                        if (TryComp<SpriteComponent>(slot.Item, out var itemSpriteComp))
						{
							layer.Rsi = itemSpriteComp.BaseRSI; // probably inefficient
						}
                    }
                }
				else if (layer.RsiState == "test-right-arm")
				{
					layer.Visible = HasItem(ent, ent.Comp.SlotRightArm);
					var slotGot = TryGetSlot(ent, "RightArm", out var slot);
                    if (slotGot && slot != null && slot.HasItem)
                    {
                        if (TryComp<SpriteComponent>(slot.Item, out var itemSpriteComp))
                            {
                                layer.Rsi = itemSpriteComp.BaseRSI;
                            }
                    }
				}
				else if (layer.RsiState == "test-left-arm")
				{
					layer.Visible = HasItem(ent, ent.Comp.SlotLeftArm);
					var slotGot = TryGetSlot(ent, "LeftArm", out var slot);
                    if (slotGot && slot != null && slot.HasItem)
                    {
                        if (TryComp<SpriteComponent>(slot.Item, out var itemSpriteComp))
						{
							layer.Rsi = itemSpriteComp.BaseRSI;
						}
                    }
				}
				else if (layer.RsiState == "test-right-leg")
				{
					layer.Visible = HasItem(ent, ent.Comp.SlotRightLeg);
					var slotGot = TryGetSlot(ent, "RightLeg", out var slot);
                    if (slotGot && slot != null && slot.HasItem)
                    {
                        if (TryComp<SpriteComponent>(slot.Item, out var itemSpriteComp))
						{
							layer.Rsi = itemSpriteComp.BaseRSI;
						}
                    }
				}
				else if (layer.RsiState == "test-left-leg")
				{
					layer.Visible = HasItem(ent, ent.Comp.SlotLeftLeg);
					var slotGot = TryGetSlot(ent, "LeftLeg", out var slot);
                    if (slotGot && slot != null && slot.HasItem)
                    {
                        if (TryComp<SpriteComponent>(slot.Item, out var itemSpriteComp))
						{
							layer.Rsi = itemSpriteComp.BaseRSI;
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
}
