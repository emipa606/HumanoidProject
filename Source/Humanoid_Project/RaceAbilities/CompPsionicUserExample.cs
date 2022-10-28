using AbilityUser;
using Verse;

namespace RaceAbilities;

public class CompPsionicUserExample : CompAbilityUser
{
    private bool firstTick;

    private bool gaveAbilities;

    private bool IsAvianRace
    {
        get
        {
            var pawn = Pawn;
            bool hasHediff;
            if (pawn == null)
            {
                hasHediff = false;
            }
            else
            {
                var health = pawn.health;
                hasHediff = health?.hediffSet != null;
            }

            return hasHediff && Pawn.health.hediffSet.HasHediff(HediffDef.Named("AvianHeDiff"));
        }
    }

    public override bool TryTransformPawn()
    {
        return IsAvianRace;
    }

    public override void CompTick()
    {
        var pawn = Pawn;
        if (pawn is not { Spawned: true })
        {
            return;
        }

        if (Find.TickManager.TicksGame <= 200 || !IsAvianRace)
        {
            return;
        }

        if (!firstTick)
        {
            PostInitializeTick();
        }

        base.CompTick();
    }

    private void PostInitializeTick()
    {
        var pawn = Pawn;
        if (pawn is not { Spawned: true })
        {
            return;
        }

        if (Pawn.story != null &&
            Pawn.story.DisabledWorkTagsBackstoryAndTraits.OverlapsWithOnAnyWorkType(WorkTags.Violent))
        {
            return;
        }

        firstTick = true;
        Initialize();
        if (gaveAbilities)
        {
            return;
        }

        gaveAbilities = true;
        AddPawnAbility(RaceAbilitiesDefOf.AvianPower);
    }

    public override void PostExposeData()
    {
        base.PostExposeData();
        Scribe_Values.Look(ref gaveAbilities, "gaveAbilities");
    }
}