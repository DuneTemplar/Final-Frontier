using Content.Shared._FinalFrontier.Nationality;
using Content.Shared.Access.Components;
using Content.Shared.Access.Systems;
using Content.Shared.GameTicking;
using Content.Shared.Inventory;
using Content.Shared.PDA;
using Content.Shared.Roles.Jobs;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Server._FinalFrontier.Nationality;

/// <summary>
/// This system handles nation alignment on players.
/// </summary>
public sealed class NationalitySystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly SharedJobSystem _jobSystem = default!;
	    [Dependency] private readonly InventorySystem _inventorySystem = default!;
    [Dependency] private readonly SharedIdCardSystem _idCardSystem = default!;

    // Dictionary to store original nation alignments for players
    private readonly Dictionary<string, string> _playerOriginalNations = new();

    private readonly HashSet<string> _tsfJobs = new()
    {
        "Sheriff",
        "Bailiff",
        "SeniorOfficer", // Sergeant
        "Deputy",
        "Brigmedic",
        "NFDetective",
        "PublicAffairsLiaison",
        "Cadet",
        "TsfEngineer",
        "TsfBorg",
    };

    private readonly HashSet<string> _rogues = new()
    {
        "PirateCaptain",
        "PirateFirstMate",
        "Pirate",
        "PDVInfiltrator",
        "PdvBorg",
    };

    public override void Initialize()
    {
        base.Initialize();

        // Subscribe to player spawn event to add the nation component
        SubscribeLocalEvent<PlayerSpawnCompleteEvent>(OnPlayerSpawnComplete);

        // Subscribe to player detached event to clean up stored preferences
        SubscribeLocalEvent<PlayerDetachedEvent>(OnPlayerDetached);
    }

    private void OnPlayerDetached(PlayerDetachedEvent args)
    {
        // Clean up stored preferences when player disconnects
        _playerOriginalNations.Remove(args.Player.UserId.ToString());
    }

    private void OnPlayerSpawnComplete(PlayerSpawnCompleteEvent args)
    {
        // Add the nation component with the player's nation alignment
        var nationComp = EnsureComp<Shared._FinalFrontier.Nationality.NationalityComponent>(args.Mob);

        var playerId = args.Player.UserId.ToString();
        var profileNation = args.Profile.Nation;

        // Store the player's original nation alignment if not already stored
        if (!_playerOriginalNations.ContainsKey(playerId))
        {
            _playerOriginalNations[playerId] = profileNation;
        }

        // todo - make this a switch statement or something lol. who cares.
        // Check if player's job is one of the TSF jobs
        if (args.JobId != null && _tsfJobs.Contains(args.JobId))
        {
            // Assign TSF nation
            nationComp.NationName = "TSF";
        }
        // Check if player's job is one of the Rogue jobs
        else if (args.JobId != null && _rogues.Contains(args.JobId))
        {
            // Assign Rogue nation
            nationComp.NationName = "PDV";
        }
        else
        {
            // Use "Neutral" as fallback for no alignment
            if (string.IsNullOrEmpty(profileNation))
                profileNation = "Neutral";

            // Restore the player's original nation alignment
            nationComp.NationName = profileNation;
        }

        // Ensure the component is networked to clients
        Dirty(args.Mob, nationComp);

        // Update the player's ID card with the nation alignment
        UpdateIdCardNation(args.Mob, nationComp.NationName);
    }

    /// <summary>
    /// Updates the player's ID card with their nation alignment
    /// </summary>
    private void UpdateIdCardNation(EntityUid playerEntity, string nationName)
    {
        // Try to get the player's ID card
        if (!_inventorySystem.TryGetSlotEntity(playerEntity, "id", out var idUid))
            return;

        var cardId = idUid.Value;

        // Check if it's a PDA with an ID card inside
        if (TryComp<PdaComponent>(idUid, out var pdaComponent) && pdaComponent.ContainedId != null)
            cardId = pdaComponent.ContainedId.Value;

        // Update the ID card with nation alignment
        if (TryComp<IdCardComponent>(cardId, out var idCard))
        {
            _idCardSystem.TryChangeNationName(cardId, nationName, idCard);
        }
    }
}
