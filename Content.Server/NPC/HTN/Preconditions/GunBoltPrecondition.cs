using Content.Server.Weapons.Ranged.Systems;
using Content.Shared.Weapons.Ranged.Events;
using Content.Shared.Weapons.Ranged.Components;

namespace Content.Server.NPC.HTN.Preconditions;

/// <summary>
/// Gets if a gun is unbolted or not. Final Frontier.
/// </summary>
public sealed partial class GunUnboltPrecondition : HTNPrecondition
{
    [Dependency] private readonly IEntityManager _entManager = default!;

    public override bool IsMet(NPCBlackboard blackboard)
    {
        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);
        var gunSystem = _entManager.System<GunSystem>();

        if (!gunSystem.TryGetGun(owner, out var gunUid, out _))
        {
            return false;
        }
		if (!_entManager.TryGetComponent(gunUid, out ChamberMagazineAmmoProviderComponent? chamberComp))
		{
			return false;
		}
		if ((chamberComp.BoltClosed != null) && chamberComp.BoltClosed.Value)
		{
			return false;
		}



        return true;
    }
}
