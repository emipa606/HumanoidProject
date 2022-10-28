using AbilityUser;
using Verse;

namespace HumanAbilities;

public class CompPsionicUserExample : CompAbilityUser
{
    private bool firstTick;

    private bool gaveAbilities;

    private bool IsHumanRace
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

            return hasHediff && Pawn.health.hediffSet.HasHediff(HediffDef.Named("RedLightningImpact"));
        }
    }

    public override bool TryTransformPawn()
    {
        return IsHumanRace;
    }

    public override void CompTick()
    {
        var pawn = Pawn;
        if (pawn is not { Spawned: true })
        {
            return;
        }

        if (Find.TickManager.TicksGame <= 200 || !IsHumanRace)
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
        AddPawnAbility(HumanAbilitiesDefOf.HumanPower);
    }

    public override void PostExposeData()
    {
        base.PostExposeData();
        Scribe_Values.Look(ref gaveAbilities, "gaveAbilities");
    }
}